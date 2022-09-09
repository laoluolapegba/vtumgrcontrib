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
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("API ENTRY: Inside VtuTopUp API call.");

                    var apikey = (string)HttpContext.Request.Headers["x-api-key"];

                    int partnerid = _transactionRecordService.GetPartnerIdbykey(apikey);

                    if (partnerid < 1)
                    {
                        return Unauthorized(new
                        {
                            status = "2001",
                            responseMessage = "Authorization Error : Invalid API KEY"
                        });
                    }

                    bool isDuplicate = _transactionRecordService.IsTransactionExist(rechargeRequest.TransactionReference, partnerid);
                    if(isDuplicate)
                    {
                        return BadRequest(new
                        {
                            status = "2002",
                            responseMessage = "Duplicate transaction",
                            details = rechargeRequest.TransactionReference
                        });
                    }
                    await _transactionRecordService.RecordTransaction(rechargeRequest, partnerid);

                    var response = _aMQService.SubmitTopupOrder(rechargeRequest);
                    return Ok(new
                    {
                        status = "00",
                        responseMessage = "Submitted Successfully",
                        details = response
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
                _logger.LogError($"Api failure in VtuTopUp with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit vtu topup {JsonConvert.SerializeObject(rechargeRequest)}"

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

                    int partnerid = _transactionRecordService.GetPartnerIdbykey(apikey);

                    if (partnerid < 1)
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
                            responseMessage = $"Tranasction reference number {statusRequest.TransactionReference} not found"
                        });
                    }
                    switch (trans.serviceproviderid)
                    {
                        case (int)ServiceProvider.MTN:
                            _mtnToupService.QueryTransactionStatus(statusRequest);
                            break;
                        case (int)ServiceProvider.Airtel:

                            _airtelPreupsService.QueryTransactionStatus(statusRequest);
                            break;
                        case (int)ServiceProvider.GLO:
                            _gloTopupService.QueryTransactionStatus(statusRequest);
                            break;
                        case (int)ServiceProvider.NineMobile:
                            break;

                        default:
                            break;
                    }

                    


                    var header = new { x = 0, y = 1 };//_mapper.Map<HpinPurchaseOrder, HpinOrderHeader >(order);
                    return Ok(new
                    {
                        status = "00",
                        message = "",
                        data = header
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
                _logger.LogError($"Api failure in GetTransactionbyId with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit vtu topup {JsonConvert.SerializeObject(statusRequest)}"

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
    }
}
