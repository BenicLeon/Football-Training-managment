using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Model
{
    public class UpdatePlayerDto
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public int Number { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
    }
}
