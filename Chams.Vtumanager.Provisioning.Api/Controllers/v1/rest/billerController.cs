using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.AbujaDisco;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.BillPayments;
using Chams.Vtumanager.Provisioning.Services.BillPayments.AbujaDisco;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Api.Controllers.v1.rest
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/rest/[controller]")]
    [ApiController]
    [Produces("application/json")]
    
    public class billerController : Controller
    {
        private readonly ILogger<billerController> _logger;
        
        private readonly IBillerPaymentsService _billspaymentService;
        private readonly ITransactionRecordService _transactionRecordService;



        public billerController(
            ILogger<billerController> logger,
            ITransactionRecordService transactionRecordService,
            IBillerPaymentsService billspaymentService
            )
        {
            _logger = logger;
            _transactionRecordService = transactionRecordService;
            _billspaymentService = billspaymentService;
        }


        [HttpPost("exchange")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BillPaymentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BillPay(BillpaymentRequest renewRequest, CancellationToken cancellation)
        {
            //public async Task<IActionResult> BillPay([FromBody] string renewRequest, CancellationToken cancellation)
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {

                /*
                 * 
                //string rawRequest = await this.Request.Content.ReadAsStringAsync();
                Encoding encoding = null;
                if (!Request.Body.CanSeek)
                {
                    // We only do this if the stream isn't *already* seekable,
                    // as EnableBuffering will create a new stream instance
                    // each time it's called
                    Request.EnableBuffering();
                }
                Request.Body.Position = 0;
                var reader = new StreamReader(Request.Body, encoding ?? Encoding.UTF8);
                var body = await reader.ReadToEndAsync().ConfigureAwait(false);
                Request.Body.Position = 0
                
                */
                //var rawRequestBody = await Request.GetRawBodyAsync();

                _logger.LogInformation($"Inside API Call biller.exchange with request : {JsonConvert.SerializeObject(renewRequest)}");

                string apikey = (string)HttpContext.Request.Headers["x-api-key"];

                //_logger.LogInformation($"my API Key : {apikey}");
                //apikey = "CHAMSS-PHExG8qddpxcKduT72VesGoa4Z";
                var partnerKey = await _transactionRecordService.GetPartnerbyAPIkey(apikey);

                if (partnerKey == null)
                {
                    return Unauthorized(new
                    {
                        status = "2001",
                        responseMessage = "Authorization Error : Invalid API KEY"
                    });
                }
                // check balance


                int billpayentsCategory = (int)ProductCategory.BillPayments;
                _logger.LogInformation($"Fetching wallet balance for partnerId {partnerKey.PartnerId}, productCategory {billpayentsCategory}");

                var epurseBalance = _transactionRecordService.GetEpurseByPartnerIdCategoryId(partnerKey.PartnerId, billpayentsCategory);
                if (epurseBalance == null)
                {
                    return Ok(new
                    {
                        status = "20008",
                        responseMessage = "Product category not authorized for this partner"
                    });
                }
                var apiresponse = await _billspaymentService.BillerPayAsync(renewRequest, cancellation);

                return Ok(new
                {
                    apiresponse
                });

                //if (ModelState.IsValid)
                //{


                //}
                //else
                //{
                //    return BadRequest(new
                //    {
                //        status = "99",
                //        message = ModelState.GetErrorMessages()

                //    });
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError($"Api failure in biller exchange with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit biller exchange {JsonConvert.SerializeObject(renewRequest)}"

                });
            }

        }


        [HttpPost("proxy")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Proxy(
            ProxyRequest proxyRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                _logger.LogInformation($"Inside API Call biller.proxy with request {JsonConvert.SerializeObject(proxyRequest)}");



                if (ModelState.IsValid)
                {
                    var apikey = (string)HttpContext.Request.Headers["x-api-key"];

                    var partner = _transactionRecordService.GetPartnerbyAPIkey(apikey);

                    if (partner == null)
                    {
                        _logger.LogInformation($"failed to fetch partner info");
                        return Unauthorized(new
                        {
                            status = "2001",
                            responseMessage = "Authorization Error : Invalid API KEY"
                        });
                    }
                    var apiRsponse = await _billspaymentService.ProxyAsync(proxyRequest, cancellation);

                    return Ok(new
                    {
                        apiRsponse
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
                _logger.LogError($"Api failure in proxy api with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit proxy api  {JsonConvert.SerializeObject(proxyRequest)}"

                });
            }

        }

        
    }
    public static class RawRequestHelper
    {
        /// <summary>
        /// this method helps to accept raw string from a request 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> GetRawBodyAsync(    this HttpRequest request,    Encoding encoding = null)
        {
            if (!request.Body.CanSeek)
            {
                // We only do this if the stream isn't *already* seekable,
                // as EnableBuffering will create a new stream instance
                // each time it's called
                request.EnableBuffering();
            }

            request.Body.Position = 0;

            var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);

            var body = await reader.ReadToEndAsync().ConfigureAwait(false);

            request.Body.Position = 0;

            return body;
        }
    }
}
