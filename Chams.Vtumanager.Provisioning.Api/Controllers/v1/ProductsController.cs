using AutoMapper;
using Chams.Vtumanager.Provisioning.Api.ViewModels;
using Chams.Vtumanager.Provisioning.Entities.Common;
using Chams.Vtumanager.Provisioning.Entities.Product;
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
    //[Authorize]
    public class productsController : ControllerBase
    {
        private readonly ILogger<productsController> _logger;
        
        private readonly ITransactionRecordService _transactionRecordService;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="aMQService"></param>
        /// <param name="transactionRecordService"></param>
        public productsController(
            ILogger<productsController> logger,
            
            ITransactionRecordService transactionRecordService
            )
        {
            _logger = logger;
            
            _transactionRecordService = transactionRecordService;
        }


        /// <summary>
        /// Returns a list of available Databundle products by Serviceprovider
        /// </summary>
        /// <param name="rechargeRequest"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("list/{serviceProviderId}")]
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
                    int partnerid = _transactionRecordService.GetPartnerIdbykey(apikey);

                    if (partnerid < 1)
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
