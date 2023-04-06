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
        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            /*var fireBaseClient = new Firebase.Database.FirebaseClient("https://salon-ff877-default-rtdb.firebaseio.com/");
            
            PublicKey bool Save(User user)
            fireBaseClient.Child("Records").PostAsync(new MyDatabaseRecord
            {
                DBUsername = txtUsername.Text,
                DBPassword = txtPassword.Text

            });*/

            if (txtUsername.Text == "admin" && txtPassword.Text == "12345")
            {
                Navigation.PushAsync(new NearbyUsersPage());
            }
            else
            {
                ErrorLbl.Text = "Your username or password is incorrect";
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistrationPage());
        }

    }
}