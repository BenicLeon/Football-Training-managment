using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Football.Model
{
    public class Team
    {
        public Guid Id { get; set; }

       
        public string Name { get; set; }

       
        public string City { get; set; }

       
        public string Stadium { get; set; }

        public int FoundedYear { get; set; }

       
    }
}
