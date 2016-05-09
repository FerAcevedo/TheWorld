using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TheWorldEF.Controllers.Api;

namespace TheWorldEF.ViewModels
{
    public class TripViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public IEnumerable<StopViewModel> Stops { get; set; }
    }
}