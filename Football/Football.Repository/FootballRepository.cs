using Football.Model;
using Football.Repository.Common;

using Microsoft.Extensions.Logging;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;

namespace Football.Repository
{
    public class FootballRepository: IFootballRepository

    {
        string connString = "Host=localhost;Username=postgres;Password=fcfullam13;Database=footballPlayers";


        public string PostPlayer(Player player)
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
                    return "Not found";
                }
                return "Succesfully added";
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }

        }
        
        public string  DeletePlayer(Player player)
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
                    return "Player not found.";
                }
                return "Succesfully deleted";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
       
        public List<Player> GetPlayer()

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
            return players;

        }
        
        public Player GetPlayerById(Guid id)

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
                    Console.WriteLine("Player not found");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return footballPlayer;

        }


        
        public string UpdatePlayers(Guid id, Player player)
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
                    return "Player not found";
                      
                }
                return "Succesfully updated";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}


    

