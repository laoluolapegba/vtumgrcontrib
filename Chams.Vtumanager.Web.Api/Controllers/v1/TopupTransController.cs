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

namespace Chams.Vtumanager.Web.Api.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TopupTransController : Controller
    {
        private readonly ILogger<TopupTransController> _logger;
        private readonly ITransactionRecordService _transactionRecordService;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>

        /// <param name="transactionRecordService"></param>
        public TopupTransController(
            ILogger<TopupTransController> logger,
            ITransactionRecordService transactionRecordService
            )
        {
            _logger = logger;

            _transactionRecordService = transactionRecordService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockTopUpRequest"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpPost("AccountTopUp")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StockTopUpResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VtuTopUp(
            StockTopUpRequest stockTopUpRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("API ENTRY: Inside AccountTopUp API call.");

                    if (!_transactionRecordService.IsPartnerExist(stockTopUpRequest.PartnerId))
                    {
                        return BadRequest(new
                        {
                            status = "99",
                            message = "Invalid Account Code"

                        });
                    }
                    var accountTopUpRequest = new AccountTopUpRequest
                    {
                        ServiceProviderId = stockTopUpRequest.ServiceProviderId,
                        Amount = stockTopUpRequest.Amount,
                        PartnerId = stockTopUpRequest.PartnerId
                    };
                    await _transactionRecordService.TopUpBusinessAccount(accountTopUpRequest);
                    
                    return Ok(new
                    {
                        status = "00",
                        message = "Topup Succeeded",
                        
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
                _logger.LogError($"Api failure in TopUpBusinessAccount with error message {ex.Message}  error details {ex}");
return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit dealer stock topup {JsonConvert.SerializeObject(stockTopUpRequest)}"

                });
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="serviceProviderId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("GetAccountBalancebyPartnerId/{partnerId}/{serviceProviderId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AccountBalancesResponseView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAccountBalancebyPartnerId(
            string partnerId, int serviceProviderId ,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("API ENTRY: Inside GetAccountBalancesbyPartnerId API call.");
                    AccountBalRequest accountBalRequest = new AccountBalRequest
                    {
                        PartnerId = partnerId,
                        ServiceProviderId = serviceProviderId
                    };

                    var balance = await _transactionRecordService.GetEpurseAccountBal(accountBalRequest);
                    if(balance != null)
                    {
                        AccountBalancesResponseView acctBal = new AccountBalancesResponseView
                        {
                            CommissionAcctBalance = balance.commision_account_balance,
                            AccountCode = partnerId,
                            MainAcctBalance = balance.main_account_balance,
                            ServiceProviderId = balance.serviceprovider_id
                        };
                        return Ok(new
                        {
                            status = "00",
                            message = "",
                            data = acctBal
                        });
                    }
                    else
                    {
                        return NotFound(
                            new
                        {
                            status = "00",
                            message = "",
                            
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
                _logger.LogError($"Api failure in VtuTopUp with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit GetAccountBalancebyPartnerId {JsonConvert.SerializeObject(partnerId)}"

                });
            }

        }

        /// <summary>
        /// Returns All epurse accounts balance for a selected Business Account
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("GetAllAccountBalancebyPartnerId")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<AccountBalancesResponseView>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAccountBalancebyPartnerId(
            string partnerId,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("API ENTRY: Inside GetAllAccountBalancebyPartnerId API call.");

                    List<AccountBalancesResponseView> lstBalances = new List<AccountBalancesResponseView>();

                    var balances = await _transactionRecordService.GetAllEpurseAccounts(partnerId);
                    if (balances.Count > 0)
                    {
                        foreach (var item in balances)
                        {
                            AccountBalancesResponseView acctBal = new AccountBalancesResponseView
                            {
                                CommissionAcctBalance = item.commision_account_balance,
                                AccountCode = partnerId,
                                MainAcctBalance = item.main_account_balance,
                                ServiceProviderId = item.serviceprovider_id
                            };
                            lstBalances.Add(acctBal);
                        }

                        return Ok(new
                        {
                            status = "00",
                            message = "",
                            data = lstBalances
                        });
                    }
                    else
                    {
                        return NotFound(new
                        {
                            status = "00",
                            message = "",
                            
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
                _logger.LogError($"Api failure in VtuTopUp with error message {ex.Message}  error details {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to submit GetAllAccountBalancebyPartnerId {JsonConvert.SerializeObject(partnerId)}"

                });
            }

        }

    }

}
