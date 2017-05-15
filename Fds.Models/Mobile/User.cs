using Newtonsoft.Json;

namespace Fds.Models.Mobile{
    public class User : IUser {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        [JsonConstructor]
        public User( string id, string username, string email ) {
            Id = id;
            UserName = username;
            Email = email;
        }

        public User(IUser value): this(value.Id, value.UserName, value.Email) {
        }
    }
}
