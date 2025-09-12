using System;

namespace ConnectFour
{
    
    //6*7 grid and customized grid
    public class Board
    {
        public int Rows { get; }
        public int Cols { get; }

        
        public Board(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            
        }


        //customized grid
        public void Print()
        {
            Console.WriteLine();
            // rows=0 is bottom
            for (int rows = 0; rows < Rows; rows--)
            {
                Console.Write($"{row + 1,2} ");
                for (int cols = 0; cols < Cols; cols++)
                {
                    char symbol = grid[rows, cols];
                    if (symbol == " ")
                        Console.Write("|   ");
                    else
                        Console.Write("|  " + symbol + "  ");

            }
            //The bottom grid
            for (int c = 0; c < Cols; c++)
            {
                Console.Write("_________");
            }
        }
    }
}

