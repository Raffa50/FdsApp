using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fds.Models {
    public class EventType{
        public virtual int Id { get; set; }

        [Required, MinLength( DomainConstraints.EventTypeMinLen )]
        public virtual string Type { get; set; }

        public virtual ICollection< Event > Events { get; set; }
    }
}