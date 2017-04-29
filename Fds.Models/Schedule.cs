using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fds.Models {
    public class Schedule <TUser> where TUser : IUser{
        public virtual int Id { get; set; }

        [DataType( DataType.DateTime )]
        public virtual DateTime DateTime { get; set; }

        public virtual int EventId { get; set; }
        public virtual Event<TUser> Event { get; set; }

        public virtual ICollection< UserJoinEvent<TUser> > UserJoined { get; set; }
    }
}