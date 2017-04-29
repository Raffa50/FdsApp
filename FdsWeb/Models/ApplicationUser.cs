using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FdsWeb.Models {
    public enum Role {
        User,
        Mod,
        Admin
    }

    public class ApplicationUser : IdentityUser {
        public virtual Role Role { get; set; }

        public virtual ICollection< UserJoinEvent > UserJoined { get; set; }

        #region fields excluded from JSON serialization
        [IgnoreDataMember]
        public override string PasswordHash { get; set; }
        [IgnoreDataMember]
        public override string SecurityStamp { get; set; }
        [IgnoreDataMember]
        public override string ConcurrencyStamp { get; set; }
        #endregion
    }
}