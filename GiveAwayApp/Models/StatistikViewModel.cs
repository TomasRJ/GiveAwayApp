using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveAwayApp.Models
{
    public class StatistikViewModel
    {
        public List<Spil> PopulæreSpilList { get; set; }
        public List<Spil> PopulæreGenreList { get; set; }
        public int AntalBesøg { get; set; }
        public int AntalBrugere { get; set; }
        public List<Statistik> SyvDageStatistik { get; set; }
    }
}
