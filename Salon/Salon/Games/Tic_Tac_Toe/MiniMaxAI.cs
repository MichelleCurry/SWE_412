using System;
using System.Collections.Generic;
using System.Text;

namespace Salon.Games.Tic_Tac_Toe
{
    internal class MiniMaxAI
    {
        public char player { get; set; }
        private char opponent { get; set; }

        readonly private int maxDepth = 3;

        public MiniMaxAI(char oppPlayer)
        {
            opponent = oppPlayer;
            player = oppPlayer == 'X' ? 'O' : 'X';
            Console.WriteLine("AI is: " + player);
        }

        public int[,] GetBestMove(char[,] board, char winner)
        {
            int bestScore = int.MinValue;
            int row = 0;
            int col = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '-')
                    {
                        board[i, j] = player;
                        int score = MiniMax(board, 0, false, winner, int.MinValue, int.MaxValue);
                        board[i, j] = '-';
                        if (score > bestScore)
                        {
                            bestScore = score;
                            row = i;
                            col = j;
                        }
                    }
                }
            }
            Console.WriteLine("Chose position " + ((row * 3) + ((col + 1))));
            return new int[,] { { row, col } };
        }

        public int MiniMax(char[,] board, int depth, bool isMaximizing, char winner, int alpha, int beta)
        {
            char currentWinner = CheckWinner(board);
            if (currentWinner != '-')
            {
                return GetScore(currentWinner, depth);
            }

            Console.WriteLine("Depth: " + depth);
            if (depth == maxDepth)
            {
                return 0;
            }

            int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int row = i;
                    int col = j;

                    if (board[row, col] == '-')
                    {
                        char[,] testBoard = (char[,])board.Clone();
                        testBoard[row, col] = isMaximizing ? player : opponent;

                        int score = MiniMax(testBoard, depth + 1, !isMaximizing, winner, alpha, beta);

                        if (isMaximizing)
                        {
                            bestScore = Math.Max(score, bestScore);
                            alpha = Math.Max(alpha, score);
                        }
                        else
                        {
                            bestScore = Math.Min(score, bestScore);
                            beta = Math.Min(beta, score);
                        }

                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
            }

            return bestScore;
        }

        public int GetScore(char winner, int depth)
        {
            if (winner == player)
            {
                return 10 - depth;
            }
            else if (winner == opponent)
            {
                return depth - 10;
            }
            return 0;
        }

        public char CheckWinner(char[,] board)
        {
            // Check Rows
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != '-')
                {
                    return board[i, 0];
                }
            }
            // Check Columns
            for (int i = 0; i < 3; i++)
            {
                if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != '-')
                {
                    return board[0, i];
                }
            }
            // Check Diagonals
            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != '-')
            {
                return board[0, 0];
            }
            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != '-')
            {
                return board[0, 2];
            }
            // If no winner found and the board is full, return tie
            if (IsBoardFull(board))
            {
                return 'T';
            }
            return '-'; // Return '-' if no winner and game still in progress
        }

        public bool IsBoardFull(char[,] board)
        {
            bool isFull = true;
            foreach (char c in board)
            {
                if (c == '-')
                {
                    isFull = false;
                }
            }
            return isFull;
        }
    }
}