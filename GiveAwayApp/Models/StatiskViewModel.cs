using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveAwayApp.Models
{
    public class StatiskViewModel
    {
        public List<Spil> PopulæreSpilList { get; set; }
        public List<string> PopulæreGenreList { get; set; }
        public int AntalBesøg { get; set; }
        public int AntalBrugere { get; set; }
        public string[] LandeKodeList { get; set; }
    }
}
