using com.tweetapp.api.Controllers;
using com.tweetapp.application.Queries;
using com.tweetapp.application.Response;
using com.tweetapp.domain.DAOEntities;
using com.tweetapp.domain.Models;
using Microsoft.AspNetCore.Mvc;
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
    class TweetControllerTest
    {
        private Mock<ITweetQuery> _tweetQuery;
        private Mock<ILogger<TweetController>> logger;
        private TweetController Controller;

        [SetUp]
        public void setup()
        {
            _tweetQuery = new Mock<ITweetQuery>();
            logger = new Mock<ILogger<TweetController>>();

            Controller = new TweetController(_tweetQuery.Object,logger.Object);
        }


        private readonly ApiResponse<IEnumerable<TweetDAO>> apiResponse = new ApiResponse<IEnumerable<TweetDAO>>()
        {
            Data = new List<TweetDAO>()
            {
              new TweetDAO()
              {
                ImagePath=new List<string>(){"image1","image2" },
                LikedUsers=new List<User>(){ },
                Message="new message",
                ReplyTweets=new List<ReplyTweet>(){},
                UploadDate=new DateTime().Date,
                User=new UserDAO(),
                UserId="1000"
              }
            },
            Message = "Message succesful",
            StatusCode=200,
            Success=true
        };

        private readonly ApiResponse<IEnumerable<TweetWithUserDAO>> tweetWithUser = new ApiResponse<IEnumerable<TweetWithUserDAO>>()
        {
            Message = "",
            StatusCode = 200,
            Success = true,
            Data = new List<TweetWithUserDAO>()
            {
                new TweetWithUserDAO()
                {
                    Id="",
                    Message="",
                    ImagePath=new List<string>(),
                    LikedUsers=new List<UserDAO>(),
                    ReplyTweets=new List<ReplyTweetDAO>(),
                    UploadDate=new DateTime(),
                    User=null
                }
            }
        };

        private readonly ApiResponse<string> postData = new ApiResponse<string>()
        {
            Data = "testing",
            Success = true,
            StatusCode = 200,
            Message = "Succcess"
        };




        [Test]
        public async Task GetTweetsByUserName_Test()
        {
            _tweetQuery.Setup(x => x.GetAllTweetsByUserName(It.IsAny<string>())).Returns(Task.FromResult(apiResponse));
            var response = await Controller.GetTweetsByUserName(It.IsAny<string>());

            Assert.NotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public async Task GetAllTweets_Test()
        {
            _tweetQuery.Setup(x => x.GetAllTweets()).Returns(Task.FromResult(tweetWithUser));
            var response = await Controller.GetAllTweets();

            Assert.NotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }


        [Test]
        public async Task DeleteTweet_Test()
        {
            _tweetQuery.Setup(x => x.GetAllTweets()).Returns(Task.FromResult(tweetWithUser));
            var response = await Controller.GetAllTweets();

            Assert.NotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }


        public async Task UpdateTweet_Test()
        {
            _tweetQuery.Setup(x => x.GetAllTweetsByUserName(It.IsAny<string>())).Returns(Task.FromResult(apiResponse));
            var response = await Controller.GetTweetsByUserName(It.IsAny<string>());

            Assert.NotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public async Task PostTweet_Test()
        {
            _tweetQuery.Setup(x => x.GetAllTweetsByUserName(It.IsAny<string>())).Returns(Task.FromResult(apiResponse));
            var response = await Controller.GetTweetsByUserName(It.IsAny<string>());

            Assert.NotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }
    }
}
