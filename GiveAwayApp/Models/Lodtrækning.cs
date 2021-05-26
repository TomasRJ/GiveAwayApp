using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveAwayApp.Models
{
    public class Lodtrækning
    {
        public int LodtrækningId { get; set; }
        public int ValgteSpilId { get; set; }
        public string VinderBrugerId { get; set; }
        public bool ValgtTilLodtrækning { get; set; }
        public bool ErTrukket { get; set; }
    }
}
