using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FdsApp{
    public class ApiHelper {
        private const string baseUrl = "http://localhost";
        readonly HttpClient _client= new HttpClient();

        public async Task< IEnumerable< Events > > GetEvents() {
            
        }
    }
}
