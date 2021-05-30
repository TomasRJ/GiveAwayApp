using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiveAwayApp.Areas.Identity.Data;

namespace GiveAwayApp.Models
{
    public class LodtrækningViewModel
    {
        public List<Spil> ValgteSpilList { get; set; }
        public List<Spil> TrukketSpilList { get; set; }
        public List<Spil> SpilTilLodtrækning { get; set; }
        public GiveAwayAppUser BrugerInfo { get; set; }
    }
}
