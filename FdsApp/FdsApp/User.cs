using System;
using System.Collections.Generic;
using System.Text;
using Fds.Models;

namespace FdsApp{
    public class User : IUser {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
