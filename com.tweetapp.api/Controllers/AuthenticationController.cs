using Azure.Messaging.ServiceBus;
using com.tweetapp.api.Helpers;
using com.tweetapp.application.Queries;
using com.tweetapp.application.Response;
using com.tweetapp.domain.DAOEntities;
using com.tweetapp.domain.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        protected readonly ILogger<AuthenticationController> _logger;
        protected readonly IConfiguration _config;

        public AuthenticationController(IUserRegisterationQuery userRegisteration, ILogger<AuthenticationController> logger, IConfiguration config)
        {
            _userRegisteration = userRegisteration;
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// Register as new user
        /// </summary>
        /// <param name="user">Request's payload</param>
        /// <returns>Register as new user</returns>
        [HttpPost]
        [Route("register")]
        public async Task<ApiResponse<string>> RegisterUser([FromBody] UserRegisterDAO user)
        {
            _logger.LogInformation($"{user.UserName} User Register");
            string message = user.UserName + " Registered successful" + " on " + DateTime.Now;
            string connectionstr = _config.GetValue<string>("sbConnString");
            var client = new ServiceBusClient(connectionstr);
            var sender = client.CreateSender("tweet-app-messaging");
            var sbmsg = new ServiceBusMessage(message);
            await sender.SendMessageAsync(sbmsg);
            return await _userRegisteration.UserRegistartion(user);
        }

        /// <summary>
        /// Logging in User
        /// </summary>
        /// <param name="user">Request's payload</param>
        /// <returns>Gets logging in</returns>
        [HttpPost]
        [Route("login")]
        public async Task<ApiResponse<string>> LoginUser([FromBody] UserLoginDAO user)
        {
            
                _logger.LogInformation($"{user.UserName} user login");
                var result = await _userRegisteration.UserLogin(user);
                
                using (var producer =
                 new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build())
                {
                    try
                    {
                        Console.WriteLine(producer.ProduceAsync("tweet_app", new Message<Null, string> { Value = user.UserName + " logged in!" })
                            .GetAwaiter()
                            .GetResult());


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Oops, something went wrong: {e}");
                    }
                    return result;
                };
        }

        /// <summary>
        /// Updates the Password
        /// </summary>
        /// <param name="forgotPassword">Request's payload</param>
        /// <returns>Updates the Password</returns>
        [HttpPut]
        [Route("ForgotPassword")]
        public async Task<ApiResponse<string>> ForgotPassword([FromBody] ForgotPasswordDAO forgotPassword)
        {
            _logger.LogInformation($"{forgotPassword.Email} forgot password action");
            return await _userRegisteration.ForgotPassword(forgotPassword);
            
        }

        /// <summary>
        /// Setting a New Password
        /// </summary>
        /// <param name="password">Request's payload</param>
        /// <returns>Setting a New password</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Route("ResetPassword")]
        public async Task<ApiResponse<string>> ResetPassword([FromBody] ResetPasswordDAO password)
        {
            //Request.Headers
            var email = GetEmailFromToken.GetEmail(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            _logger.LogInformation($"{email} reset password action");
            return await _userRegisteration.ResetPassword(email,password.Password);
        }

        /// <summary>
        /// Logging out
        /// </summary>
        /// <returns>Logging Out</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("Logout")]
        public async Task<ApiResponse<string>> UserLogout()
        {
            //Request.Headers
            var email = GetEmailFromToken.GetEmail(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            _logger.LogInformation($"{email} user logged out action");
            Response.Headers[HeaderNames.Authorization] = "";
            return await _userRegisteration.Logout(email);
        }

        /// <summary>
        /// Returns a list of AllUsers
        /// </summary>
        /// <returns>A list of AllUsers</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("Users")]
        public async Task<ApiResponse<IEnumerable<UserDAO>>> GetAllUsers()
        {
            _logger.LogInformation($"Get All Users");
            return await _userRegisteration.GetAllUsers();   
        }


        /// <summary>
        /// Returns Users by Searching with username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>List of Users by username</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("search/{username}")]
        public async Task<ApiResponse<IEnumerable<UserDAO>>> GetSearchedUserName([FromRoute]string userName)
        {
            _logger.LogInformation($"Search user by user name");
            return await _userRegisteration.GetSearchedUser(userName);
        }

    }
}
