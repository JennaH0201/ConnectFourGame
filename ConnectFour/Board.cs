using System;

namespace ConnectFour
{

    //6*7 grid and customized grid
    public class Board
    {
        public int Rows { get; }
        public int Cols { get; }
        private char[,] grid;


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


        public void Print()
        {
            Console.WriteLine();
            // rows=0 is bottom
            for (int r = Rows - 1; r >= 0; r--)
            {
                Console.Write((r + 1).ToString().PadLeft(2));
                Console.Write(" ");

                for (int c = 0; c < Cols; c++)
                {
                    char symbol = grid[r, c];
                    if (symbol == ' ')
                        Console.Write("|   ");
                    else
                        Console.Write("|  " + symbol + "  ");
                }
                Console.WriteLine("|");

            }
            Console.Write("   ");
            for (int c = 1; c <= Cols; c++)
            {
                if (c < 20)
                    Console.Write("  " + c + "  ");
                else
                    Console.Write("  " + c + " ");
            }

        }
        
    }
};

