using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FdsWeb.Models;

namespace FdsWeb.ViewModels {
    public class Search {
        public DateTime? Date { get; set; }

        public DateTime? DateBegins { get; set; }
        public DateTime? DateFinish { get; set; }

        public int? EventTypeId { get; set; }

        public byte? Age { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public double? Radius { get; set; }
    }
}