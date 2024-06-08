using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Model
{
    public class PlayerDto
    {
        public Guid Id { get; set; }
        public Guid? TeamId { get; set; }
        public string PlayerName { get; set; }
        public string Position { get; set; }
        public int Number { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
    }
}
