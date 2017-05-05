using System.Collections.Generic;
using Fds.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FdsWeb.Models {
    public class ApplicationUser : IdentityUser, IUser {
        public virtual Role Role { get; set; }

        public virtual ICollection< UserJoinEvent > UserJoined { get; set; }
        public virtual ICollection< Event > CreatedEvents { get; set; }

        /*#region fields excluded from JSON serialization
        [IgnoreDataMember]
        public override string PasswordHash { get; set; }
        [IgnoreDataMember]
        public override string SecurityStamp { get; set; }
        [IgnoreDataMember]
        public override string ConcurrencyStamp { get; set; }
        #endregion*/
    }
}