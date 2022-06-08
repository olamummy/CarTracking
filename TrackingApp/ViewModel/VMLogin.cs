using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingApp.ViewModel
{
    public class VMLogin
    {
        [Required]
        public string UserName { get; set; }
    }
}
