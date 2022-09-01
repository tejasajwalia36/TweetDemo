using TweetWeb.Dto;
namespace TweetWeb.Service
{
    public interface ITweetService
    {
        Task<ServiceResponse<int>> AddTweet(TweetDto tweetDto, string userName);

        Task<ServiceResponse<int>> ReplyTweet(TweetDto tweetDto, string userName, int tweetId);

        Task<ServiceResponse<int>> UpdateTweet(int tweetid, UpdateTweetDto tweetDto, string userName);

        Task<ServiceResponse<int>> DeleteTweet(int tweetId, string userName);

        Task<ServiceResponse<List<GetTweetDto>>> GetTweets();

        Task<ServiceResponse<List<GetTweetDto>>> GetTweets(string userName);
    }
}
