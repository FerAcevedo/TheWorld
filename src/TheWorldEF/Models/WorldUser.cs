using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TheWorldEF.Models
{
    public class WorldUser: IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}