using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetWeb.Dto;
using TweetWeb.Service;

namespace TweetWebApi.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("/api/v1.0/tweets/register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var response = await this.userService.Register(userDto, userDto.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("/api/v1.0/tweets/login")]
        public async Task<IActionResult> Login(LoginIdDto loginIdDto)
        {
            var response = await this.userService.Login(loginIdDto.LoginId, loginIdDto.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("/api/v1.0/tweets/{userName}/forgot")]
        public async Task<IActionResult> Forgot(string userName, ForgotDto forgotDto)
        {
            var response = await this.userService.ForgotPassword(userName, forgotDto.Password, forgotDto.ConfirmPassword);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("/api/v1.0/tweets/users/all")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await this.userService.GetAllUsers();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("/api/v/1.0/tweets/user/search/{userName}")]
        public async Task<IActionResult> GetUsers(string userName)
        {
            var response = await this.userService.SearchUsers(userName);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
