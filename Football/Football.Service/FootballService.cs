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
           
            return await _playerRepository.DeletePlayerAsync(id);
        }
        public async Task<List<Player>> GetPlayerAsync() {
            List<Player> players = await _playerRepository.GetPlayerAsync(); 
            return players;
               
        }
        public async Task<Player> GetPlayerByIdAsync(Guid id)
        {
            Player player = await _playerRepository.GetPlayerByIdAsync(id);
            if (player == null)
            {
                return null;
            }
            return player;
        }
        public async Task<bool> UpdatePlayersAsync(Guid id, Player player)
        {
            var playerExists = await _playerRepository.GetPlayerByIdAsync(id);
            if (playerExists != null)
            {
                return await _playerRepository.UpdatePlayersAsync(id, player);
            }
            return false;
        }
       

    }
}
