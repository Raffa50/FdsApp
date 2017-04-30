using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FdsApp{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EventReviews : ContentPage{
		public EventReviews (int id){
			InitializeComponent ();

		    ReviewsList.ItemsSource = ApiHelper.GetEventReviews( id ).Result;
		}

	    private void CreateReviewClicked( object sender, EventArgs e ) {
	        if( ApiHelper.currentUser == null ) {
	            Navigation.PushAsync( new LoginPage() );
	        }
	    }
	}
}
