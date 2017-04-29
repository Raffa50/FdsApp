using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using Plugin.Geolocator.Abstractions;
using Fds.Models;

namespace FdsApp{
	public partial class MainPage : ContentPage	{
	    private const string apiUrl = "http://fdsge.azurewebsites.net/api";
	    readonly HttpClient _client = new HttpClient();

        public MainPage() {
			InitializeComponent();

            /*var response = _client.GetAsync(apiUrl + "/Events").Result;
            if (!response.IsSuccessStatusCode) throw new Exception();
            var content = response.Content.ReadAsStringAsync().Result;
            var events = JsonConvert.DeserializeObject< ICollection< Event< User > > >( content );
            Console.WriteLine(events.Count);*/
            
            _updateEvents();

            //PositionSender.SendPosition( "1", 44.4056499, 8.946256 );
            //CrossGeolocator.Current.PositionChanged += _geoUpdate;
        }

	    private async void _updateEvents() {
	        var events= await new ApiHelper().GetEvents();
	        foreach( var e in events ) {
	            MyMap.Pins.Add( new Pin(){ Label = e.Name, Position = new Xamarin.Forms.Maps.Position(e.Latitude,e.Longitude)} );
	        }
	    }

        private async void _geoUpdate(object sender, PositionEventArgs e) {
            Console.WriteLine("Position updated");
            await PositionSender.SendPosition( "1", e.Position.Latitude, e.Position.Longitude );

            MyMap.MoveToRegion( MapSpan.FromCenterAndRadius( 
                new Xamarin.Forms.Maps.Position(e.Position.Latitude, e.Position.Longitude), 
                Distance.FromMiles(1))
            );
        }
    }
}
