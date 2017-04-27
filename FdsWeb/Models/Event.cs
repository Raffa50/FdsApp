using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FdsWeb.Data;

namespace FdsWeb.Models{
    public class Event{
        public virtual int Id { get; set; }
        [Required, MinLength(DomainConstraints.EventNameMinLen)]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Description { get; set; }

        [Required]
        public virtual string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }

        public virtual ICollection< Schedule > Schedule { get; set; }

        public virtual int EventTypeId { get; set; }
        public virtual EventType EventType { get; set; }

        public virtual ICollection< UserJoinEvent > UserJoined { get; set; }
    }
}
