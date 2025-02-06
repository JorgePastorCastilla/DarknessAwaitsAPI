using DarknessAwaits_API.Models;

namespace DarknessAwaits_API.Services.LeaderBoard
{
    public interface ILeaderboardService
    {        public Task<IEnumerable<Game>> GetClassification();
        public Task<int> InsertGameAsync(Game game);
        public Task<int> DeleteGameAsync(int gameId);
    }
}
