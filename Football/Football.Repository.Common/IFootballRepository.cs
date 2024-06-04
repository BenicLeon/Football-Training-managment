using System.Numerics;
using Football.Model;

namespace Football.Repository.Common
{
    public interface IFootballRepository
    {
        string PostPlayer(Player player);
        string DeletePlayer(Player player);
        public List<Player> GetPlayer();

        public Player GetPlayerById(Guid id);

        public string UpdatePlayers(Guid id, Player player);
    }
}
