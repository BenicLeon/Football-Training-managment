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
        public async Task<string> PostPlayerAsync(Player player)
        {
            string result = await _playerRepository.PostPlayerAsync(player);

            return result;
        }
        public async Task<string> DeletePlayerAsync(Guid id) { 
            string result = await _playerRepository.DeletePlayerAsync(id);
            return result;
        }
        public async Task<List<Player>> GetPlayerAsync() {
            List<Player> players = await _playerRepository.GetPlayerAsync(); 
            return players;
               
        }
        public async Task<Player> GetPlayerByIdAsync(Guid id)
        {
            Player player = await _playerRepository.GetPlayerByIdAsync(id);
           return player;
        }
        public async Task<bool> UpdatePlayersAsync(Guid id, Player player)

        {
            var playerAvailable = await _playerRepository.GetPlayerByIdAsync(id);
            if (playerAvailable != null && playerAvailable.PlayerId == player.PlayerId)
            {
                var updateResult = await _playerRepository.UpdatePlayersAsync(id, player);

                
                return updateResult;
            }
            else
            {
                return false;
            }
        }

    }
}
