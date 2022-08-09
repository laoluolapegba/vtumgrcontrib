using Chams.Vtumanager.Provisioning.Api.ViewModels;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.Authentication;
using Chams.Vtumanager.Provisioning.Services.TransactionRecordService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Api.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<LoginController> _logger;
        private readonly IAuthenticationService _authenticationService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="authenticationService"></param>
        public LoginController(
            IConfiguration config,
            IAuthenticationService authenticationService,
            ILogger<LoginController> logger
            )
        {
            _config = config;
            _logger = logger;
            _authenticationService = authenticationService;
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            //IActionResult response = Unauthorized();
            var authModel = new AuthenticationModel();
            var user = _authenticationService.Authenticate(new Entities.UserLogin { Username = userLogin.Username, Password = userLogin.Password});
            
            if(user != null)
            {
                var isveriified = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password);
                if (!isveriified)
                {
                    authModel.IsAuthenticated = false;
                    authModel.Message = $"Incorrect Credentials";
                    return Unauthorized(new
                    {
                        authModel
                    });
                }
                _logger.LogInformation($"Generate token for : {JsonConvert.SerializeObject(user)}");
                var tokenString = _authenticationService.GenerateToken(user);
                authModel.IsAuthenticated = true;
                authModel.Email = user.EmailAddress;
                authModel.UserName = user.Username;
                authModel.Token = tokenString;
                authModel.Message = "Success";




                return Ok(new { authModel });
            }

            authModel.IsAuthenticated = false;
            authModel.Message = $"No Accounts Registered with {userLogin.Username}.";
            return NotFound(new
            {
                authModel
            });
            
            //return response;
        }

        
    }
}
