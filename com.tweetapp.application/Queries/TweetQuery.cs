using AutoMapper;
using com.tweetapp.application.Response;
using com.tweetapp.domain.DAOEntities;
using com.tweetapp.domain.Models;
using com.tweetapp.infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.application.Queries
{
    public interface ITweetQuery
    {
        public Task<ApiResponse<string>> PostTweet(TweetDAO tweet,int id);
        public Task<ApiResponse<IEnumerable<TweetDAO>>> GetAllMyTweets(string email);
        public Task<ApiResponse<IEnumerable<TweetWithUserDAO>>> GetAllTweets();
    }
    public class TweetQuery : ITweetQuery
    {
        protected readonly ITweetRepository _tweetRepository;
        protected readonly IUserRepository _userRepository;
        protected readonly IMapper _mapper;

        public TweetQuery(ITweetRepository tweetRepository, IMapper mapper, IUserRepository userRepository)
        {
            _tweetRepository = tweetRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<TweetDAO>>> GetAllMyTweets(string email)
        {
            if (email == null)
            {
                return new ApiResponse<IEnumerable<TweetDAO>>()
                {
                    Message = "Please Fill all the Mandatory Fields",
                    StatusCode = 500,
                    Success = false,
                    Data = null
                };
            }
            else
            {
                var user =await _userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    return new ApiResponse<IEnumerable<TweetDAO>>()
                    {
                        Message = "Email not found",
                        StatusCode = 500,
                        Success = false,
                        Data = null
                    };
                }
                else
                {
                    var myTweets = await _tweetRepository.ViewMyTweets(email);
                    var userTweet = _mapper.Map<IEnumerable<TweetDAO>>(myTweets);
                    return new ApiResponse<IEnumerable<TweetDAO>>()
                    {
                        Message = "Tweets Retried Successful.",
                        StatusCode = 200,
                        Success = true,
                        Data = userTweet
                    };
                }
            }
        }

        public async Task<ApiResponse<IEnumerable<TweetWithUserDAO>>> GetAllTweets()
        {
            var allTweets = await _tweetRepository.ViewAllTweets();
            ICollection<TweetWithUserDAO> tweets = new List<TweetWithUserDAO>();
            foreach (var item in allTweets)
            {
                TweetWithUserDAO tweetWithUser = new TweetWithUserDAO();
                tweetWithUser.CreatedDate = item.CreatedDate;
                tweetWithUser.Message = item.Message;
                tweetWithUser.UserName = item.User.FirstName;
                tweets.Add(tweetWithUser);
            }
            return new ApiResponse<IEnumerable<TweetWithUserDAO>>()
            {
                Message = "All Tweets Retried Successful.",
                StatusCode = 200,
                Success = true,
                Data = tweets
            };
        }

        public async Task<ApiResponse<string>> PostTweet(TweetDAO tweet, int id)
        {
            if(tweet==null || tweet.Message==null || tweet.CreatedDate == null)
            {
                return new ApiResponse<string>()
                {
                    Message = "Please Fill all the Mandatory Fields",
                    StatusCode = 500,
                    Success = false,
                    Data = null
                };
            }
            else
            {
              var userTweet = _mapper.Map<Tweet>(tweet);
              userTweet.UserId = id;
              var isChanged = await _tweetRepository.PostTweet(userTweet);
              if (isChanged)
               {
                    return new ApiResponse<string>()
                    {
                        Message = "Tweet Posted Successful.",
                        StatusCode = 200,
                        Success = true,
                        Data = null
                    };
                }
                else
                {
                    return new ApiResponse<string>()
                    {
                        Message = "Something Went wrong! please check again....",
                        StatusCode = 401,
                        Success = false,
                        Data = null
                    };
                }
            }
        }
    }
}
