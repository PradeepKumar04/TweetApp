using com.tweetapp.api.Controllers;
using com.tweetapp.application.Queries;
using com.tweetapp.application.Response;
using com.tweetapp.domain.DAOEntities;
using com.tweetapp.domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.tweet.application.Test.Controller
{
    [TestFixture]
    public class AuthenticationControllerTest
    {
        private Mock<IUserRegisterationQuery> _userQuery;
        private Mock<ILogger<AuthenticationController>> logger;
        private AuthenticationController Controller;

        

             private readonly ApiResponse<IEnumerable<UserDAO>> tweetWithUser = new ApiResponse<IEnumerable<UserDAO>>()
             {
                 Message = "",
                 StatusCode = 200,
                 Success = true,
                 Data = new List<UserDAO>()
            {
               new UserDAO()
               {
                   Email="sample@gmail.com",
                   FirstName="sample",
                   Gender="1",
                   LastName="test1",
                   LastSeen=null,
                   PhoneNumber="8838383838",
                   UserName="sampletest"
               }
            }
             };

        private readonly ApiResponse<IEnumerable<UserDAO>> tweetWithUser1 = new ApiResponse<IEnumerable<UserDAO>>()
        {
            Message = "",
            StatusCode = 200,
            Success = true,
            Data = new List<UserDAO>()
            {
               new UserDAO()
               {
                   Email="sample@gmail.com",
                   FirstName="sample",
                   Gender="1",
                   LastName="test1",
                   LastSeen=null,
                   PhoneNumber="8838383838",
                   UserName="sampletest"
               }
            }
        };

        private readonly ApiResponse<string> logout = new ApiResponse<string>()
        {
            Data = "data",
            Message = "test1",
            StatusCode = 200,
            Success = true
        };



        [SetUp]
        public void setup()
        {
            _userQuery = new Mock<IUserRegisterationQuery>();
            logger = new Mock<ILogger<AuthenticationController>>();
            Controller = new AuthenticationController(_userQuery.Object, logger.Object);
        }

        [Test]
        public async Task GetSearchedUserName_Test()
        {
            _userQuery.Setup(x => x.GetSearchedUser(It.IsAny<string>())).Returns(Task.FromResult(tweetWithUser));
            var response = await Controller.GetSearchedUserName(It.IsAny<string>());

            Assert.NotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public async Task GetAllUsers_Test()
        {
            _userQuery.Setup(x => x.GetAllUsers()).Returns(Task.FromResult(tweetWithUser));
            var response = await Controller.GetAllUsers();

            Assert.NotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }
        [Test]
        public async Task UserLogout_Test()
        {
            _userQuery.Setup(x => x.GetSearchedUser(It.IsAny<string>())).Returns(Task.FromResult(tweetWithUser));
            var response = await Controller.GetSearchedUserName(It.IsAny<string>());

            Assert.NotNull(response);
            Assert.AreEqual(200, response.StatusCode);

    }
}
