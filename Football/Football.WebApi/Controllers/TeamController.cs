using Football.WebApi;
using Football.Service;
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
        

        public TeamController(ILogger<TeamController> logger)
        {
            _logger = logger;
            
        }


        FootballService service = new FootballService();

        [HttpPost("AddPlayer")]
        public ActionResult Post(Player player)
        {
            try
            {
                service.PostPlayer(player);
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
               
                return Ok(service.DeletePlayer(player)); 
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
                
                return Ok(service.GetPlayer());
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
                service.GetPlayerById(id);
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
               service.UpdatePlayers(id, player);
                return Ok("Succesfully updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

