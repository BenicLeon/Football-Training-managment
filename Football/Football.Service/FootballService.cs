using Football.Model;
using Football.Repository;
using Football.Service.Common;


namespace Football.Service

{
    public class FootballService: IFootballService
    {
        FootballRepository playerRepository = new FootballRepository();
        public string PostPlayer(Player player)
        {
           
            return playerRepository.PostPlayer(player);
        }
        public string DeletePlayer(Player player) { 
            return playerRepository.DeletePlayer(player); 
        }
        public List<Player> GetPlayer() {
            return playerRepository.GetPlayer();
        }
        public Player GetPlayerById(Guid id)
        {
            return playerRepository.GetPlayerById(id);
        }
        public string UpdatePlayers(Guid id, Player player)
        {
            return playerRepository.UpdatePlayers(id, player);
        }

    }
}
