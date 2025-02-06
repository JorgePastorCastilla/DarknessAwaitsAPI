using System.Security.Claims;
using DarknessAwaits_API.Services.LeaderBoard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DarknessAwaits_API.Models;
using DarknessAwaits_API.Services;
namespace DarknessAwaits_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {

        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardL1Service)
        {
            _leaderboardService = leaderboardL1Service;
        }

        [HttpPost("InsertGame"), Authorize]
        public async Task<ActionResult<Game>> InsertGameLevel1(Game game)
        {
            var userId = int.Parse(User?.FindFirstValue(ClaimTypes.Sid));
            game.user = userId;
            int id = await _leaderboardService.InsertGameAsync(game);
            if (id == 0)
            {
                return BadRequest("Error inserting game for user " + game.user);
            }
            game.id = id;
            return Ok(game);
        }

        [HttpGet("GetClassification")]
        public async Task<ActionResult<IEnumerable<Game>>> GetClassification()
        {
            var games = await _leaderboardService.GetClassification();
            return Ok(games);
        }

        [HttpPost("DeleteGame"), Authorize]
        public async Task<ActionResult> DeleteGame(int gameId)
        {
            int rowsDeleted = await _leaderboardService.DeleteGameAsync(gameId);
            return Ok(rowsDeleted);
        }
    }
}
