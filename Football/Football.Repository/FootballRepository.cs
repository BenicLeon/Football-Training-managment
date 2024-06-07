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


        public async Task<string> PostPlayerAsync(Player player)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

               
                var commandText = "INSERT INTO \"Players\" (player_id, team_id, player_name, position, number, age, nationality) VALUES (@player_id, @team_id, @player_name, @position, @number, @age, @nationality)";
                using var command = new NpgsqlCommand(commandText, conn);

                
                command.Parameters.AddWithValue("@player_id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                command.Parameters.AddWithValue("@team_id", NpgsqlTypes.NpgsqlDbType.Uuid, (object)player.TeamId ?? DBNull.Value);
                command.Parameters.AddWithValue("@player_name", player.PlayerName);
                command.Parameters.AddWithValue("@position", player.Position);
                command.Parameters.AddWithValue("@number", player.Number);
                command.Parameters.AddWithValue("@age", player.Age);
                command.Parameters.AddWithValue("@nationality", player.Nationality);

                var numberOfCommits = await command.ExecuteNonQueryAsync();
                if (numberOfCommits == 0)
                {
                    return "Insertion failed";
                }

                return "Successfully added";
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Exception: {ex.Message}");
                return ex.Message;
            }
        }

        public async Task<string> DeletePlayerAsync(Guid playerId)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                var commandText = "DELETE FROM \"Players\" WHERE player_id = @player_id;";

                using var command = new NpgsqlCommand(commandText, conn);

                command.Parameters.AddWithValue("@player_id", NpgsqlTypes.NpgsqlDbType.Uuid, playerId);

                conn.Open();
                var numberOfCommits = await command.ExecuteNonQueryAsync();
                if (numberOfCommits == 0)
                {
                    return "Player not found.";
                }
                return "Successfully deleted";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<Player>> GetPlayerAsync()
        {
            var players = new List<Player>();
            try
            {
                using var conn = new NpgsqlConnection(connString);
                var commandText = "SELECT * FROM \"Players\";";

                using var command = new NpgsqlCommand(commandText, conn);

                conn.Open();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var footballPlayer = new Player
                    {
                        PlayerId = Guid.Parse(reader[0].ToString()),
                        TeamId = Guid.TryParse(reader[1].ToString(), out var result) ? result : null,
                        PlayerName = reader[2].ToString(),
                        Position = reader[3].ToString(),
                        Number = Convert.ToInt32(reader[4]),
                        Age = Convert.ToInt32(reader[5]),
                        Nationality = reader[6].ToString()
                    };

                    players.Add(footballPlayer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return players;
        }

        public async Task<Player> GetPlayerByIdAsync(Guid id)
        {
            var footballPlayer = new Player();

            try
            {
                using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                
                var checkPlayerCommandText = "SELECT COUNT(1) FROM \"Players\" WHERE \"player_id\" = @id";
                using var checkPlayerCommand = new NpgsqlCommand(checkPlayerCommandText, conn);
                checkPlayerCommand.Parameters.AddWithValue("@id", id);

                var playerExists = (long)await checkPlayerCommand.ExecuteScalarAsync() > 0;
                if (!playerExists)
                {
                    return null;
                }

                
                var commandText = "SELECT * FROM \"Players\" WHERE \"player_id\" = @id;";
                using var command = new NpgsqlCommand(commandText, conn);
                command.Parameters.AddWithValue("@id", id);

                using var reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {

                    await reader.ReadAsync();
                    footballPlayer.PlayerId = Guid.Parse(reader[0].ToString());
                    footballPlayer.TeamId = Guid.TryParse(reader[1].ToString(), out var result) ? result : null;
                    footballPlayer.PlayerName = reader[2].ToString();
                    footballPlayer.Position = reader[3].ToString();
                    footballPlayer.Number = Convert.ToInt32(reader[4]);
                    footballPlayer.Age = Convert.ToInt32(reader[5]);
                    footballPlayer.Nationality = reader[6].ToString();
                }
                else
                {
                    return null; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }

            return footballPlayer;
        }






        public async Task<bool> UpdatePlayersAsync(Guid id, Player player)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                
                var checkPlayerCommandText = "SELECT COUNT(1) FROM \"Players\" WHERE \"player_id\" = @id";
                using var checkPlayerCommand = new NpgsqlCommand(checkPlayerCommandText, conn);
                checkPlayerCommand.Parameters.AddWithValue("@id", id);

                var playerExists = (long)await checkPlayerCommand.ExecuteScalarAsync() > 0;
                if (!playerExists)
                {
                    return false; 
                }

                
                var commandText = "UPDATE \"Players\" SET player_name = @player_name, position = @position, number = @number, age = @age, nationality = @nationality WHERE player_id = @id";
                using var command = new NpgsqlCommand(commandText, conn);

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@player_name", player.PlayerName);
                command.Parameters.AddWithValue("@position", player.Position);
                command.Parameters.AddWithValue("@number", player.Number);
                command.Parameters.AddWithValue("@age", player.Age);
                command.Parameters.AddWithValue("@nationality", player.Nationality);

                var numberOfCommits = await command.ExecuteNonQueryAsync();

                return numberOfCommits > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}


    

