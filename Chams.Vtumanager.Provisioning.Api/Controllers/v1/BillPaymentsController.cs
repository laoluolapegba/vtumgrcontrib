﻿using Chams.Vtumanager.Provisioning.Entities.BillPayments.Dstv;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.BillPayments;
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
    
    public class BillPaymentsController : Controller
    {
        private readonly ILogger<BillPaymentsController> _logger;
        
        private readonly IBillPaymentsService _billspaymentService;
        private readonly ITransactionRecordService _transactionRecordService;


        
        public BillPaymentsController(
            ILogger<BillPaymentsController> logger,
            IBillPaymentsService billspaymentService
            )
        {
            _logger = logger;
            
            _billspaymentService = billspaymentService;
        }


        [HttpPost("BillPayments")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DstvResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillPayments(
            DstvRequest rechargeRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {

                    var dstvresponse  = await _billspaymentService.DstvPaymentAsync(rechargeRequest, cancellation);

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
                    message = $"Failed to submit BillPayments {JsonConvert.SerializeObject(rechargeRequest)}"

                });
            }

        }

    }
}
