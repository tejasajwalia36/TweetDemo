using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TweetWeb.Dto;
using TweetWeb.Model.Data;
using Microsoft.Extensions.Configuration;
using TweetWeb.Model;

namespace TweetWeb.Service
{
    public class UserService : IUserService
    {
        private readonly TweetDbContext dataContext;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public UserService(IMapper mapper, TweetDbContext dataContext, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.dataContext = dataContext;
            this.configuration = configuration;
        }

        public async Task<ServiceResponse<List<UserDto>>> GetAllUsers()
        {
            var response = new ServiceResponse<List<UserDto>>();
            var user = dataContext.Users.ToList();
            response.Success = true;
            response.Data = this.mapper.Map<List<UserDto>>(user);
            return response;
        }

        public async Task<ServiceResponse<List<UserDto>>> SearchUsers(string loginId)
        {
            var response = new ServiceResponse<List<UserDto>>();
            var user = dataContext.Users.Where(x => x.LoginId.StartsWith(loginId)).ToList();
            response.Success = true;
            response.Data = this.mapper.Map<List<UserDto>>(user);
            return response;
        }

        public async Task<ServiceResponse<string>> Login(string loginId, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await dataContext.Users.FirstOrDefaultAsync(u => u.LoginId.ToLower().Equals(loginId.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                response.Data = "Bearer " + CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<int>> ForgotPassword(string loginId, string password, string confirmPassword)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (string.IsNullOrEmpty(loginId))
            {
                response.Success = false;
                response.Message = "Login id is required.";
            }
            else if (string.IsNullOrEmpty(password))
            {
                response.Success = false;
                response.Message = "Password is required.";
            }
            else if (string.IsNullOrEmpty(confirmPassword))
            {
                response.Success = false;
                response.Message = "Confirm password is required.";
            }
            else if (!await ConfirmPasswordCheck(password, confirmPassword))
            {
                response.Success = false;
                response.Message = "Password and confirm password should be same.";
            }
            else
            {
                User user = dataContext.Users.FirstOrDefault(x => x.LoginId == loginId);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not exists.";
                }
                else
                {
                    CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;

                    await dataContext.SaveChangesAsync();

                    response.Data = user.Id;
                    response.Message = "User password changed successfully.";
                }
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(UserDto userDto, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (!await ConfirmPasswordCheck(userDto.Password, userDto.ConfirmPassword))
            {
                response.Success = false;
                response.Message = "Password and confirm password should be same.";
            }
            else if (await LoginIdExists(userDto.LoginId))
            {
                response.Success = false;
                response.Message = "Login id already exists.";
            }
            else if (await EmailExists(userDto.Email))
            {
                response.Success = false;
                response.Message = "Email already exists.";
            }
            else
            {
                User user = this.mapper.Map<User>(userDto);
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                dataContext.Users.Add(user);
                await dataContext.SaveChangesAsync();

                response.Data = user.Id;
                response.Message = "User registed successfully.";
            }
            return response;
        }

        public async Task<bool> LoginIdExists(string loginId)
        {
            if (await dataContext.Users.AnyAsync(x => x.LoginId.ToLower() == loginId.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> EmailExists(string email)
        {
            if (await dataContext.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ConfirmPasswordCheck(string password, string confirmPassword)
        {
            if (password == confirmPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.LoginId)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(this.configuration.GetSection("TokenSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
