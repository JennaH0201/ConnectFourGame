using System;

namespace ConnectFour
{

    //6*7 grid and customized grid
    public class Board
    {
        public int Rows { get; }
        public int Cols { get; }
        private char[,] grid;

        //winning 
        /*public bool CheckForConnectFour(char symbol)
        {
            int need = WinningDiscs;
        if (symbol == ' ') return false;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    if (grid[r, c]! == symbol)
                        continue;
                    if (CheckDirection(row, col, symbol, 0, 1)  //vertical
                        || CheckDirection(row, col, symbol, 1, 0) //horizontal
                        || CheckDirection(row, col, symbol, 1, 1) //diagonal /
                        || CheckDirection(row, col, symbol, 1, -1)) //diagonal \
                        return true;
                }
            }

            return false;
        } */


        public int WinningDiscs => (int)Math.Round(Rows * Cols * 0.1);
        public bool IsCellEmpty(int row, int col)
        {
            return grid[row, col] == ' ';
        }

        public Board(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            grid = new char[Rows, Cols];

            //make the grid empty
            for (int r = Rows - 1; r >= 0; r--)
            {
                for (int c = 0; c < Cols; c++)
                {
                    grid[r, c] = ' ';
                }
            }

        }

        public void PlaceDisc(int row, int col, char symbol)
        {
            grid[row, col] = symbol;
        }

        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Move your dics, Player 1 (@) / Player 2 (#)");
            Console.WriteLine("You can enter o3, b3, m5");
            Console.WriteLine("ex) o3 is ordinary disc at column 3");
            Console.WriteLine();
            Console.ResetColor();
            // rows=0 is bottom
            for (int r = Rows - 1; r >= 0; r--)
            {
                Console.Write((r + 1).ToString().PadLeft(2));
                Console.Write(" ");

                for (int c = 0; c < Cols; c++)
                {
                    char symbol = grid[r, c];
                    if (symbol == ' ') symbol = ' ';
                        Console.Write($"| {symbol} ");

                }
                Console.WriteLine("|");

            }
            //column numbers
            Console.Write("   ");
            for (int c = 0; c < Cols; c++)
                Console.Write($"  {c + 1} ");
            Console.WriteLine();
        }
        
    }
};