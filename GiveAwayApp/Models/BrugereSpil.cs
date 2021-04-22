using System;
using GiveAwayApp.Areas.Identity.Data;

namespace GiveAwayApp.Models
{
    public class BrugereSpil
    {
        public string BrugerId { get; set; }
        public GiveAwayAppUser Bruger { get; set; }
        public int SpilId { get; set; }
        public Spil Spil { get; set; }
        public DateTime OprettelsesDato { get; set; }
    }
}
