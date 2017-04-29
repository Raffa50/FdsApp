using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fds.Models;
using Newtonsoft.Json;

namespace FdsApp{
    public class ApiHelper {
        private const string apiUrl = "http://fdsge.azurewebsites.net/api";
        readonly HttpClient _client= new HttpClient();

        public async Task<ICollection<Event<User>>> GetEvents() {
            var response = await _client.GetAsync( apiUrl + "/Events" );

            if ( !response.IsSuccessStatusCode ) throw new Exception();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Event<User>>>(content);
        }
    }
}
