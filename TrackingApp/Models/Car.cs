using System;
using System.Collections.Generic;

namespace TrackingApp.Models
{
    public partial class Car
    {
        public Car()
        {
            Location = new HashSet<Location>();
        }

        public long Id { get; set; }
        public string CarId { get; set; }
        public DateTime? DateTimeAdded { get; set; }
        public string Name { get; set; }

        public ICollection<Location> Location { get; set; }
    }
}
