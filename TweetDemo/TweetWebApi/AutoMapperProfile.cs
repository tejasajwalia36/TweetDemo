using AutoMapper;
using TweetWeb.Dto;
using TweetWeb.Model;

namespace TweetWebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Tweet, TweetDto>();
            CreateMap<TweetDto, Tweet>();

            CreateMap<User, TweetUserDto>();
            CreateMap<TweetUserDto, User>();

            CreateMap<Tweet, GetTweetDto>();
            CreateMap<GetTweetDto, Tweet>();
        }
    }
}
