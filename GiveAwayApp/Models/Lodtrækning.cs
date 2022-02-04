using GiveAwayApp.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiveAwayApp.Models
{
    public class Lodtrækning
    {
        public int LodtrækningId { get; set; }
        public int ValgteSpilId { get; set; }
        public Spil ValgteSpil { get; set; }
        public GiveAwayAppUser Vinder { get; set; }
        public bool ValgtTilLodtrækning { get; set; }
        public bool ErTrukket { get; set; }
    }
}
