﻿namespace Fds.Models{
    public class User : IUser {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public User( string id, string username, string email ) {
            Id = id;
            UserName = username;
            Email = email;
        }
    }
}
