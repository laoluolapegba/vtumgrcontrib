using AutoMapper;
using Chams.Vtumanager.Fulfillment.NineMobile.Services;
using Chams.Vtumanager.Provisioning.Api.ViewModels;
using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.EtopUp;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.BillPayments.AbujaDisco;
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
    public class generalservicesController : ControllerBase
    {
        private readonly ILogger<vtuController> _logger;
        private readonly IAMQService _aMQService;
        private readonly ITransactionRecordService _transactionRecordService;
        private readonly IBillerPaymentsService _billspaymentService;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="aMQService"></param>
        /// <param name="transactionRecordService"></param>
        public generalservicesController(
            ILogger<vtuController> logger,
            IAMQService aMQService,
            ITransactionRecordService transactionRecordService,
            IBillerPaymentsService billspaymentService
            )
        {
            _logger = logger;
            _aMQService = aMQService;
            _transactionRecordService = transactionRecordService;
            _billspaymentService = billspaymentService;



        }


        /// <summary>
        /// Request Wallet Balance
        /// </summary>
        /// <param name="rechargeRequest"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("balance")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize]
        public async Task<IActionResult> WalletBalance(
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("API ENTRY: Inside wallet balance API call.");

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
                    int billpayentsCategory = (int)ProductCategory.BillPayments;
                    _logger.LogInformation($"Fetching wallet balance for partnerId {partner.Id}, productCategory {billpayentsCategory}");

                    var epurseBalance =  _transactionRecordService.GetEpurseByPartnerIdCategoryId(partner.Id, billpayentsCategory);

                    if(epurseBalance == null )
                    {
                        return Ok(new
                        {
                            status = "20008",
                            responseMessage = "Product category not authorized for this partner"
                        });
                    }
                    return Ok(new
                    {
                        balance = epurseBalance.MainAcctBalance,
                        lastDeposit = epurseBalance.LastDebitDate
                    });


                }
                else
                {
                    return BadRequest(new
                    {
                        status = "2010",
                        message = ModelState.GetErrorMessages()

                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Api failure in wallet balance with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit get wallet balance "

                });
            }

        }

        /// <summary>
        /// Returns a list of available bill payment services
        /// </summary>
        /// <param name="transactionReference"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("servicelist")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceLisrResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ServiceList(
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation($"API ENTRY: Inside ServiceList API cal");

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
                    var serviceresponse = await _billspaymentService.ServiceListAsync();

                    return Ok(new
                    {
                        serviceresponse
                    });

                    

                }
                else
                {
                    return BadRequest(new
                    {
                        status = "99",
                        message = ModelState.GetErrorMessages()

                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Api failure in ServiceList with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit ServiceList"

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
                        message = ModelState.GetErrorMessages()

                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Api failure in ListProductByServiceProviderId with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit vtu topup {JsonConvert.SerializeObject(serviceProviderId)}"

                });
            }

        }


        
    }
}
