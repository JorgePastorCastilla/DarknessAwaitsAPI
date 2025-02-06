using Dapper;
using DarknessAwaits_API.Data;
using DarknessAwaits_API.Models;
using System.Data;

namespace DarknessAwaits_API.Services.Users
{
    public class UsersService: IUsersService
    {

        private readonly DataContext _dataContext;

        public UsersService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<User> InsertUserAsync(User user)
        {
            var query = "INSERT INTO User (username, email, password) VALUES (@username, @email, @password)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("username", user.username, DbType.String);
            parameters.Add("email", user.email, DbType.String);
            //byte[] hashBytes = System.Text.Encoding.Latin1.GetBytes(user.PasswordHash);
            //parameters.Add("PasswordHash", hashBytes, DbType.Binary);
            parameters.Add("password", user.password, DbType.String);

            using var connection = _dataContext.CreateConnection();

            var id = await connection.QuerySingleAsync<int>(query, parameters);

            var createdUser = new User
            {
                id = id,
                username = user.username,
                email = user.email,
                password = user.password
            };

            return createdUser;
        }

        public async Task<User?> GetUserAsync(string userEmail)
        {
            var sql = "select id,username,Email,Password from User where email=@email";
            var parameters = new DynamicParameters();
            parameters.Add("email", userEmail);
            using var conn = _dataContext.CreateConnection();
            try
            {
                var user = await conn.QueryFirstAsync<User>(sql, parameters);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
