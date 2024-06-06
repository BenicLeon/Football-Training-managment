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

namespace Football.WebApi.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {

        private readonly ILogger<TeamController> _logger;
        private IFootballService _footballService;

        public TeamController(ILogger<TeamController> logger, IFootballService footballService)
        {
            _logger = logger;
            _footballService = footballService;
            
        }

        [HttpPost("AddPlayer")]
        public ActionResult Post(Player player)
        {
            try
            {
                _footballService.PostPlayerAsync(player);
                return Ok("Succesfully added");
            }
               
            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("DeletePlayer")]
        public ActionResult Delete(Player player)
        {
            try
            {
               
                return Ok(_footballService.DeletePlayerAsync(player)); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetAllPlayers")]
        public ActionResult Get()

        {
            try
            {
                
                return Ok(_footballService.GetPlayerAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetPlayerById")]
        public ActionResult Get(Guid id)

        {
            try
            {
                _footballService.GetPlayerByIdAsync(id);
                return Ok("Succesfully updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
       

        [HttpPut("UpdatePlayer/{id}")]
        public ActionResult Put(Guid id, Player player)
        {
            try
            {
                _footballService.UpdatePlayersAsync(id, player);
                return Ok("Succesfully updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

