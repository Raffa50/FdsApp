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
        readonly Dictionary<int,Pin> _pinsDictionary= new Dictionary< int, Pin >();

        public MainPage() {
			InitializeComponent();
            
            _updateEvents();

            //PositionSender.SendPosition( "1", 44.4056499, 8.946256 );
            CrossGeolocator.Current.PositionChanged += _geoUpdate;
        }

	    private async void _updateEvents() {
	        var events= await ApiHelper.GetEvents();

	        EventsListView.ItemsSource = events;

            foreach ( var e in events ) {
                var pin = new Pin() {
                    Label = e.Name,
                    Position = new Xamarin.Forms.Maps.Position( e.Latitude, e.Longitude )
                };

                _pinsDictionary.Add( e.Id, pin );

                MyMap.Pins.Add( pin );
	        }
	    }

        private async void _geoUpdate(object sender, PositionEventArgs e) {
            await PositionSender.SendPosition( "1", e.Position.Latitude, e.Position.Longitude );

            MyMap.MoveToRegion( MapSpan.FromCenterAndRadius( 
                new Xamarin.Forms.Maps.Position(e.Position.Latitude, e.Position.Longitude), 
                Distance.FromMiles(1))
            );
        }

	    private void EventIdOnClicked( object sender, EventArgs e ) {
	        var btn = (Button) sender;
	        var eventId = int.Parse( btn.Text );

            MyMap.MoveToRegion( MapSpan.FromCenterAndRadius( _pinsDictionary[eventId].Position, Distance.FromMiles(1) ) );
	    }

	    private void EventNameOnClicked( object sender, EventArgs e ) {
	        var btn = (Button) sender;
	        var ev= btn.BindingContext as Event;
	        Navigation.PushAsync( new EventPage(ev) );
	    }
	}
}
