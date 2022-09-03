using AutoMapper;
using Chams.Vtumanager.Provisioning.Api.ViewModels;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
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
    [Authorize]
    public class vtuController : ControllerBase
    {
        private readonly ILogger<vtuController> _logger;
        private readonly IAMQService _aMQService;
        private readonly ITransactionRecordService _transactionRecordService;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="aMQService"></param>
        /// <param name="transactionRecordService"></param>
        public vtuController(
            ILogger<vtuController> logger,
            IAMQService aMQService,
            ITransactionRecordService transactionRecordService
            )
        {
            _logger = logger;
            _aMQService = aMQService;
            _transactionRecordService = transactionRecordService;
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
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
                        return BadRequest(new
                        {
                            status = "2001",
                            message = "Authorization Error : Invalid API KEY"
                        });
                    }

                    bool isDuplicate = _transactionRecordService.IsTransactionExist(rechargeRequest.TransactionReference, partnerid);
                    if(isDuplicate)
                    {
                        return BadRequest(new
                        {
                            status = "2002",
                            message = "Duplicate transaction",
                            data = rechargeRequest.TransactionReference
                        });
                    }
                    await _transactionRecordService.RecordTransaction(rechargeRequest, partnerid);

                    var rsponse = _aMQService.SubmitTopupOrder(rechargeRequest);
                    return Ok(new
                    {
                        status = "00",
                        message = "",
                        data = rsponse
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
            string transactionReference,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("API ENTRY: Inside GetTransactionbyId API call.");

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
                _logger.LogError($"Api failure in VtuTopUp with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit vtu topup {JsonConvert.SerializeObject(transactionReference)}"

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
