using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FdsWeb.Data;

namespace FdsWeb.Models{
    public class UserJoinEvent{
        public virtual int Id { get; set; }

        public virtual int EventId { get; set; }
        public virtual Event Event { get; set; }

        public virtual string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }

        [Range( 0, DomainConstraints.VoteMax )]
        public virtual int? Vote { get; set; }
        [MaxLength(DomainConstraints.ReviewMaxLen)]
        public virtual string Review { get; set; }
    }
}
