using AutoMapper;
using Chams.Vtumanager.Fulfillment.NineMobile.Services;
using Chams.Vtumanager.Provisioning.Api.ViewModels;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.GloTopup;
using Chams.Vtumanager.Provisioning.Services.Mtn;
using Chams.Vtumanager.Provisioning.Services.NineMobileEvc;
using Chams.Vtumanager.Provisioning.Services.QueService;
using Chams.Vtumanager.Provisioning.Services.TransactionRecordService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sales_Mgmt.Services.Simplex.Api.Helpers.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Api.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    //[Authorize]
    public class vtuController : ControllerBase
    {
        private readonly ILogger<vtuController> _logger;
        private readonly IAMQService _aMQService;
        private readonly ITransactionRecordService _transactionRecordService;
        private readonly ILightEvcService _evcService;
        private readonly IGloTopupService _gloTopupService;
        private readonly IAirtelPretupsService _airtelPreupsService;
        private readonly IMtnTopupService _mtnToupService;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="aMQService"></param>
        /// <param name="transactionRecordService"></param>
        public vtuController(
            ILogger<vtuController> logger,
            IAMQService aMQService,
            ITransactionRecordService transactionRecordService,
            ILightEvcService evcService,
            IGloTopupService gloTopupService,
            IAirtelPretupsService airtelPreupsService,
            IMtnTopupService mtnToupService
            )
        {
            _logger = logger;
            _aMQService = aMQService;
            _transactionRecordService = transactionRecordService;
            _evcService = evcService;
            _gloTopupService = gloTopupService;
            _airtelPreupsService = airtelPreupsService;
            _mtnToupService = mtnToupService;

        }


        /// <summary>
        /// Submit a Virtual Top Up service request
        /// </summary>
        /// <param name="rechargeRequest"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpPost("topup")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RechargeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize]
        public async Task<IActionResult> VtuTopUp(
            RechargeRequest rechargeRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                RechargeResponse rechargeResponse = new RechargeResponse();

                if (ModelState.IsValid)
                {
                    _logger.LogInformation("API ENTRY: Inside VtuTopUp API call.");

                    var apikey = (string)HttpContext.Request.Headers["x-api-key"];

                    var partner = await _transactionRecordService.GetPartnerbyAPIkey(apikey);

                    if (partner == null)
                    {
                        rechargeResponse.status = "2001";
                        rechargeResponse.responseMessage = "Authorization Error : Invalid API KEY";
                        rechargeResponse.TransactionReference = rechargeRequest.TransactionReference;

                        return Unauthorized(new
                        {
                            rechargeResponse
                        });
                    }
                    //string prefix = rechargeRequest.PhoneNumber.Substring(0, 4);
                    //var carrier = _transactionRecordService.GetServiceProviderByPrefix(prefix);
                    //if (carrier.ServiceProviderId == 0 || carrier.ServiceProviderId != rechargeRequest.ServiceProviderId)
                    //{
                    //    return BadRequest(new
                    //    {
                    //        status = "2002",
                    //        responseMessage = "Phone Number / Carrier mismatch"
                    //    });
                    //}
                    _logger.LogInformation($"Checking current stock balance for partner:{partner.PartnerId} telco: {rechargeRequest.ServiceProviderId}");
                    int balance = 0;
                    var stockdata = await _transactionRecordService.GetPartnerStockBalance(partner.PartnerId,  rechargeRequest.ServiceProviderId);

                    if (stockdata != null)
                    {
                        balance = stockdata.QuantityOnHand;
                    }
                    if (balance < rechargeRequest.rechargeAmount)
                    {
                        _logger.LogInformation($"Not sufficient stock balance for partner:{partner.PartnerId} telco: {rechargeRequest.ServiceProviderId} is {balance}");

                        rechargeResponse.status = "2003";
                        rechargeResponse.responseMessage = "Insufficient Stock Balance";
                        rechargeResponse.TransactionReference = rechargeRequest.TransactionReference;

                        return BadRequest(new
                        {
                            rechargeResponse

                        });

                    }
                    bool isDuplicate = _transactionRecordService.IsTransactionExist(rechargeRequest.TransactionReference, partner.PartnerId);
                    if(isDuplicate)
                    {
                        rechargeResponse.status = "2004";
                        rechargeResponse.responseMessage = "Duplicate transaction";
                        rechargeResponse.TransactionReference = rechargeRequest.TransactionReference;

                        return BadRequest(new
                        {
                            rechargeResponse

                        });
                    }

                    

                    await _transactionRecordService.RecordTransaction(rechargeRequest, partner.PartnerId);

                    var response = _aMQService.SubmitTopupOrder(rechargeRequest);

                    rechargeResponse.status = "00";
                    rechargeResponse.responseMessage = "Submitted Successfully";
                    rechargeResponse.TransactionReference = rechargeRequest.TransactionReference;

                    return Ok(new
                    {

                        rechargeResponse
                    });


                }
                else
                {
 
                    

                    return BadRequest(new
                    {
                        status = "2010",
                        responseMessage = ModelState.GetErrorMessages()

                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Api failure in VtuTopUp with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    responseMessage = $"Failed to submit vtu topup for transaction reference :{rechargeRequest.TransactionReference}"

                });
            }

        }

        /// <summary>
        /// Query the status of a transaction
        /// </summary>
        /// <param name="transactionReference"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("GetTransactionbyId")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RechargeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTransactionbyId(
            QueryTransactionStatusRequest statusRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation($"API ENTRY: Inside GetTransactionbyId API call with transref {JsonConvert.SerializeObject(statusRequest)}");

                    var apikey = (string)HttpContext.Request.Headers["x-api-key"];

                    var partner = _transactionRecordService.GetPartnerbyAPIkey(apikey);

                    if (partner == null)
                    {
                        return Unauthorized(new
                        {
                            status = "2001",
                            responseMessage = "Authorization Error : Invalid API KEY"
                        });
                    }
                    //fetch transaction from db
                    var trans = await _transactionRecordService.GetTransactionById(statusRequest.ServiceProviderId, statusRequest.transactionId);
                    if(trans == null)
                    {
                        return NotFound(new
                        {
                            status = "2005",
                            responseMessage = $"Transaction reference number {statusRequest.TransactionReference} not found"
                        });
                    }
                    QueryTxnStatusResponse queryTxnStatusResponse = null;
                    switch (trans.serviceproviderid)
                    {
                        case (int)ServiceProvider.MTN:
                            queryTxnStatusResponse = await _mtnToupService.QueryTransactionStatus(statusRequest);
                            break;
                        case (int)ServiceProvider.Airtel:
                            queryTxnStatusResponse = await _airtelPreupsService.QueryTransactionStatus(statusRequest);
                            break;
                        case (int)ServiceProvider.GLO:

                            queryTxnStatusResponse = await _gloTopupService.QueryTransactionStatus(statusRequest);

                            break;
                        case (int)ServiceProvider.NineMobile:
                            queryTxnStatusResponse = await _evcService.QueryTransactionStatus(statusRequest);
                            break;

                        default:
                            break;
                    }

                    return Ok(new
                    {
                        status = "00",
                        responseMessage = "Successful",
                        data = queryTxnStatusResponse
                    });


                }
                else
                {
                    return BadRequest(new
                    {
                        status = "99",
                        responseMessage = ModelState.GetErrorMessages()

                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Api failure in GetTransactionbyId with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    responseMessage = $"Failed to submit GetTransactionbyId{JsonConvert.SerializeObject(statusRequest)}"

                });
            }

        }

        private UserModel GetCurrentuser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity != null)
            {
                var userClaims = identity.Claims;
                return new UserModel
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                };
            }
            return null;
        }


        /// <summary>
        /// Returns a list of available Databundle products by Serviceprovider
        /// </summary>
        /// <param name="rechargeRequest"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("{serviceProviderId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ProductList>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> ListProductByServiceProviderId(
            int serviceProviderId,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("API ENTRY: Inside ListProductByServiceProviderId API call.");

                    var apikey = (string)HttpContext.Request.Headers["x-api-key"];
                    var partner = _transactionRecordService.GetPartnerbyAPIkey(apikey);

                    if (partner == null)
                    {
                        return Unauthorized(new
                        {
                            status = "2001",
                            responseMessage = "Authorization Error : Invalid API KEY"
                        });
                    }

                    var products = await _transactionRecordService.ProductList(serviceProviderId);

                    var vwdata = new List<ProductList>();
                    foreach (var item in products)
                    {
                        var prod = new ProductList
                        {
                            ProductDescription = item.ProductName,
                            Price = item.Price,
                            ProductCode = item.ProductId,
                            ProductType = Enum.GetName(typeof(ProductType), item.ServiceProviderId),
                            Validity = item.Validity,
                            ServiceProviderName = Enum.GetName(typeof(ServiceProvider), item.ServiceProviderId)
                        };
                        vwdata.Add(prod);
                    }
                    return Ok(new
                    {
                        status = "00",
                        responseMessage = "Success",
                        details = vwdata
                    });


                }
                else
                {
                    return BadRequest(new
                    {
                        status = "2010",
                        responseMessage = ModelState.GetErrorMessages()

                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Api failure in ListProductByServiceProviderId with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    responseMessage = $"Failed to submit vtu topup {JsonConvert.SerializeObject(serviceProviderId)}"

                });
            }

        }


        
    }
}
