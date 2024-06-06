using Football.Model;
using Football.Repository.Common;
using Football.Service.Common;


namespace Football.Service

{
    public class FootballService: IFootballService
    {
        private IFootballRepository _playerRepository;

        public FootballService(IFootballRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        public Task<string> PostPlayerAsync(Player player)
        {
           
            return _playerRepository.PostPlayerAsync(player);
        }
        public Task<string> DeletePlayerAsync(Player player) { 
            return _playerRepository.DeletePlayerAsync(player); 
        }
        public Task<List<Player>> GetPlayerAsync() {
            return _playerRepository.GetPlayerAsync();
        }
        public Task<Player> GetPlayerByIdAsync(Guid id)
        {
            return _playerRepository.GetPlayerByIdAsync(id);
        }
        public Task<string> UpdatePlayersAsync(Guid id, Player player)
        {
            return _playerRepository.UpdatePlayersAsync(id, player);
        }

    }
}
