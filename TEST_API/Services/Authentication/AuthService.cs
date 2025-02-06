using Microsoft.IdentityModel.Tokens;
using DarknessAwaits_API.Models;
using DarknessAwaits_API.Services.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace DarknessAwaits_API.Services.Authentication
{
    public class AuthService : IAuthService
    {

        private readonly IUsersService _dataAccess;
        private readonly IConfiguration _configuration;

        public AuthService(IUsersService dataAccess, IConfiguration configuration)
        {
            _dataAccess = dataAccess;
            _configuration = configuration;
        }

        public async Task<User?> RegisterAsync(UserDto userDto, bool instructor)
        {
            User user = new User();

            user.username = userDto.username;
            user.email = userDto.email;
            user.password = BCrypt.Net.BCrypt.HashPassword(userDto.password);

            User? newUser = await _dataAccess.InsertUserAsync(user);
            if (newUser != null)
            {
                return newUser;
            }
            return null;
        }

        public async Task<User?> LoginAsync(UserDto userDto)
        {
            //Usuari? user = _dataContext.Usuaris.FirstOrDefault(u => u.Email == userDto.Email);
            User? user = await _dataAccess.GetUserAsync(userDto.email);
            if (user != null)
            {
                string token = CreateToken(user);

                user.Token = token;

                return user;
            }
            return null;

        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username)
                , new Claim(ClaimTypes.Sid, user.id.ToString())
                , new Claim(ClaimTypes.Email, user.email)
                , new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        public Task LogoutAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifyPasswordHashAsync(UserDto userDto)
        {
            bool result = await ExistsUserAsync(userDto);
            if (!result)
            {
                return false;
            }

            User user = await _dataAccess.GetUserAsync(userDto.email);

            if (!BCrypt.Net.BCrypt.Verify(userDto.password, user.password))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ExistsUserAsync(UserDto userDto)
        {
            User? user = await _dataAccess.GetUserAsync(userDto.email);
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}
