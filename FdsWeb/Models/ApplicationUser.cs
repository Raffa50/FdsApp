using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FdsWeb.Models{
    public enum Role { User, Mod, Admin }

    public class ApplicationUser : IdentityUser{
        public virtual Role Role { get; set; }

        public virtual ICollection<UserJoinEvent> UserJoined { get; set; }
    }
}
