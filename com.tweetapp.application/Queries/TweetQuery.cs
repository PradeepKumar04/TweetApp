using AutoMapper;
using com.tweetapp.application.Response;
using com.tweetapp.domain.DAOEntities;
using com.tweetapp.domain.Models;
using com.tweetapp.infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.application.Queries
{
    public interface ITweetQuery
    {
        public Task<ApiResponse<string>> PostTweet(TweetDAO tweet,string id);
        public Task<ApiResponse<IEnumerable<TweetDAO>>> GetAllTweetsByUserName(string username);
        public Task<ApiResponse<IEnumerable<TweetWithUserDAO>>> GetAllTweets();
        public Task<ApiResponse<string>> LikeTweet(string tweetId, string userId);
        public Task<ApiResponse<string>> ReplyTweet(string userName , string tweetId, string message);
        public Task<ApiResponse<bool>> DeleteTweet(string id);
        public Task<ApiResponse<bool>> UpdateTweet(string id, string username, TweetDAO tweet);
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

        public async Task<ApiResponse<IEnumerable<TweetDAO>>> GetAllTweetsByUserName(string username)
        {
            if (username == null)
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
                var user =await _userRepository.GetUserByUserName(username);
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
                    var myTweets = await _tweetRepository.ViewMyTweets(username);
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
                tweetWithUser.UploadDate = item.UploadDate;
                tweetWithUser.Message = item.Message;
                tweetWithUser.ImagePath = item.ImagePath;
                tweetWithUser.LikedUsers = _mapper.Map<List<UserDAO>>(item.LikedUsers);
                tweetWithUser.ReplyTweets =_mapper.Map<List<ReplyTweetDAO>>(item.ReplyTweets);
                tweetWithUser.User = _mapper.Map<UserDAO>(item.User);
                tweetWithUser.Id = item.Id;
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

        public async Task<ApiResponse<string>> PostTweet(TweetDAO tweet, string id)
        {
            if(tweet==null || tweet.Message==null || tweet.UploadDate == null)
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
              var user = await _userRepository.GetUserById(id);
               userTweet.User = user;
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

        public async Task<ApiResponse<string>> LikeTweet(string userName, string tweetId)
        {
            if(tweetId==null || userName == null)
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
                var user = await _userRepository.GetUserByUserName(userName);
                if (user == null)
                {
                    return new ApiResponse<string>()
                    {
                        Message = "User not found",
                        StatusCode = 500,
                        Success = false,
                        Data = null
                    };
                }
                var tweet = await _tweetRepository.GetTweetByID(tweetId);
                if(tweet == null)
                {
                    return new ApiResponse<string>()
                    {
                        Message = "Tweet not found",
                        StatusCode = 500,
                        Success = false,
                        Data = null
                    };
                }
                var isUserLiked = tweet.LikedUsers.Where(s => s.Id == user.Id).FirstOrDefault();
                if (isUserLiked==null)
                {
                    tweet.LikedUsers.Add(user);
                }
                else
                {
                    tweet.LikedUsers.Remove(isUserLiked);
                }
                var isUpdated= await _tweetRepository.ReplyOrLikeTweet(tweet);
                if (isUpdated)
                {
                    return new ApiResponse<string>()
                    {
                        Message = isUserLiked==null?"User Liked the tweet":"User unliked the tweet",
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

        public async Task<ApiResponse<string>> ReplyTweet(string userName, string tweetId, string message)
        {
            if (tweetId == null || userName == null)
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
                var user = await _userRepository.GetUserByUserName(userName);
                if (user == null)
                {
                    return new ApiResponse<string>()
                    {
                        Message = "User not found",
                        StatusCode = 500,
                        Success = false,
                        Data = null
                    };
                }
                var tweet = await _tweetRepository.GetTweetByID(tweetId);
                if (tweet == null)
                {
                    return new ApiResponse<string>()
                    {
                        Message = "Tweet not found",
                        StatusCode = 500,
                        Success = false,
                        Data = null
                    };
                }
                ReplyTweet replyTweet = new ReplyTweet()
                {
                    ReplyMessage = message,
                    User = user
                };
                tweet.ReplyTweets.Add(replyTweet);
                var isUpdated = await _tweetRepository.ReplyOrLikeTweet(tweet);
                if (isUpdated)
                {
                    return new ApiResponse<string>()
                    {
                        Message = "User Replyed to Tweet",
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

        public async Task<ApiResponse<bool>> DeleteTweet(string id)
        {
            if(id=="" || id == null)
            {
                return new ApiResponse<bool>()
                {
                    Message = "Tweet not found",
                    StatusCode = 500,
                    Success = false,
                    Data = false
                };
            }
            var tweet = await _tweetRepository.GetTweetByID(id);
            if (tweet == null)
            {
                return new ApiResponse<bool>()
                {
                    Message = "Tweet not found",
                    StatusCode = 500,
                    Success = false,
                    Data = false
                };
            }
           var result= await _tweetRepository.DeleteTweet(id);
            return new ApiResponse<bool>()
            {
                Message = result ? "Tweet Deleted Successfully." :"Something went wrong!",
                StatusCode = result ? 200:500,
                Success = result,
                Data = false
            };

        }

        public async Task<ApiResponse<bool>> UpdateTweet(string id, string username, TweetDAO tweet1)
        {
            if (id == "" || id == null || username=="" || username==null)
            {
                return new ApiResponse<bool>()
                {
                    Message = "Tweet not found",
                    StatusCode = 500,
                    Success = false,
                    Data = false
                };
            }
            var tweet = await _tweetRepository.GetTweetByID(id);
            if (tweet == null)
            {
                return new ApiResponse<bool>()
                {
                    Message = "Tweet not found",
                    StatusCode = 500,
                    Success = false,
                    Data = false
                };
            }

            tweet.ImagePath = tweet1.ImagePath;
            tweet.LikedUsers = tweet1.LikedUsers;
            tweet.Message = tweet1.Message;
            tweet.ReplyTweets = tweet1.ReplyTweets;
           var result = await _tweetRepository.ReplyOrLikeTweet(tweet);
            return new ApiResponse<bool>()
            {
                Message = result ? "Tweet Updated Successfully." : "Something went wrong!",
                StatusCode = result ? 200 : 500,
                Success = result,
                Data = false
            };

        }
    }
}
