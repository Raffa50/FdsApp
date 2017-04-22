using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FdsWeb.Models{
    public class Event{
        public virtual int Id { get; set; }
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual int ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }

        public virtual ICollection< Schedule > Schedule { get; set; }
    }
}
