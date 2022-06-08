using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingApp.ViewModel
{
    public class LocationVM
    {  
        [Required]
        public string CarId { get; set; }

        [Required]
        public string Longtitude { get; set; }

        [Required]
        public string Latitude { get; set; }
    }
}
