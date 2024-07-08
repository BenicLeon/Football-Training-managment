namespace Football.WebApi
{
    public class AddPlayerToTeamModel
    {
        public Guid PlayerId { get; set; }
        public Guid TeamId { get; set; }
    }

}
