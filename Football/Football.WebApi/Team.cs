using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Football.WebApi
{
    public class Team
    {
        public Guid TeamId { get; set; }

       
        public string TeamName { get; set; }

       
        public string City { get; set; }

       
        public string Stadium { get; set; }

        public int FoundedYear { get; set; }

       
    }
}
