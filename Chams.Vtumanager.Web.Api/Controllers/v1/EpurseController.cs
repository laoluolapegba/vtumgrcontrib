using Chams.Vtumanager.Provisioning.Entities.Epurse;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.TransactionRecordService;
using Chams.Vtumanager.Web.Api.Helpers.Validation;
using Chams.Vtumanager.Web.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace Chams.Vtumanager.Web.Api.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EpurseController : Controller
    {
        private readonly ILogger<EpurseController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionRecordService _transactionRecordService;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="transactionRecordService"></param>
        /// <param name="mapper"></param>
        public EpurseController(
            ILogger<EpurseController> logger,
            ITransactionRecordService transactionRecordService,
            IMapper mapper
            )
        {
            _logger = logger;
            _mapper = mapper;
            _transactionRecordService = transactionRecordService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpGet("{partnerId}", Name = "Get")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(EpurseAccountMaster), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAccountbyPartnerId(int partnerId,
           CancellationToken cancellation)
        {
            try
            {
                _logger.LogInformation("API ENTRY: Inside Epurse Get API call.");
                var result = _transactionRecordService.GetEpurseByPartnerId(partnerId);


                return Ok(new
                {
                    status = "00",
                    message = "Success",
                    data = result
                });
            }
            catch (Exception exp)
            {
                _logger.LogError($"Api failure in Get with error message {exp.Message}  error details {exp}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to get epurse account {JsonConvert.SerializeObject(partnerId)}"

                });
            }
        }
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<EpurseAccountMaster>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("API ENTRY: Inside Epurse Get API call.");
                var results = await _transactionRecordService.GetEpurseAccounts();

                var result = new
                {
                    Count = results.Count(),
                    Results = results
                    //Results = _mapper.Map<CampModel[]>(results)
                };
                return Ok(new
                {
                    status = "00",
                    message = "Success",
                    data = results
                });
            }
            catch (Exception exp)
            {
                _logger.LogError($"Api failure in Get with error message {exp.Message}  error details {exp}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "99",
                    message = $"Failed to getall epurse accounts"

                });
            }
        }

        /// 
        /// </summary>
        /// <param name="stockTopUpRequest"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpPut("TopUp")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(EpurseTopUpResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EpurseTopUp(
            EpurseTopUpRequest stockTopUpRequest,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation($"API ENTRY: Inside EpurseTopUp API call. request {JsonConvert.SerializeObject(stockTopUpRequest)}");

                    if (!_transactionRecordService.IsEpurseExist(stockTopUpRequest.PartnerId, stockTopUpRequest.TenantId))
                    {
                        return BadRequest(new
                        {
                            status = "99",
                            message = $"Could not find epurse account for Partner with Id {stockTopUpRequest.PartnerId}"

                        });
                    }
                    var accountTopUpRequest = new AccountTopUpRequest
                    {
                        TenantId = stockTopUpRequest.TenantId,
                        Amount = stockTopUpRequest.Amount,
                        PartnerId = stockTopUpRequest.PartnerId,
                        ProductCategoryId = stockTopUpRequest.ProductcategoryId
                    };
                    var epacct = await _transactionRecordService.CreditEpurseAccount(accountTopUpRequest);

                    return Ok(new
                    {
                        status = "00",
                        message = "Topup Successful",
                        data = epacct

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
        /// <param name="epurseAccountModel"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(EpurseAccountMaster), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Epurse(EpurseAccountModel epurseAccountModel,
            CancellationToken cancellation)
        {
            await Task.Delay(0, cancellation).ConfigureAwait(false);
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation($"API ENTRY: Inside Epurse Create API call. request:{JsonConvert.SerializeObject(epurseAccountModel)}");                    

                    bool ispartnerExists = _transactionRecordService.IsPartnerExist(epurseAccountModel.PartnerId);
                    if(!ispartnerExists)
                    {
                        return Ok(new
                        {
                            status = "01",
                            message = $"Invalid PartnerId {epurseAccountModel.PartnerId}"
                        });
                    }
                    List<EpurseAccountMaster> acctsCreated = new List<EpurseAccountMaster>();
                    foreach (int productCategory in epurseAccountModel.ProductCategoryIds)
                    {
                        EpurseAccountMaster epurseAccount = new EpurseAccountMaster
                        {
                            PartnerId = epurseAccountModel.PartnerId,
                            TenantId = epurseAccountModel.TenantId,
                            ProductcategoryId = productCategory,
                            CreatedBy = epurseAccountModel.CreatedBy,
                            AuthorisedBy = epurseAccountModel.AuthorisedBy
                        };
                        _logger.LogInformation($"Checking if account exists for category: {productCategory}");

                        var acct = _transactionRecordService.GetEpurseByPartnerIdCategoryId(epurseAccount.PartnerId, productCategory );

                        
                        if (acct == null)
                        {
                            _logger.LogInformation($"Creating Account  for category: {productCategory}");
                            var createdAccount = await _transactionRecordService.CreateEpurseAccount(epurseAccount, productCategory);
                            acctsCreated.Add(createdAccount);
                        }
                        
                        
                        

                        //if (acct != null)
                        //{
                        //    return Ok(new
                        //    {
                        //        status = "02",
                        //        message = "Partner account already exists",
                        //        data = acct
                        //    });
                        //}

                    }

                    //return Created($"/v1/api/epurse/{createdAccount.PartnerId}", createdAccount);

                    return Ok(acctsCreated);

                    //return Created(new
                    //{
                    //    status = "00",
                    //    message = "Partner already exists",
                    //    data = acct
                    //});

                    //return Created($"/v1/api/epurse/{createdAccount.PartnerId}", _mapper.Map<EpurseAccountModel>(createdAccount));

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
            catch (Exception exp)
            {
                _logger.LogError($"Api failure in Create Epurse with error message {exp.Message}  error details {exp}");
                    return StatusCode(StatusCodes.Status500InternalServerError, new
{
                    status = "99",
                    message = $"Failed to create epurse account {JsonConvert.SerializeObject(epurseAccountModel)}"

                });
            }

        }
    }
}
