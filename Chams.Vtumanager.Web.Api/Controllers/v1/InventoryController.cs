using Chams.Vtumanager.Provisioning.Services.TransactionRecordService;
using Chams.Vtumanager.Web.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sales_Mgmt.Services.Simplex.Api.Helpers.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using System.Security.Claims;
using Chams.Vtumanager.Web.Api.Helpers.Validation;

namespace Chams.Vtumanager.Web.Api.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InventoryController : Controller
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly ITransactionRecordService _transactionRecordService;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>

        /// <param name="transactionRecordService"></param>
        public InventoryController(
            ILogger<InventoryController> logger,
            ITransactionRecordService transactionRecordService
            )
        {
            _logger = logger;

            _transactionRecordService = transactionRecordService;
        }
       
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partnerId"></param>
        
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("GetInventorybyPartnerId/{partnerId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<StockBalanceView>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInventorybyPartnerId(
            int partnerId, 
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("API ENTRY: Inside GetInventorybyPartnerId API call.");

                    var balances = await _transactionRecordService.GetStockbalancesbyPartnerId(partnerId);
                    return Ok(new
                    {
                        status = "00",
                        message = "Success",
                        data = balances
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
                _logger.LogError($"Api failure in GetInventorybyPartnerId with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit GetInventorybyPartnerId {JsonConvert.SerializeObject(partnerId)}"

                });
            }

        }


        /// <summary>
        /// Purchase Stock using 
        /// </summary>
        /// <param name="stockPurchaseRequest"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpPost("StockPurchase")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StockBalanceView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> StockPurchase(
            StockPurchaseRequest stockPurchaseRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    if(stockPurchaseRequest.items.Count() < 1)
                    {
                        return BadRequest(new
                        {
                            status = "99",
                            message = "Order must contain at least 1 item"

                        });
                    }

                    _logger.LogInformation("API ENTRY: Inside StockPurchase API call.");
                    StockPurchaseOrder stockPurchaseOrder = new StockPurchaseOrder
                    {
                        PartnerId = stockPurchaseRequest.PartnerId,
                        tenantId = stockPurchaseRequest.tenantId,
                        UserId = stockPurchaseRequest.UserId,
                        ProductCategoryId = stockPurchaseRequest.ProductCategoryId

                    };
                    var orderdetails = new List<StockPurchaseOrder.Item>();
                    decimal orderTotal = 0;
                    foreach (var item in stockPurchaseRequest.items)
                    {
                        StockPurchaseOrder.Item orderdetail = new StockPurchaseOrder.Item();
                        orderdetail.Quantity = item.Quantity;
                        orderdetail.ServiceProviderId = item.ServiceProviderId;
                        orderdetails.Add(orderdetail);
                        orderTotal += item.Quantity;

                    }
                    stockPurchaseOrder.items = orderdetails;

                    var partnerAccount = _transactionRecordService.GetEpurseByPartnerIdCategoryId(stockPurchaseRequest.PartnerId, stockPurchaseRequest.ProductCategoryId);
                    if(partnerAccount!=null)
                    {
                        decimal balance = partnerAccount.MainAcctBalance;

                        if (balance < orderTotal)
                        {
                            return Ok(new
                            {
                                status = "03",
                                message = "Insufficient Balance",

                            });
                        }

                        var purchaseStatus = await _transactionRecordService.PurchaseStock(stockPurchaseOrder, true);
                        if (purchaseStatus)
                        {
                            return Ok(new
                            {
                                status = "00",
                                message = "Success",
                                //data = balances
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                status = "04",
                                message = "Stock Purchase failed",
                                //data = balances
                            });
                        }
                    }
                    else
                    {
                        return Ok(new
                        {
                            status = "99",
                            message = "Invalid Epurse Account",
                            //data = balances
                        });
                    }
                    

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
                _logger.LogError($"Api failure in StockPurchase with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit StockPurchase {JsonConvert.SerializeObject(stockPurchaseRequest)}"

                });
            }

        }


    }

}
