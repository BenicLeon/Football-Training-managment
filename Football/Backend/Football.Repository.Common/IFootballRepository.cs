using System.Numerics;
using Football.Model;

namespace Football.Repository.Common
{
    public interface IFootballRepository
    {
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task<IEnumerable<Player>> GetTeamPlayersAsync();
        Task<Player> GetPlayerByIdAsync(Guid id);
        Task<bool> AddPlayerAsync(Player player);
        Task<bool> UpdatePlayerAsync(Player player);
        Task<bool> DeletePlayerAsync(Guid id);
        Task<bool> AddPlayerToTeamAsync(Guid playerId);
        Task<bool> RemovePlayerFromTeamAsync(Guid playerId);
        Task<bool> UpdatePlayerContractAsync(Guid playerId, string contract);

        Task<bool> IsPlayerInTeamByIdAsync(Guid playerId);
       


    }
}
