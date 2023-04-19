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

        public void onTimedEvent(object sender, EventArgs e)
        {
            lockout = false;
            failedLogin = 0;
            ErrorLbl.Text = string.Empty;
            
        }

        public void setLoginTimer()
        {
            System.Timers.Timer lockoutTimer = new System.Timers.Timer(86400000);
            lockoutTimer.Elapsed += onTimedEvent;
            lockoutTimer.AutoReset = false;
            lockoutTimer.Start();
        }

        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            //if the user has logged in 5 or more times unsuccessfully,
            //a 24 hour timer is set and logins are automatically denied
            if(failedLogin >= 5 || lockout == true)
            {
                lockout = true;
                ErrorLbl.Text = "Too many login attempts. Try again in 24 hours";
                setLoginTimer();
                failedLogin++;
                return;
            }
            
            //normal login authentication
            if (txtUsername.Text == "admin" && txtPassword.Text == "12345")
            {
                Navigation.PushAsync(new NearbyUsersPage());
            }
            else 
            {
                ErrorLbl.Text = "Your username or password is incorrect";
                failedLogin++;
            }

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistrationPage());
        }

    }
}