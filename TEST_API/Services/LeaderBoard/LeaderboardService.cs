using Dapper;
using DarknessAwaits_API.Data;
using DarknessAwaits_API.Models;
using System.Data;

namespace DarknessAwaits_API.Services.LeaderBoard
{
    public class LeaderboardService : ILeaderboardService
    {

        private readonly DataContext _dataContext;

        public LeaderboardService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> InsertGameAsync(Game game)
        {
            var sql = "insert into Game (user, trys, miliseconds, complete) values " +
                "(@user, 1, @miliseconds, false)";
            using var connection = _dataContext.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("user", game.user);
            parameters.Add("miliseconds", game.miliseconds);
            var rowsInserted = await connection.ExecuteAsync(sql, parameters);
            if (rowsInserted == 0)
            {
                return 0;
            }

            var idGame = await GetLastInsertedGameIdAsync();

            return idGame;
        }

        private async Task<int> GetLastInsertedGameIdAsync()
        {
            var sql = "SELECT MAX(id) FROM Game";
            using var connection = _dataContext.CreateConnection();
            var id = await connection.QuerySingleAsync<int>(sql);
            if (id != 0)
            {
                return id;
            }
            else { return 0; }
        }

        public async Task<IEnumerable<Game>> GetClassification()
        {
            String sql = "SELECT Game.id, Game.user, User.username,"
                + " Game.miliseconds, Game.date"
                + " FROM Game INNER JOIN User ON Game.user = User.id"
                + " INNER JOIN (SELECT Game.user, MAX(Game.miliseconds) AS MaxTime"
                + " FROM Game"
                + " GROUP BY Game.user) A ON Game.user=A.user AND Game.miliseconds = A.MaxTime"
                + " WHERE User.id > 6"
                + " ORDER BY Game.miliseconds DESC";
            using var connection = _dataContext.CreateConnection();
            var games = await connection.QueryAsync<Game>(sql);

            return games;
        }

        public async Task<int> DeleteGameAsync(int gameId)
        {
            var sql = "DELETE FROM Game WHERE id=@id";
            using var connection = _dataContext.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("id", gameId);
            var rowsDeleted = await connection.ExecuteAsync(sql, parameters);
            if (rowsDeleted <= 0)
            {
                return 0;
            }

            return rowsDeleted;
        }
    }
}
