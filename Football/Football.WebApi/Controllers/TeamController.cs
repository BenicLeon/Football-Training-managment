using Football.WebApi;
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

        string connString = "Host=localhost;Username=postgres;Password=fcfullam13;Database=footballPlayers";



        [HttpPost("AddPlayer")]
        public ActionResult Post(Player player)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                var commandText = "INSERT INTO \"Players\" VALUES (@player_id,@team_id, @player_name,@position,@number,@age,@nationality);";

                using var command = new NpgsqlCommand(commandText, conn);

                command.Parameters.AddWithValue("@player_id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                command.Parameters.AddWithValue("@team_id", NpgsqlTypes.NpgsqlDbType.Uuid, (object)player.TeamId ?? DBNull.Value);
                command.Parameters.AddWithValue("@player_name", player.PlayerName);
                command.Parameters.AddWithValue("@position", player.Position);
                command.Parameters.AddWithValue("@number", player.Number);
                command.Parameters.AddWithValue("@age", player.Age);
                command.Parameters.AddWithValue("@nationality", player.Nationality);

                conn.Open();
                var numberOfCommits = command.ExecuteNonQuery();
                if (numberOfCommits == 0)
                {
                    return NotFound();
                }
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
                using var conn = new NpgsqlConnection(connString);
                var commandText = "DELETE FROM \"Players\" WHERE player_id = @player_id;;";

                using var command = new NpgsqlCommand(commandText, conn);

                command.Parameters.AddWithValue("@player_id", NpgsqlTypes.NpgsqlDbType.Uuid, player.PlayerId);


                conn.Open();
                var numberOfCommits = command.ExecuteNonQuery();
                if (numberOfCommits == 0)
                {
                    return NotFound("Player not found.");
                }
                return Ok("Succesfully deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetAllPlayers")]
        public ActionResult Get()

        {
            var players = new List<Player>();
            try
            {

                using var conn = new NpgsqlConnection(connString);
                var commandText = "SELECT * FROM \"Players\";";

                using var command = new NpgsqlCommand(commandText, conn);

                conn.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var footballPlayer = new Player();
                        footballPlayer.PlayerId = Guid.Parse(reader[0].ToString());
                        footballPlayer.TeamId = Guid.TryParse(reader[1].ToString(), out var result) ? result : null;
                        footballPlayer.PlayerName = reader[2].ToString();
                        footballPlayer.Position = reader[3].ToString();
                        footballPlayer.Number = Convert.ToInt32(reader[4]);
                        footballPlayer.Age = Convert.ToInt32(reader[5]);
                        footballPlayer.Nationality = reader[6].ToString();

                        players.Add(footballPlayer);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok(players);

        }
        [HttpGet("GetPlayerById")]
        public ActionResult Get(Guid id)

        {
            var footballPlayer = new Player();

            try
            {


                using var conn = new NpgsqlConnection(connString);
                var commandText = "SELECT * FROM \"Players\" WHERE \"player_id\" = @id;";


                using var command = new NpgsqlCommand(commandText, conn);
                command.Parameters.AddWithValue("@id", id);

                conn.Open();
                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    reader.Read();
                    footballPlayer.PlayerId = Guid.Parse(reader[0].ToString());
                    footballPlayer.TeamId = Guid.TryParse(reader[1].ToString(), out var result) ? result : null;
                    footballPlayer.PlayerName = reader[2].ToString();
                    footballPlayer.Position = reader[3].ToString();
                    footballPlayer.Number = Convert.ToInt32(reader[4]);
                    footballPlayer.Age = Convert.ToInt32(reader[5]);
                    footballPlayer.Nationality = reader[6].ToString();


                }
                if (footballPlayer == null)
                {
                    return NotFound("player not found");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return Ok(footballPlayer);

        }
        [HttpPut("UpdatePlayer/{id}")]
        public ActionResult Put(Guid id, Player player)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                var commandText = "UPDATE \"Players\" SET  team_id = @team_id,player_name = @player_name,position = @position,number = @number,age = @age,nationality = @nationality WHERE player_id = @id";
                using var command = new NpgsqlCommand(commandText, conn);

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@player_id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                command.Parameters.AddWithValue("@team_id", NpgsqlTypes.NpgsqlDbType.Uuid, (object)player.TeamId ?? DBNull.Value);
                command.Parameters.AddWithValue("@player_name", player.PlayerName);
                command.Parameters.AddWithValue("@position", player.Position);
                command.Parameters.AddWithValue("@number", player.Number);
                command.Parameters.AddWithValue("@age", player.Age);
                command.Parameters.AddWithValue("@nationality", player.Nationality);


                conn.Open();
                var numberOfCommits = command.ExecuteNonQuery();

                if (numberOfCommits == 0)
                {
                    return NotFound();
                }
                return Ok("Succesfully updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

