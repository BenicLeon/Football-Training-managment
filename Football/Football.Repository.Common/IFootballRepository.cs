using System.Numerics;
using Football.Model;

namespace Football.Repository.Common
{
    public interface IFootballRepository
    {
        Task<string> PostPlayerAsync(Player player);
        Task<string> DeletePlayerAsync(Guid id);
        Task<List<Player>> GetPlayerAsync();

        Task<Player> GetPlayerByIdAsync(Guid id);

        Task<bool> UpdatePlayersAsync(Guid id, Player player);
    }
}
