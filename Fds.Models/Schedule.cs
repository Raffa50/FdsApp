using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fds.Models {
    public class Schedule{
        public virtual int Id { get; set; }

        [DataType( DataType.DateTime )]
        public virtual DateTime DateTime { get; set; }

        public virtual int EventId { get; set; }
        public virtual Event Event { get; set; }

        public virtual ICollection< UserJoinEvent > UserJoined { get; set; }
    }
}