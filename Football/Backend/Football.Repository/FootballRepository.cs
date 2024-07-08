using Football.Model;
using Football.Repository.Common;
using Npgsql;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Football.Repository
{
    public class FootballRepository : IFootballRepository
    {
        private readonly string _connectionString;
        private readonly Guid TeamId = new Guid("1d267852-c8f5-493c-b064-8553517b0393"); // Hardkodirani ID tima

        public FootballRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            var players = new List<Player>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT * FROM players", conn);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        players.Add(new Player
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Contract = reader.GetString(reader.GetOrdinal("contract")),
                            UserId = reader.GetGuid(reader.GetOrdinal("userid"))
                        });
                    }
                }
            }

            return players;
        }
        

        public async Task<IEnumerable<Player>> GetTeamPlayersAsync()
        {
            var players = new List<Player>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(@"SELECT p.id, p.name, p.contract, p.userid 
                                              FROM players p
                                              INNER JOIN team_players tp ON p.id = tp.playerid
                                              WHERE tp.teamid = @TeamId", conn);
                cmd.Parameters.AddWithValue("@TeamId", TeamId);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        players.Add(new Player
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Contract = reader.GetString(reader.GetOrdinal("contract")),
                            UserId = reader.GetGuid(reader.GetOrdinal("userid"))
                        });
                    }
                }
            }

            return players;
        }

        public async Task<Player> GetPlayerByIdAsync(Guid id)
        {
            Player player = null;

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT * FROM players WHERE id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        player = new Player
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Contract = reader.GetString(reader.GetOrdinal("contract")),
                            UserId = reader.GetGuid(reader.GetOrdinal("userid"))
                        };
                    }
                }
            }

            return player;
        }

        public async Task<bool> AddPlayerAsync(Player player)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("INSERT INTO players (id, name, contract, userid) VALUES (@Id, @Name, @Contract, @UserId)", conn);
                cmd.Parameters.AddWithValue("@Id", player.Id);
                cmd.Parameters.AddWithValue("@Name", player.Name);
                cmd.Parameters.AddWithValue("@Contract", player.Contract);
                cmd.Parameters.AddWithValue("@UserId", player.UserId);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdatePlayerAsync(Player player)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("UPDATE players SET name = @Name, contract = @Contract WHERE id = @Id", conn);
                cmd.Parameters.AddWithValue("@Name", player.Name);
                cmd.Parameters.AddWithValue("@Contract", player.Contract);
                cmd.Parameters.AddWithValue("@Id", player.Id);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeletePlayerAsync(Guid id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("DELETE FROM players WHERE id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> AddPlayerToTeamAsync(Guid playerId)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Provjera da li igrač ima postavljen ugovor
                var checkCmd = new NpgsqlCommand("SELECT contract FROM players WHERE id = @PlayerId", conn);
                checkCmd.Parameters.AddWithValue("@PlayerId", playerId);
                var contract = (string)await checkCmd.ExecuteScalarAsync();

                if (string.IsNullOrEmpty(contract))
                {
                    throw new InvalidOperationException("Player must have a contract before being added to the team.");
                }

                var cmd = new NpgsqlCommand("INSERT INTO team_players (playerid, teamid) VALUES (@PlayerId, @TeamId)", conn);
                cmd.Parameters.AddWithValue("@PlayerId", playerId);
                cmd.Parameters.AddWithValue("@TeamId", TeamId);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
        public async Task<bool> RemovePlayerFromTeamAsync(Guid playerId)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("DELETE FROM team_players WHERE playerid = @PlayerId AND teamid = @TeamId", conn);
                cmd.Parameters.AddWithValue("@PlayerId", playerId);
                cmd.Parameters.AddWithValue("@TeamId", TeamId);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdatePlayerContractAsync(Guid playerId, string contract)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("UPDATE players SET contract = @Contract WHERE id = @PlayerId", conn);
                cmd.Parameters.AddWithValue("@Contract", contract);
                cmd.Parameters.AddWithValue("@PlayerId", playerId);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
        public async Task<bool> IsPlayerInTeamByIdAsync(Guid playerId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new NpgsqlCommand("SELECT COUNT(*) FROM team_players WHERE playerid = @PlayerId", connection);
                command.Parameters.AddWithValue("PlayerId", playerId);

                var count = (long)await command.ExecuteScalarAsync();
                return count > 0;
            }
        }

    }
}