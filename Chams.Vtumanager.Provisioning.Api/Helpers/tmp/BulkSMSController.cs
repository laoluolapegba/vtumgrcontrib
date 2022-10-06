using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.BulkSMS;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.BillPayments;
using Chams.Vtumanager.Provisioning.Services.BillPayments.BulkSMS;
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
    
    public class BulkSMSController : Controller
    {
        private readonly ILogger<BulkSMSController> _logger;
        
        private readonly IBulkSMSPaymentsService _billspaymentService;
        private readonly ITransactionRecordService _transactionRecordService;


        
        public BulkSMSController(
            ILogger<BulkSMSController> logger,
            IBulkSMSPaymentsService billspaymentService
            )
        {
            _logger = logger;
            
            _billspaymentService = billspaymentService;
        }


        [HttpPost("billpayments")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BillPaymentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillPayments(
            BulkSMSRequest renewRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {

                    var dstvresponse  = await _billspaymentService.BulkSMSPaymentAsync(renewRequest, cancellation);

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
                _logger.LogError($"Api failure in BulkSMSPaymentAsync with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit BulkSMSPaymentAsync {JsonConvert.SerializeObject(renewRequest)}"

                });
            }

        }

    }
}
