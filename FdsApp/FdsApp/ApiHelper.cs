using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fds.Models;
using Newtonsoft.Json;

namespace FdsApp{
    public static class ApiHelper {
        const string apiUrl = "http://fdsge.azurewebsites.net/api";
        static readonly HttpClient _client= new HttpClient();
        public static User currentUser { get; private set; }

        public static async Task<IEnumerable<Event>> GetEvents() {
            var response = await _client.GetAsync( apiUrl + "/Events" );

            if ( !response.IsSuccessStatusCode ) throw new Exception();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Event>>(content);
        }

        public static async Task< IEnumerable< UserJoinEvent > > GetEventReviews( int id ) {
            var response = _client.GetAsync( apiUrl + "/Events/" + id ).Result;
            if (!response.IsSuccessStatusCode) throw new Exception();
            var content = await response.Content.ReadAsStringAsync();
            var ev= JsonConvert.DeserializeObject< Event >( content );
            return ev.UserJoined;
        }

        public static async Task<bool> Login( string email, string password ) {
            var json = JsonConvert.SerializeObject( new UserLogin(){Email = email, Password = password} );
            var response = _client.PostAsync(apiUrl + "Login", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            if( !response.IsSuccessStatusCode )
                return false;

            var content = await response.Content.ReadAsStringAsync();
            try {
                currentUser = JsonConvert.DeserializeObject<User>( content );
            } catch( Exception e ) {
                return false;
            }

            return true;
        }
    }
}
