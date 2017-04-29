using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fds.Models {
    public class Event <TUser> where TUser : IUser{
        public virtual int Id { get; set; }

        [Required, MinLength( DomainConstraints.EventNameMinLen )]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Description { get; set; }

        [Required]
        public virtual string ApplicationUserId { get; set; }

        public virtual TUser ApplicationUser { get; set; }

        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }

        public virtual ICollection< Schedule<TUser> > Schedule { get; set; }

        public virtual int EventTypeId { get; set; }
        public virtual EventType<TUser> EventType { get; set; }

        public virtual ICollection< UserJoinEvent<TUser> > UserJoined { get; set; }
    }
}