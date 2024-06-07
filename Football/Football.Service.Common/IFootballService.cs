using Football.Model;




namespace Football.Service.Common

{
    public interface IFootballService
    {
         Task<string> PostPlayerAsync(Player player);

         Task<string> DeletePlayerAsync(Guid id);

         Task<List<Player>> GetPlayerAsync();

         Task<Player> GetPlayerByIdAsync(Guid id);

         Task<bool> UpdatePlayersAsync(Guid id, Player player);
    }
}
