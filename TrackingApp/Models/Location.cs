using System;
using System.Collections.Generic;

namespace TrackingApp.Models
{
    public partial class Location
    {
        public long Id { get; set; }
        public long CarId { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }
        public DateTime DateTimeAdded { get; set; }

        public Car Car { get; set; }
    }
}
