using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Salon.Games.Tic_Tac_Toe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        private static char PlayerIcon { get; set; }
        public StartPage()
        {
            InitializeComponent();
            PlayerIcon = 'X';
        }
        public void single_player_clicked(object sender, EventArgs args)
        {
            SetPlayerIcon();
            App.Current.MainPage = new GameBoard(PlayerIcon, "sp");
            Console.WriteLine("Single Player Clicked");
        }

        public void multi_player_clicked(object sender, EventArgs args)
        {
            SetPlayerIcon();
            App.Current.MainPage = new GameBoard(PlayerIcon, "mp");
            Console.WriteLine("Multi Player Clicked");
        }

        private void SetPlayerIcon()
        {
            if (XIcon.IsChecked)
            {
                PlayerIcon = 'X';
            }
            else
            {
                PlayerIcon = 'O';
            }

        }
        public void Back_Clicked(object sender, EventArgs args)
        {
            App.Current.MainPage = new StartPage();
        }
    }
}