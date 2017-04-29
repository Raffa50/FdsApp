﻿using System;
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

		    PositionSender.SendPosition( "1", 44.4056499, 8.946256 );
            CrossGeolocator.Current.PositionChanged += _geoUpdate;
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
