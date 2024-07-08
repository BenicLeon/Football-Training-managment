using Football.Service.Common;
using Football.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class FootballController : ControllerBase
{
    private readonly IFootballService _footballService;

    public FootballController(IFootballService footballService)
    {
        _footballService = footballService;
    }

    [Authorize(Roles = "Secretary")]
    [HttpGet("players")]
    public async Task<IActionResult> GetAllPlayers()
    {
        var players = await _footballService.GetAllPlayersAsync();
        return Ok(players);
    }
    


    [HttpGet("teamPlayers")]
    public async Task<IActionResult> GetTeamPlayers()
    {
        var players = await _footballService.GetTeamPlayersAsync();
        return Ok(players);
    }

    [Authorize(Roles = "Secretary")]
    [HttpPost("teamPlayers")]
    public async Task<IActionResult> AddPlayerToTeam([FromBody] AddPlayerToTeamModel model)
    {
        var result = await _footballService.AddPlayerToTeamAsync(model.PlayerId);
        if (!result)
        {
            return StatusCode(500, "Error adding player to team.");
        }
        return Ok();
    }

    [Authorize(Roles = "Secretary")]
    [HttpDelete("teamPlayers/{playerId}")]
    public async Task<IActionResult> RemovePlayerFromTeam(Guid playerId)
    {
        var result = await _footballService.RemovePlayerFromTeamAsync(playerId);
        if (!result)
        {
            return StatusCode(500, "Error removing player from team.");
        }
        return Ok();
    }

    [Authorize(Roles = "Secretary")]
    [HttpPut("players/{playerId}/contract")]
    public async Task<IActionResult> UpdatePlayerContract(Guid playerId, [FromBody] UpdateContractModel model)
    {
        var result = await _footballService.UpdatePlayerContractAsync(playerId, model.Contract);
        if (!result)
        {
            return StatusCode(500, "Error updating player contract.");
        }
        return Ok();
    }

    [HttpGet("teamStatus/{playerId}")]
    public async Task<IActionResult> GetTeamStatusById(Guid playerId)
    {
        var isInTeam = await _footballService.IsPlayerInTeamByIdAsync(playerId);
        return Ok(new { isInTeam });
    }

}
