using Football.Model;
using Football.Repository.Common;
using Football.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Football.Service
{
    public class FootballService : IFootballService
    {
        private readonly IFootballRepository _footballRepository;

        public FootballService(IFootballRepository footballRepository)
        {
            _footballRepository = footballRepository;
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            return await _footballRepository.GetAllPlayersAsync();
        }
       

        public async Task<IEnumerable<Player>> GetTeamPlayersAsync()
        {
            return await _footballRepository.GetTeamPlayersAsync();
        }

        public async Task<Player> GetPlayerByIdAsync(Guid id)
        {
            return await _footballRepository.GetPlayerByIdAsync(id);
        }

        public async Task<bool> AddPlayerAsync(Player player)
        {
            return await _footballRepository.AddPlayerAsync(player);
        }

        public async Task<bool> UpdatePlayerAsync(Player player)
        {
            return await _footballRepository.UpdatePlayerAsync(player);
        }

        public async Task<bool> DeletePlayerAsync(Guid id)
        {
            return await _footballRepository.DeletePlayerAsync(id);
        }

        public async Task<bool> AddPlayerToTeamAsync(Guid playerId)
        {
            return await _footballRepository.AddPlayerToTeamAsync(playerId);
        }

        public async Task<bool> RemovePlayerFromTeamAsync(Guid playerId)
        {
            return await _footballRepository.RemovePlayerFromTeamAsync(playerId);
        }

        public async Task<bool> UpdatePlayerContractAsync(Guid playerId, string contract)
        {
            return await _footballRepository.UpdatePlayerContractAsync(playerId, contract);
        }
        public async Task<bool> IsPlayerInTeamByIdAsync(Guid playerId)
        {
            return await _footballRepository.IsPlayerInTeamByIdAsync(playerId);
        }

    }
}
