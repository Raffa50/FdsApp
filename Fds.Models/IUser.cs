using System;
using System.Collections.Generic;
using System.Text;

namespace Fds.Models{
    public interface IUser{
        string Id { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
    }
}
