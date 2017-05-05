using System.ComponentModel.DataAnnotations;

namespace Fds.Models {
    public class UserJoinEvent{
        public virtual int Id { get; set; }

        public virtual int EventId { get; set; }
        public virtual Event Event { get; set; }

        [Required]
        public virtual string ApplicationUserId { get; set; }
        public virtual IUser ApplicationUser { get; set; }

        public virtual int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }

        [Range( 0, DomainConstraints.VoteMax )]
        public virtual int? Vote { get; set; }

        [MaxLength( DomainConstraints.ReviewMaxLen )]
        public virtual string Review { get; set; }
    }
}