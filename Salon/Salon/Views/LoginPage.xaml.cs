using Salon.Model;
using Salon.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Salon
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        readonly UserRepository repository = new UserRepository();
        public LoginPage()
        {
            InitializeComponent();
        }

        //handles correct and incorrect logins
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            User existingUser = await repository.GetUser(usernameLbl.Text);

           // firebase login
            if (await repository.Login(usernameLbl.Text, passwordLbl.Text))
            {
                App.CurrentUser = existingUser.Username;
                Console.WriteLine("User logged in: " + App.CurrentUser);
                await Navigation.PushAsync(new NearbyUsersPage());
            }

            // non-firebase login for testing purposes
            else if (usernameLbl.Text == "admin" && passwordLbl.Text == "12345")
            {
                await Navigation.PushAsync(new NearbyUsersPage());
            }
            // empty login field
            else if (usernameLbl.Text == null || passwordLbl.Text == null)
            {
                ErrorLbl.Text = "*Your username or password is missing*";
            }
            else
            {
                ErrorLbl.Text = "*Your username or password is incorrect*";
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistrationPage());
        }

    }
}