using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveAwayApp.Models
{
    public class Statistik
    {
        public int StatistikId { get; set; }
        public int AntalBesøgere { get; set; }
        public DateTime AntalBesøgereForDato { get; set; }
    }
}
