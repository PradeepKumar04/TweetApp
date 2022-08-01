using com.tweetapp.api.Helpers;
using com.tweetapp.application.Queries;
using com.tweetapp.application.Response;
using com.tweetapp.domain.DAOEntities;
using com.tweetapp.domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        protected readonly IUserRegisterationQuery _userRegisteration;

        public AuthenticationController(IUserRegisterationQuery userRegisteration)
        {
            _userRegisteration = userRegisteration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ApiResponse<string>> RegisterUser([FromBody] UserRegisterDAO user)
        {
            return await _userRegisteration.UserRegistartion(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ApiResponse<string>> LoginUser([FromBody] UserLoginDAO user)
        {
            return await _userRegisteration.UserLogin(user);
        }

        [HttpPut]
        [Route("ForgotPassword")]
        public async Task<ApiResponse<string>> ForgotPassword([FromBody] ForgotPasswordDAO forgotPassword)
        {
            return await _userRegisteration.ForgotPassword(forgotPassword);
            
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Route("ResetPassword")]
        public async Task<ApiResponse<string>> ResetPassword([FromBody] ResetPasswordDAO password)
        {
            //Request.Headers
            var email = GetEmailFromToken.GetEmail(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            return await _userRegisteration.ResetPassword(email,password.Password);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("Logout")]
        public async Task<ApiResponse<string>> UserLogout()
        {
            //Request.Headers
            var email = GetEmailFromToken.GetEmail(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            Response.Headers[HeaderNames.Authorization] = "";
            return await _userRegisteration.Logout(email);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("Users")]
        public async Task<ApiResponse<IEnumerable<UserDAO>>> GetAllUsers()
        {
            return await _userRegisteration.GetAllUsers();   
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("search/{username}")]
        public async Task<ApiResponse<IEnumerable<UserDAO>>> GetSearchedUserName([FromRoute]string userName)
        {
            return await _userRegisteration.GetSearchedUser(userName);
        }

    }
}
