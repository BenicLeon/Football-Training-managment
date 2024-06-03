using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Football.WebApi
{
    public class Player
    {
        public Guid PlayerId { get; set; }

        
        public Guid? TeamId { get; set; }

       
        public string PlayerName { get; set; }

        
        public string Position { get; set; }

        public int Number { get; set; }

        public int Age { get; set; }

        
        public string Nationality { get; set; }

        
    }
}
