using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football.Model
{
    public class TrainingSession
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public ICollection<Guid> Attendees { get; set; } = new List<Guid>(); // Lista prisutnih igrača
    }

}
