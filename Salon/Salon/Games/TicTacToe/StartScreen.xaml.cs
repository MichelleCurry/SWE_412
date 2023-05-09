using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Salon
{
    public partial class StartScreen : ContentPage
    {
        public StartScreen()
        {
            InitializeComponent();
        }

        public void single_player_clicked(object sender, EventArgs args)
        {
            App.Current.MainPage = new single_player();
            Console.WriteLine("Single Player Clicked");
        }

        public void multi_player_clicked(object sender, EventArgs args)
        {
            App.Current.MainPage = new Multi_Player();
            Console.WriteLine("Multi Player Clicked");
        }

        public void Multi_Each_Btn_Clicked(object sender, EventArgs args)
        {

        }
        public void Back_Clicked(object sender, EventArgs args)
        {
            App.Current.MainPage = new NearbyUsersPage();            
        }
    }
}