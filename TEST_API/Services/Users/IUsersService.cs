using DarknessAwaits_API.Models;

namespace DarknessAwaits_API.Services.Users
{
    public interface IUsersService
    {
        public Task<User> InsertUserAsync(User user);
        public Task<User?> GetUserAsync(string userEmail);
    }
}
