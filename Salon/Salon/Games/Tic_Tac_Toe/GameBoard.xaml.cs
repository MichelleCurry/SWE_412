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
    public partial class GameBoard : ContentPage
    {
        private readonly Button[,] btnList;
        private readonly char[,] gameBoard;
        private readonly char playerIcon;
        private readonly String gameType;
        private readonly MiniMaxAI aiPlayer;
        private bool isGameOver; // Added variable to track if the game is over

        public GameBoard(char playerIcon, String gametype)
        {
            InitializeComponent();
            this.playerIcon = playerIcon;
            this.gameType = gametype;

            btnList = new Button[3, 3] {
                { box1, box2, box3 },
                { box4, box5, box6 },
                { box7, box8, box9 }
            };

            gameBoard = new char[3, 3] {
                { '-', '-', '-' },
                { '-', '-', '-' },
                { '-', '-', '-' }
            };
            aiPlayer = new MiniMaxAI(playerIcon);
            isGameOver = false; // Initialize isGameOver to false
        }

        public char CheckWinner()
        {
            // Check Rows
            for (int i = 0; i < 3; i++)
            {
                if (gameBoard[i, 0] == gameBoard[i, 1] && gameBoard[i, 1] == gameBoard[i, 2] && gameBoard[i, 0] != '-')
                {
                    isGameOver = true;
                    return gameBoard[i, 0];
                }
            }
            // Check Columns
            for (int i = 0; i < 3; i++)
            {
                if (gameBoard[0, i] == gameBoard[1, i] && gameBoard[1, i] == gameBoard[2, i] && gameBoard[0, i] != '-')
                {
                    isGameOver = true;
                    return gameBoard[0, i];
                }
            }
            // Check Diagonals
            if (gameBoard[0, 0] == gameBoard[1, 1] && gameBoard[1, 1] == gameBoard[2, 2] && gameBoard[0, 0] != '-')
            {
                isGameOver = true;
                return gameBoard[0, 0];
            }
            if (gameBoard[0, 2] == gameBoard[1, 1] && gameBoard[1, 1] == gameBoard[2, 0] && gameBoard[0, 2] != '-')
            {
                isGameOver = true;
                return gameBoard[0, 2];
            }
            // If no winner found and the board is full, return tie
            if (IsBoardFull())
            {
                isGameOver = true;
                return 'T';
            }
            return '-'; // Return '-' if no winner and game still in progress
        }

        public bool IsBoardFull()
        {
            // Check if any cell in the board still has a blank symbol
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameBoard[i, j] == '-')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Position_Clicked(object sender, EventArgs args)
        {
            if (isGameOver) return;
            Button btn = (Button)sender;
            int row = Grid.GetRow(btn);
            int col = Grid.GetColumn(btn);

            if (gameBoard[row, col] == '-')
            {
                Console.WriteLine("Human turn");
                gameBoard[row, col] = playerIcon;
                Display_Move(btn, playerIcon);
                char winner = CheckWinner();

                if (winner == 'T')
                {
                    DisplayAlert("Tie", "Its a tie no one won", "OK");
                }
                else if (winner != '-')
                {
                    DisplayAlert("Winner", winner + " has won the game!", "OK");
                }
                else
                {
                    Console.WriteLine("AI turn");
                    int[,] bestMove = aiPlayer.GetBestMove(gameBoard, winner);

                    Console.WriteLine(aiPlayer.player);
                    gameBoard[bestMove[0, 0], bestMove[0, 1]] = aiPlayer.player;
                    Button aiBtn = btnList[bestMove[0, 0], bestMove[0, 1]];
                    Display_Move(aiBtn, aiPlayer.player);
                    winner = CheckWinner();

                    if (winner != '-')
                    {
                        DisplayAlert("Winner", winner + " has won the game!", "OK");
                    }
                }
            }
        }

        public void Display_Move(Button btn, char icon)
        {
            btn.FontSize = 30;
            btn.Text = icon.ToString();
            btn.BackgroundColor = Color.Transparent;
            btn.IsEnabled = false;
        }

        //EDITED
        public void Back_Clicked(object sender, EventArgs args)
        {
            App.Current.MainPage = new StartPage();
        }

        public void Reset_Clicked(object sender, EventArgs args)
        {
            App.Current.MainPage = new StartPage();

        }
    }
}