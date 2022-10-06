using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.PortharcourtElectric;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.BillPayments;
using Chams.Vtumanager.Provisioning.Services.BillPayments.PortHarcourt;
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
    
    public class PortHarcourtController : Controller
    {
        private readonly ILogger<PortHarcourtController> _logger;
        
        private readonly IPortHarcourtPaymentsService _billspaymentService;
        private readonly ITransactionRecordService _transactionRecordService;


        
        public PortHarcourtController(
            ILogger<PortHarcourtController> logger,
            IPortHarcourtPaymentsService billspaymentService
            )
        {
            _logger = logger;
            
            _billspaymentService = billspaymentService;
        }


        [HttpPost("postpaid")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BillPaymentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostPaid(
            PortHarcourtElectricRequest renewRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {

                    var dstvresponse  = await _billspaymentService.PortHarcourtPostpaidAsync(renewRequest, cancellation);

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
                _logger.LogError($"Api failure in BillPayments with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit BillPayments {JsonConvert.SerializeObject(renewRequest)}"

                });
            }

        }

        [HttpPost("prepaid")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BillPaymentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Prepaid(
            PortHarcourtElectricRequest renewRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {

                    var dstvresponse = await _billspaymentService.PortHarcourtPrepaidAsync(renewRequest, cancellation);

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
                _logger.LogError($"Api failure in BillPayments with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit BillPayments {JsonConvert.SerializeObject(renewRequest)}"

                });
            }

        }
    }
}
