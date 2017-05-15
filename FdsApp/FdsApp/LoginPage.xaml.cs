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
	public partial class LoginPage : ContentPage{
		public LoginPage (){
			InitializeComponent ();
		}

	    private void LoginClicked( object sender, EventArgs e ) {
	        if( ApiHelper.Login( EmailText.Text, PasswordText.Text ).Result ) {
	            DisplayAlert( "Login", "Ora sei loggato!", "Ok" );
	            Navigation.PushModalAsync( new NavigationPage( new MainPage() ) );
	        } else
	            DisplayAlert( "Login", "Errore di login!", "Ok" );
	    }
	}
}
