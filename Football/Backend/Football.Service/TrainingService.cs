using Football.Model;
using Football.Repository;
using Football.Repository.Common;
using Football.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Football.Service
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _trainingRepository;

        public TrainingService(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }

        public async Task<IEnumerable<TrainingSession>> GetAllTrainingSessionsAsync()
        {
            return await _trainingRepository.GetAllTrainingSessionsAsync();
        }

        public async Task<TrainingSession> GetTrainingSessionByIdAsync(int id)
        {
            return await _trainingRepository.GetTrainingSessionByIdAsync(id);
        }

        public async Task<bool> AddTrainingSessionAsync(TrainingSession session)
        {
            return await _trainingRepository.AddTrainingSessionAsync(session);
        }

        public async Task<bool> UpdateTrainingSessionAsync(TrainingSession session)
        {
            return await _trainingRepository.UpdateTrainingSessionAsync(session);
        }

        public async Task<bool> DeleteTrainingSessionAsync(int id)
        {
            return await _trainingRepository.DeleteTrainingSessionAsync(id);
        }

        public async Task<IEnumerable<Player>> GetPlayersByTrainingSessionIdAsync(int trainingSessionId)
        {
            return await _trainingRepository.GetPlayersByTrainingSessionIdAsync(trainingSessionId);
        }
        public async Task<bool> TrainingSessionExistsAsync(int trainingSessionId)
        {
            return await _trainingRepository.TrainingSessionExistsAsync(trainingSessionId);
        }

        public async Task<bool> AddPlayerToTrainingSessionAsync(Guid playerId, int trainingSessionId)
        {
            // Provjerite postoji li trening
            var sessionExists = await _trainingRepository.TrainingSessionExistsAsync(trainingSessionId);
            if (!sessionExists)
            {
                return false;
            }

            // Dodajte igrača u tablicu trainingattendees
            return await _trainingRepository.AddPlayerToTrainingSessionAsync(playerId, trainingSessionId);
        }

        public async Task<bool> RemovePlayerFromTrainingSessionAsync(Guid playerId, int trainingSessionId)
        {
            // Provjerite postoji li trening
            var sessionExists = await _trainingRepository.TrainingSessionExistsAsync(trainingSessionId);
            if (!sessionExists)
            {
                return false;
            }

            // Uklonite igrača iz tablice trainingattendees
            return await _trainingRepository.RemovePlayerFromTrainingSessionAsync(playerId, trainingSessionId);
        }
        public async Task<IEnumerable<TrainingSession>> GetTrainingSessionsByPlayerIdAsync(Guid playerId)
        {
            return await _trainingRepository.GetTrainingSessionsByPlayerIdAsync(playerId);
        }

        public async Task AddPrivateTraining(Guid playerId, string youtubeLink)
        {
            try
            {
                await _trainingRepository.AddPrivateTraining(playerId, youtubeLink);
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while adding private training", ex);
            }
        }


        public async Task<IEnumerable<string>> GetPrivateTrainings(Guid playerId)
        {
            return await _trainingRepository.GetPrivateTrainings(playerId);
        }


    }
}
