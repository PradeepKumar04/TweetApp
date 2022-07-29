using AutoMapper;
using com.tweetapp.domain.DAOEntities;
using com.tweetapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.application.Mapper
{
    public class DAOMapping : Profile
    {
        public DAOMapping()
        {
            CreateMap<TweetDAO,Tweet>().ReverseMap();
            CreateMap<ReplyTweetDAO, ReplyTweet>().ReverseMap();
            CreateMap<UserDAO, User>().ReverseMap();
        }
    }
}
