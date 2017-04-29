using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FdsWeb.Data;

namespace FdsWeb.ViewModels {
    public class UserReview {
        public int EventId { get; set; }

        [Range( 0, DomainConstraints.VoteMax )]
        public int Vote { get; set; }

        [MaxLength( DomainConstraints.ReviewMaxLen )]
        public string Review { get; set; }

        public int SheduleId { get; set; }
    }
}