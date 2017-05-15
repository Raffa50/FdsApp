using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace FdsApp{
    public static class PositionSender{
        private static DeviceClient _deviceClient;

        public static async Task SendPosition( double latitude, double longitude ) {
            if( _deviceClient == null ) 
                _deviceClient = DeviceClient.CreateFromConnectionString("HostName=FdsHub.azure-devices.net;DeviceId=device;SharedAccessKey=89VzpeHc7pq2IECaHO1scD+u1/6MZGbhXRYCeHpnNjw=");

            var message =  new {
                deviceID= "device",
                ApplicationUserId = ApiHelper.currentUser.Id,
                Latitude= latitude,
                Longitude= longitude,
                Time= DateTime.Now
            };

            string messageString = JsonConvert.SerializeObject(message);
            var messageToSend = new Message(Encoding.UTF8.GetBytes(messageString));

            await _deviceClient.SendEventAsync(messageToSend);
            Console.WriteLine("Message sent!");
        }
    }
}
