using Football.Model;




namespace Football.Service.Common

{
    public interface IFootballService
    {
         Task<string> PostPlayerAsync(Player player);

         Task<string> DeletePlayerAsync(Player player);

         Task<List<Player>> GetPlayerAsync();

         Task<Player> GetPlayerByIdAsync(Guid id);

         Task<string> UpdatePlayersAsync(Guid id, Player player);
    }
}
