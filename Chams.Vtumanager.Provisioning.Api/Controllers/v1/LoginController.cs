using Chams.Vtumanager.Provisioning.Api.ViewModels;
using Chams.Vtumanager.Provisioning.Entities.ViewModels;
using Chams.Vtumanager.Provisioning.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IAuthenticationService _authenticationService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="authenticationService"></param>
        public LoginController(
            IConfiguration config,
            IAuthenticationService authenticationService
            )
        {
            _config = config;
            _authenticationService = authenticationService;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = _authenticationService.Authenticate(new Entities.UserLogin { Username = userLogin.Username, Password = userLogin.Password});
            if(user != null)
            {
                var token = _authenticationService.GenerateToken(user);
                return Ok(token);
            }
            return NotFound(new
            {
                status = "00",
                message = "User Not Found",
            });
        }

        
    }
}
