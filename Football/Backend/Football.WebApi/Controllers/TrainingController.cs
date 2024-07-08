using Football.Model;
using Football.Service;
using Football.Service.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Football.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TrainingController : ControllerBase
    {
        private readonly ITrainingService _trainingService;
        private readonly IFootballService _footballService;

        public TrainingController(ITrainingService trainingService, IFootballService footballService)
        {
            _trainingService = trainingService;
            _footballService = footballService;
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> GetAllTrainingSessions()
        {
            var sessions = await _trainingService.GetAllTrainingSessionsAsync();
            return Ok(sessions);
        }

        [HttpGet("sessions/{id}")]
        public async Task<IActionResult> GetTrainingSessionById(int id)
        {
            var session = await _trainingService.GetTrainingSessionByIdAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            return Ok(session);
        }

        [HttpPost("sessions")]
        public async Task<IActionResult> AddTrainingSession([FromBody] TrainingSession session)
        {
            var result = await _trainingService.AddTrainingSessionAsync(session);
            if (!result)
            {
                return BadRequest("Failed to add session.");
            }
            return Ok();
        }

        [HttpPut("sessions/{id}")]
        public async Task<IActionResult> UpdateTrainingSession(int id, [FromBody] TrainingSession session)
        {
            if (id != session.Id)
            {
                return BadRequest("Session ID mismatch.");
            }

            var result = await _trainingService.UpdateTrainingSessionAsync(session);
            if (!result)
            {
                return BadRequest("Failed to update session.");
            }
            return Ok();
        }

        [HttpDelete("sessions/{id}")]
        public async Task<IActionResult> DeleteTrainingSession(int id)
        {
            var result = await _trainingService.DeleteTrainingSessionAsync(id);
            if (!result)
            {
                return BadRequest("Failed to delete session.");
            }
            return Ok();
        }

        [HttpPost("sessions/{id}/attend")]
        public async Task<IActionResult> AttendSession(int id, [FromBody] AttendRequest request)
        {
            var result = await _trainingService.AddPlayerToTrainingSessionAsync(request.PlayerId, id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("sessions/{id}/unattend")]
        public async Task<IActionResult> UnattendSession(int id, [FromBody] AttendRequest request)
        {
            var result = await _trainingService.RemovePlayerFromTrainingSessionAsync(request.PlayerId, id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("my-sessions/{playerId}")]
        public async Task<IActionResult> GetSessionsByPlayer(Guid playerId)
        {
            var sessions = await _trainingService.GetTrainingSessionsByPlayerIdAsync(playerId);
            if (sessions == null)
            {
                return NotFound();
            }
            return Ok(sessions);
        }

        [HttpGet("sessions/{id}/players")]
        public async Task<IActionResult> GetPlayersByTrainingSessionId(int id)
        {
            var players = await _trainingService.GetPlayersByTrainingSessionIdAsync(id);
            return Ok(players);
        }
        [HttpPost("team-players/{playerId}/add-private-training")]
        public async Task<IActionResult> AddPrivateTraining(Guid playerId, [FromBody] string youtubeLink)
        {
            try
            {
                await _trainingService.AddPrivateTraining(playerId, youtubeLink);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("team-players/{playerId}/private-trainings")]
        public async Task<IActionResult> GetPrivateTrainings(Guid playerId)
        {
            try
            {
                var links = await _trainingService.GetPrivateTrainings(playerId);
                return Ok(links);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}

