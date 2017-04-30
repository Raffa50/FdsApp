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
		public EventPage (Event<User> @event){
			InitializeComponent ();

		    EventDetail.BindingContext = @event;
		}
	}
}
