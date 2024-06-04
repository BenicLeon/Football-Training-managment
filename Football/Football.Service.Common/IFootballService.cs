using Football.Model;




namespace Football.Service.Common

{
    public interface IFootballService
    {
        public string PostPlayer(Player player);

        public string DeletePlayer(Player player);

        public List<Player> GetPlayer();

        public Player GetPlayerById(Guid id);

        public string UpdatePlayers(Guid id, Player player);
    }
}
