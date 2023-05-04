using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Salon
{
    public partial class App : Application
    {
        public static String CurrentUser { get; set; }

        public App()
        {
            InitializeComponent();

            //change page in the brackets to change first loaded page

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
