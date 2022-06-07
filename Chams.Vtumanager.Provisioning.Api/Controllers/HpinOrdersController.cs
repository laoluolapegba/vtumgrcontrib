using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sales_Mgmt.Services.Simplex.Api.Helpers.Validation;

using static System.Collections.Specialized.BitVector32;

namespace Sales_Mgmt.Services.Simplex.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class HpinOrdersController : ControllerBase
    {

        private readonly ILogger<HpinOrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _config;

        /// <summary>
        /// Submits a hybrid PIN order to simplex for processing fulfillment
        /// </summary>
        /// <param name="simplexService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public HpinOrdersController(
            IConfiguration configuration,
            ILogger<HpinOrdersController> logger, 
            IMapper mapper,
            IHttpClientFactory httpFactory)
        {
            _logger = logger;
            _mapper = mapper;
            _httpFactory = httpFactory;
            _config = configuration;
        }
        // <summary>
        /// Query a previously submitted HybridPIN PO
        /// </summary>
        /// <param name="orderNo">The purchase order number</param>
        /// <param name="cancellation"></param>
        /// <returns>HpinOrderHeader</returns>
        [HttpGet("GetOrder")]
        public async Task<IActionResult> QueryOrder(
            string orderNo,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("API ENTRY: Inside QueryOrder API call.");




                    var header = new { x= 0, y =1};//_mapper.Map<HpinPurchaseOrder, HpinOrderHeader >(order);
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
                _logger.LogError($"Api failure in PostApprovedOrder with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                {
                    status = "99",
                    message = $"Failed to submit hpin purchase order {orderNo}"

                });

               
                //return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to submit hpin purchase order {orderNo}");
            }

        }
        
    }
}
