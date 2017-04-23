using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FdsWeb.Data;

namespace FdsWeb.ViewModels{
    public class CreateEvent{
        [Required, MinLength(DomainConstraints.EventNameMinLen)]
        public string Name { get; set; }

        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }

        public List< DateTime > Schedule { get; set; }
    }
}
