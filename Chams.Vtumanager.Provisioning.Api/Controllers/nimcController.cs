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
using Microsoft.Extensions.Logging;
using Sales_Mgmt.Services.Simplex.Api.Helpers.Validation;
using Sales_Mgmt.Services.Simplex.Api.ViewModels;
using SalesMgmt.Services.Entities.Hpin;
using SalesMgmt.Services.Entities.ViewModels;
using SalesMgmt.Services.Services.Simplex;

namespace Sales_Mgmt.Services.Simplex.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class nimcController : ControllerBase
    {
        private readonly ISimplexService _simplexService;
        private readonly ILogger<HpinOrdersController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Validate NIN from  NIMC
        /// </summary>
        /// <param name="simplexService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public nimcController(ISimplexService simplexService,  ILogger<HpinOrdersController> logger, IMapper mapper)
        {
            _simplexService = simplexService;
            _logger = logger;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Initialize a specified channel partner
        /// </summary>
        /// <param name="nin"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpPost("{nin}")] 
        public async Task<IActionResult> ValiateNIN(
            string nin,
            CancellationToken cancellation)
        {

            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("API ENTRY: Inside Get ValiateNIN API call.");
                    if (nin != "313000000009")
                    {
                        return NotFound("NIN Not found");
                    }
                    Citizen ct = await _simplexService.ValidateNIN(nin);
                    
                    return Ok(ct);
                }
                else
                {
                    return BadRequest(ModelState.GetErrorMessages());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Api failure in ValiateNIN with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to query nin {nin}");
            }

        }

    }
}
