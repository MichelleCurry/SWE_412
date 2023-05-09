using Salon.Model;
using Salon.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Salon
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        int failedLogin = 0;
        bool lockout = false;
        bool loggedIn = false;

        public void onLockoutEvent(object sender, EventArgs e)
        {
            lockout = false;
            failedLogin = 0;
            ErrorLbl.Text = string.Empty;
        }

        public void setLockoutTimer()
        {
            System.Timers.Timer lockoutTimer = new System.Timers.Timer(86400000);
            lockoutTimer.Elapsed += onLockoutEvent;
            lockoutTimer.AutoReset = false;
            lockoutTimer.Start();
        }

        public void onUserTimeoutEvent(object sender, EventArgs e)
        {
            loggedIn = false;
        }

        public void setLoggedInTimer()
        {
            System.Timers.Timer loggedInTimer = new System.Timers.Timer(86400000);
            loggedInTimer.Elapsed += onUserTimeoutEvent;
            loggedInTimer.AutoReset = false;
            loggedInTimer.Start();
        }

        readonly UserRepository repository = new UserRepository();
        public LoginPage()
        {
            InitializeComponent();
            if (loggedIn)
            {
                //setLoggedInTimer();
                App.Current.MainPage = new NearbyUsersPage();
            }
        }

        //handles correct and incorrect logins
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            //if the user has logged in 5 or more times unsuccessfully,
            //a 24 hour timer is set and logins are automatically denied
            if(failedLogin >= 5 || lockout)
            {
                lockout = true;
                ErrorLbl.Text = "Too many login attempts. Try again in 24 hours";
                setLockoutTimer();
                failedLogin++;
                return;
            }
            
            User existingUser = await repository.GetUser(usernameLbl.Text);

           // firebase login
            if (await repository.Login(usernameLbl.Text, passwordLbl.Text))
            {
                App.CurrentUser = existingUser.Username;
                Console.WriteLine("User logged in: " + App.CurrentUser);
                App.Current.MainPage = new NearbyUsersPage();
            }

            // non-firebase login for testing purposes
            else if (usernameLbl.Text == "admin" && passwordLbl.Text == "12345")
            {
                App.Current.MainPage = new NearbyUsersPage();
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
            App.Current.MainPage = new RegistrationPage();
        }

    }
}