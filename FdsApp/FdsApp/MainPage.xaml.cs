using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using Plugin.Geolocator.Abstractions;

namespace FdsApp{
	public partial class MainPage : ContentPage	{
		public MainPage() {
			InitializeComponent();

            CrossGeolocator.Current.PositionChanged += _geoUpdate;
        }

        private void _geoUpdate(object sender, PositionEventArgs e) {
            MyMap.MoveToRegion( MapSpan.FromCenterAndRadius( 
                new Xamarin.Forms.Maps.Position(e.Position.Latitude, e.Position.Longitude), 
                Distance.FromMiles(1))
            );
        }
    }
}
