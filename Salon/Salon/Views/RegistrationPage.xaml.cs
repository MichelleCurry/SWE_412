using Salon.Model;
using Salon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public RegistrationPage ()
		{
			InitializeComponent ();
		}

        // Adds newly registered users to the database
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rUsernameLbl.Text)
                || string.IsNullOrEmpty(rPasswordLbl.Text)
                || string.IsNullOrEmpty(rEmailLbl.Text))
            {
                regErrorLbl.Text = "all fields must be filled";
            }
            else
            {
                Model.User user = new Model.User
                {
                    Username = rUsernameLbl.Text,
                    Password = rPasswordLbl.Text,
                    Email = rEmailLbl.Text

                };

                regErrorLbl.Text = "user made";
                //await Task.Delay(1500);

                var isSaved = await repository.RegisterUser(user);
                if (isSaved)
                {
                    regErrorLbl.Text = ":))))) User has been saved";
                }
                else
                {
                    //await Task.Delay(2500);
                    regErrorLbl.Text = "****** User could not be saved";
                }
            }

        }
    }
}