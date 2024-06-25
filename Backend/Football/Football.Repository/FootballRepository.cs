using Football.Model;
using Football.Repository.Common;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Football.Repository
{
    public class FootballRepository : IFootballRepository
    {
        string connString = "Host=localhost;Username=postgres;Password=fcfullam13;Database=footballPlayers";

        public async Task<string> PostPlayerAsync(Player player)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                var commandText = "INSERT INTO \"Players\" (\"Id\", \"Team_id\", \"Name\", \"Position\", \"Number\", \"Age\", \"Nationality\") VALUES (@id, @team_id, @name, @position, @number, @age, @nationality)";
                using var command = new NpgsqlCommand(commandText, conn);

                command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                command.Parameters.AddWithValue("@team_id", NpgsqlTypes.NpgsqlDbType.Uuid, (object)player.TeamId ?? DBNull.Value);
                command.Parameters.AddWithValue("@name", player.Name);
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
                var commandText = "DELETE FROM \"Players\" WHERE \"Id\" = @id;";

                using var command = new NpgsqlCommand(commandText, conn);

                command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Uuid, playerId);

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
                        Id = Guid.Parse(reader[0].ToString()),
                        TeamId = Guid.TryParse(reader[1].ToString(), out var result) ? result : null,
                        Name = reader[2].ToString(),
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

                var checkPlayerCommandText = "SELECT COUNT(1) FROM \"Players\" WHERE \"Id\" = @id";
                using var checkPlayerCommand = new NpgsqlCommand(checkPlayerCommandText, conn);
                checkPlayerCommand.Parameters.AddWithValue("@id", id);

                var playerExists = (long)await checkPlayerCommand.ExecuteScalarAsync() > 0;
                if (!playerExists)
                {
                    return null;
                }

                var commandText = "SELECT * FROM \"Players\" WHERE \"Id\" = @id;";
                using var command = new NpgsqlCommand(commandText, conn);
                command.Parameters.AddWithValue("@id", id);

                using var reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    footballPlayer.Id = Guid.Parse(reader[0].ToString());
                    footballPlayer.TeamId = Guid.TryParse(reader[1].ToString(), out var result) ? result : null;
                    footballPlayer.Name = reader[2].ToString();
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

                var checkPlayerCommandText = "SELECT COUNT(1) FROM \"Players\" WHERE \"Id\" = @id";
                using var checkPlayerCommand = new NpgsqlCommand(checkPlayerCommandText, conn);
                checkPlayerCommand.Parameters.AddWithValue("@id", id);

                var playerExists = (long)await checkPlayerCommand.ExecuteScalarAsync() > 0;
                if (!playerExists)
                {
                    return false;
                }

                var commandText = "UPDATE \"Players\" SET  \"Name\" = @player_name, \"Position\" = @position, \"Number\" = @number, \"Age\" = @age, \"Nationality\" = @nationality WHERE \"Id\" = @id";
                using var command = new NpgsqlCommand(commandText, conn);

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@player_name", player.Name);
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

        public async Task<ServiceResponse<List<Player>>> GetPlayersWithFilterPagingAndSortAsync(FilterForPlayer filter, Paging paging, SortOrder sort)
        {
            var response = new ServiceResponse<List<Player>>();

            StringBuilder query = ReturnConditionString(filter, paging, sort);

            var listFromDB = new List<Player>();

            var command = new NpgsqlCommand(query.ToString(), new NpgsqlConnection(connString));

            SetFilterParams(command, filter, paging, sort);

            command.Connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                listFromDB.Add(new Player
                {
                    Id = Guid.Parse(reader[0].ToString()),
                    TeamId = Guid.TryParse(reader[1].ToString(), out var result) ? result : null,
                    Name = reader[2].ToString(),
                    Position = reader[3].ToString(),
                    Number = Convert.ToInt32(reader[4]),
                    Age = Convert.ToInt32(reader[5]),
                    Nationality = reader[6].ToString()
                });
            }

            command.Connection.Close();
            await reader.DisposeAsync();

            if (listFromDB is not null)
            {
                response.Data = listFromDB;
                response.Success = true;
                return response;
            }
            else
            {
                response.Message = "No data in database";
                response.Success = false;
                return response;
            }
        }

        #region Extensions

        private void SetFilterParams(NpgsqlCommand command, FilterForPlayer filter, Paging paging, SortOrder sort)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                command.Parameters.AddWithValue("@Name", "%" + filter.Name + "%");
            }
            if (!string.IsNullOrWhiteSpace(filter.Position))
            {
                command.Parameters.AddWithValue("@Position", "%" + filter.Position + "%");
            }
            if (!string.IsNullOrWhiteSpace(filter.Nationality))
            {
                command.Parameters.AddWithValue("@Nationality", "%" + filter.Nationality + "%");
            }

            if (!string.IsNullOrWhiteSpace(sort.OrderBy))
            {
                command.Parameters.AddWithValue("@OrderBy", sort.OrderBy);
            }
            if (!string.IsNullOrWhiteSpace(sort.OrderDirection))
            {
                command.Parameters.AddWithValue("@OrderDirection", sort.OrderDirection);
            }

            command.Parameters.AddWithValue("@PageSize", paging.PageSize);
            command.Parameters.AddWithValue("@PageNumber", paging.PageNumber);
        }

        private StringBuilder ReturnConditionString(FilterForPlayer filter, Paging paging, SortOrder sort)
        {
            StringBuilder query = new StringBuilder("SELECT * FROM \"Players\" WHERE \"IsActive\" = true");

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query.Append(" AND \"Name\" LIKE @Name");
            }

            if (!string.IsNullOrWhiteSpace(filter.Position))
            {
                query.Append(" AND \"Position\" LIKE @Position");
            }

            if (!string.IsNullOrWhiteSpace(filter.Nationality))
            {
                query.Append(" AND \"Nationality\" LIKE @Nationality");
            }

            if (!string.IsNullOrEmpty(sort.OrderDirection) && !string.IsNullOrEmpty(sort.OrderBy))
            {
                query.Append($" ORDER BY \"{sort.OrderBy}\"  {sort.OrderDirection} ");
            }

            if (int.IsPositive(paging.PageSize) && paging.PageNumber > 0)
            {
                int page = (paging.PageNumber - 1) * paging.PageSize;
                query.Append(" LIMIT @PageSize OFFSET " + page);
            }

            return query;
        }
        #endregion
    }
}
