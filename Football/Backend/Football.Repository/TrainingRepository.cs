using Football.Model;
using Football.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Football.Repository
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly string _connectionString;

        public TrainingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TrainingSession>> GetAllTrainingSessionsAsync()
        {
            var sessions = new List<TrainingSession>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT * FROM trainingsessions", conn);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        sessions.Add(new TrainingSession
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            Description = reader.GetString(2),
                        });
                    }
                }
            }

            return sessions;
        }

        public async Task<TrainingSession> GetTrainingSessionByIdAsync(int id)
        {
            TrainingSession session = null;

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT * FROM trainingsessions WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        session = new TrainingSession
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            Description = reader.GetString(2)
                        };
                    }
                }
            }

            return session;
        }

        public async Task<bool> AddTrainingSessionAsync(TrainingSession session)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("INSERT INTO trainingsessions (date, description) VALUES (@date, @description)", conn);
                cmd.Parameters.AddWithValue("@date", session.Date);
                cmd.Parameters.AddWithValue("@description", session.Description);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateTrainingSessionAsync(TrainingSession session)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("UPDATE trainingsessions SET date = @date, description = @description WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@date", session.Date);
                cmd.Parameters.AddWithValue("@description", session.Description);
                cmd.Parameters.AddWithValue("@id", session.Id);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteTrainingSessionAsync(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("DELETE FROM trainingsessions WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Player>> GetPlayersByTrainingSessionIdAsync(int trainingSessionId)
        {
            var players = new List<Player>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(@"
                    SELECT p.id, p.name, p.contract, p.userid
                    FROM players p
                    JOIN trainingattendees ta ON p.id = ta.playerid
                    WHERE ta.trainingsessionid = @trainingsessionid", conn);
                cmd.Parameters.AddWithValue("@trainingsessionid", trainingSessionId);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        players.Add(new Player
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Contract = reader.GetString(2),
                            UserId = reader.GetGuid(3)
                        });
                    }
                }
            }

            return players;
        }



        public async Task<bool> AddPlayerToTrainingSessionAsync(Guid playerId, int trainingSessionId)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("INSERT INTO trainingattendees (playerid, trainingsessionid) VALUES (@playerid, @trainingsessionid)", conn);
                cmd.Parameters.AddWithValue("@playerid", playerId);
                cmd.Parameters.AddWithValue("@trainingsessionid", trainingSessionId);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> RemovePlayerFromTrainingSessionAsync(Guid playerId, int trainingSessionId)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("DELETE FROM trainingattendees WHERE playerid = @playerid AND trainingsessionid = @trainingsessionid", conn);
                cmd.Parameters.AddWithValue("@playerid", playerId);
                cmd.Parameters.AddWithValue("@trainingsessionid", trainingSessionId);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        // Dodajte ovu metodu da provjerite postoji li trening sesija
        public async Task<bool> TrainingSessionExistsAsync(int trainingSessionId)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM trainingsessions WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", trainingSessionId);

                var count = (long)await cmd.ExecuteScalarAsync();
                return count > 0;
            }
        }

        public async Task<IEnumerable<TrainingSession>> GetTrainingSessionsByPlayerIdAsync(Guid playerId)
        {
            var sessions = new List<TrainingSession>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(@"
                    SELECT ts.id, ts.date, ts.description
                    FROM trainingsessions ts
                    JOIN trainingattendees ta ON ts.id = ta.trainingsessionid
                    WHERE ta.playerid = @playerid", conn);
                cmd.Parameters.AddWithValue("@playerid", playerId);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        sessions.Add(new TrainingSession
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            Description = reader.GetString(2)
                        });
                    }
                }
            }

            return sessions;
        }
        public async Task AddPrivateTraining(Guid playerId, string youtubeLink)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("UPDATE team_players SET private_training_links = array_append(private_training_links, @youtubeLink) WHERE playerid = @playerId", conn);
                cmd.Parameters.AddWithValue("@youtubeLink", youtubeLink);
                cmd.Parameters.AddWithValue("@playerId", playerId);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<string>> GetPrivateTrainings(Guid playerId)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT private_training_links FROM team_players WHERE playerid = @playerId", conn);
                cmd.Parameters.AddWithValue("@playerId", playerId);

                var links = new List<string>();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var array = reader["private_training_links"] as string[];
                        if (array != null)
                        {
                            links.AddRange(array);
                        }
                    }
                }
                return links;
            }
        }


    }
}
