using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Salon
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPage : ContentPage
	{
		public RegistrationPage ()
		{
			InitializeComponent ();
		}

        private void OnRegisterClicked(object sender, EventArgs e)
        {

            /*if (txtUsername.Text == "admin" && txtPassword.Text == "12345")
            {
                Navigation.PushAsync(new NearbyUsersPage());
            }
            else
            {
                ErrorLbl.Text = "Your username or password is incorrect";
            }*/

            RegErrorLbl.Text = "Register clicked";
        }
    }
}