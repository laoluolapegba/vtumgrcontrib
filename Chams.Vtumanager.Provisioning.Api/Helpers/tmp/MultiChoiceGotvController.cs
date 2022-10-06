using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.BillPayments;
using Chams.Vtumanager.Provisioning.Services.BillPayments.Multichoice;
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
    
    public class MultiChoiceGotvController : Controller
    {
        private readonly ILogger<MultiChoiceDstvController> _logger;
        
        private readonly IMultichoicePaymentsService _billspaymentService;
        private readonly ITransactionRecordService _transactionRecordService;


        
        public MultiChoiceGotvController(
            ILogger<MultiChoiceDstvController> logger,
            IMultichoicePaymentsService billspaymentService
            )
        {
            _logger = logger;
            
            _billspaymentService = billspaymentService;
        }

        [HttpPost("request")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BillPaymentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MultichoiceGotvRequest(
            GotvRequest gotvRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {

                    var dstvresponse = await _billspaymentService.GotvRequestAsync(gotvRequest, cancellation);

                    return Ok(new
                    {
                        dstvresponse
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
                _logger.LogError($"Api failure in MultichoiceGotv with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit MultichoiceGotv {JsonConvert.SerializeObject(gotvRequest)}"

                });
            }

        }

        [HttpPost("renew")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BillPaymentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MultichoiceGotvRenew(
            GotvRenew renewRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {

                    var dstvresponse  = await _billspaymentService.GotvRenewAsync(renewRequest, cancellation);

                    return Ok(new
                    {
                        dstvresponse
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
                _logger.LogError($"Api failure in MultichoiceGotv with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit MultichoiceGotv {JsonConvert.SerializeObject(renewRequest)}"

                });
            }

        }

    }
}
