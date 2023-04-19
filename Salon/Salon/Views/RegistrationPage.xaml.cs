using Salon.Model;
using Salon.Services;
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
        readonly UserRepository repository = new UserRepository();

        public string WebAPIkey = "AIzaSyBG9G6i1YH_LF0rTfvIYUUfSnJ6fhQlbTs";

        public RegistrationPage ()
		{
			InitializeComponent ();
		}

        // Adds newly registered users to the database
        public async void OnRegisterClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) 
                || string.IsNullOrEmpty(txtPassword.Text) 
                || string.IsNullOrEmpty(txtEmail.Text))
            {
                RegErrorLbl.Text = "all fields must be filled";
            } else
            {
                User user = new User
                {
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text
                };

                var isSaved = await repository.Save(user);
                if (isSaved)
                {
                    RegErrorLbl.Text = ":))))) User has been saved";
                    await Navigation.PushAsync(new NearbyUsersPage());
                }
                else
                {
                    RegErrorLbl.Text = "****** User could not be saved";
                }
            }

        }
    }
}