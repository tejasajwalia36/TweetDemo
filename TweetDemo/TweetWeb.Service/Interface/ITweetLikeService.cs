namespace TweetWeb.Service
{
    public interface ITweetLikeService
    {
        Task<ServiceResponse<int>> TweetLike(int tweetId, string userName);
    }
}
