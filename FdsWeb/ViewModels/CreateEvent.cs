using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fds.Models;
using FdsWeb.Attributes;
using FdsWeb.Data;
using FdsWeb.Models;

namespace FdsWeb.ViewModels {
    public class CreateEvent {
        public int? Id { get; set; }

        [Required, MinLength( DomainConstraints.EventNameMinLen )]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Latitude { get; set; }

        [Required]
        public string Longitude { get; set; }

        [EnsureMinimumElements( 1, ErrorMessage = "Almeno una data richiesta!" )]
        public List< DateTime > Schedule { get; set; }

        public int EventTypeId { get; set; }
    }
}