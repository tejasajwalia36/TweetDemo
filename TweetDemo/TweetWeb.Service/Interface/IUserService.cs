using TweetWeb.Dto;
using TweetWeb.Model;

namespace TweetWeb.Service
{
    public interface IUserService
    {
        Task<ServiceResponse<List<UserDto>>> GetAllUsers();

        Task<ServiceResponse<List<UserDto>>> SearchUsers(string loginId);

        Task<ServiceResponse<int>> Register(UserDto user, string password);

        Task<ServiceResponse<string>> Login(string loginId, string password);

        Task<ServiceResponse<int>> ForgotPassword(string loginId, string password, string confirmPassword);

        Task<bool> LoginIdExists(string loginId);

        Task<bool> EmailExists(string email);

        Task<bool> ConfirmPasswordCheck(string password, string confirmPassword);
    }
}
