using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using GiveAwayApp.Models;

namespace GiveAwayApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the GiveAwayAppUser class
    public class GiveAwayAppUser : IdentityUser
    {
        public ICollection<Spil> Spil { get; set; }
    }
}
