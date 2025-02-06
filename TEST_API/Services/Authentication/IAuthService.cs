using DarknessAwaits_API.Models;

namespace DarknessAwaits_API.Services.Authentication
{
    public interface IAuthService
    {
        public Task<User?> RegisterAsync(UserDto userDto, bool instructor);
        public Task<User?> LoginAsync(UserDto userDto);
        public Task LogoutAsync(int userId);
        public Task<bool> ExistsUserAsync(UserDto userDto);
        public Task<bool> VerifyPasswordHashAsync(UserDto userDto);
    }
}
