using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace FdsSendPositionTest{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page{
        private static DeviceClient _deviceClient;
        private readonly DispatcherTimer _timer;

        public MainPage(){
            this.InitializeComponent();

            _deviceClient = DeviceClient.CreateFromConnectionString("HostName=FdsHub.azure-devices.net;DeviceId=device;SharedAccessKey=89VzpeHc7pq2IECaHO1scD+u1/6MZGbhXRYCeHpnNjw=");

            //timer setup and start -> 1 msg every second
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private async void _timer_Tick( object sender, object e ) {
            var message =
                new {
                    deviceID = "device",
                    ApplicationUserId= "1",
                    Latitude= 44.4056499,
                    Longitude= 8.946256,
                    Time = DateTime.Now
                };

            string messageString = JsonConvert.SerializeObject(message);
            var messageToSend = new Message(Encoding.UTF8.GetBytes(messageString));

            //send message to IoTHub
            await _deviceClient.SendEventAsync(messageToSend);
        }
    }
}
