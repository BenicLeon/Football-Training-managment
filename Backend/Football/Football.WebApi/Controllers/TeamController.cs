using Football.WebApi;
using Football.Service.Common;
using Football.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using AutoMapper;

namespace Football.WebApi.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {

        private readonly ILogger<TeamController> _logger;
        private readonly IMapper _mapper;
        private IFootballService _footballService;

        public TeamController(ILogger<TeamController> logger, IFootballService footballService, IMapper mapper)
        {
            _logger = logger;
            _footballService = footballService;
            _mapper = mapper; 

        }

        [HttpPost("AddPlayer")]
        public async Task<ActionResult> Post(CreatePlayerDto createPlayerDto)
        {
            try
            {
                var player = _mapper.Map<Player>(createPlayerDto);
               await _footballService.PostPlayerAsync(player);
                return Ok("Succesfully added");
            }
               
            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("DeletePlayer")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _footballService.DeletePlayerAsync(id);
                return Ok("Successfully deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetAllPlayers")]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> Get()
        {
            try
            {
                var players = await _footballService.GetPlayerAsync();
                var playerDtos = _mapper.Map<IEnumerable<PlayerDto>>(players);

                return Ok(playerDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetPlayerById")]
        public async Task<ActionResult<PlayerDto>> Get(Guid id)

        {
            try
            {
                var player = await _footballService.GetPlayerByIdAsync(id);
                if (player == null)
                {
                    return NotFound("Player not found");
                }
                var playerDto = _mapper.Map<PlayerDto>(player);
                return Ok(playerDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPut("UpdatePlayer/{id}")]
        public async Task<ActionResult> Put(Guid id, UpdatePlayerDto updatePlayerDto)
        {
            try
            {
                var player = _mapper.Map<Player>(updatePlayerDto);
                player.Id = id;
                var updateResult = await _footballService.UpdatePlayersAsync(id, player);

                if (updateResult)
                {
                    return Ok("Successfully updated");
                }
                else
                {
                    return NotFound("Player not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetFilteredSortedPagedPlayers")]
        public async Task<ActionResult<ServiceResponse<List<Player>>>> GetFilteredSortedPagedPlayers(
    [FromQuery] FilterForPlayer filter,
    [FromQuery] Paging paging,
    [FromQuery] SortOrder sort)
        {
            var result = await _footballService.GetPlayersWithFilterPagingAndSortAsync(filter, paging, sort);
            return Ok(result);
        }



    }
}

