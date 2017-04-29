using System.ComponentModel.DataAnnotations;

namespace Fds.Models {
    public class UserJoinEvent<TUser> where TUser : IUser {
        public virtual int Id { get; set; }

        public virtual int EventId { get; set; }
        public virtual Event<TUser> Event { get; set; }

        [Required]
        public virtual string ApplicationUserId { get; set; }

        public virtual TUser ApplicationUser { get; set; }

        public virtual int ScheduleId { get; set; }
        public virtual Schedule<TUser> Schedule { get; set; }

        [Range( 0, DomainConstraints.VoteMax )]
        public virtual int? Vote { get; set; }

        [MaxLength( DomainConstraints.ReviewMaxLen )]
        public virtual string Review { get; set; }
    }
}