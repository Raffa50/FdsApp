using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fds.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FdsApp{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EventPage : ContentPage{
		public EventPage (Event @event){
			InitializeComponent ();

		    EventDetail.BindingContext = @event;
            LblAge.Text= @event.AgeMin + ( @event.AgeMax == null ? "+" : "-"+@event.AgeMax );
		}

	    private void EventReviewClicked( object sender, EventArgs e ) {
	        var btn = (Button) sender;
	        var ev = btn.BindingContext as Event;

	        Navigation.PushAsync( new EventReviews( ev.Id ) );
	    }
	}
}
