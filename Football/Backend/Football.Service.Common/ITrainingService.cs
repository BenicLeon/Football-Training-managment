using Football.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Football.Service.Common
{
    public interface ITrainingService
    {
        Task<IEnumerable<TrainingSession>> GetAllTrainingSessionsAsync();
        Task<TrainingSession> GetTrainingSessionByIdAsync(int id);
        Task<bool> AddTrainingSessionAsync(TrainingSession session);
        Task<bool> UpdateTrainingSessionAsync(TrainingSession session);
        Task<bool> DeleteTrainingSessionAsync(int id);
        Task<IEnumerable<Player>> GetPlayersByTrainingSessionIdAsync(int trainingSessionId);
        Task<bool> AddPlayerToTrainingSessionAsync(Guid playerId, int trainingSessionId);
        Task<bool> RemovePlayerFromTrainingSessionAsync(Guid playerId, int trainingSessionId);
        Task<IEnumerable<TrainingSession>> GetTrainingSessionsByPlayerIdAsync(Guid playerId);

        Task<bool> TrainingSessionExistsAsync(int trainingSessionId);

        Task AddPrivateTraining(Guid playerId, string youtubeLink);

        Task<IEnumerable<string>> GetPrivateTrainings(Guid playerId);
    }
}
