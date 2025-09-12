using System;
using ConnectFour;

namespace ConnectFour
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n Let's play Connect Four!\n");
                Console.WriteLine();
                Console.WriteLine("1. New Game");
                Console.WriteLine("2. Load Game");
                Console.WriteLine("3. Help");
                Console.WriteLine("4. Exit");

                Console.Write("Select an option (1-4): ");
                int mode = Convert.ToInt32(Console.ReadLine());

                switch (mode)
                {
                    case 1:
                        StartNewGame();
                        break;
                    case 2:
                        LoadGame();
                        break;
                    case 3:
                        ShowHelpPage();
                        break;
                    case 4:
                        Console.WriteLine("Exiting the game. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please select 1, 2, or 3.");
                        break;
                }
            }
        }
        static void StartNewGame()
        {
            //bring up a new Game

            Console.WriteLine("Starting a new game!\n");
            Console.WriteLine("1. Basic Grid (6x7)");
            Console.WriteLine("2. Custom Grid");
            Console.Write("Select an option (1-2): ");
            string? gridSize = Console.ReadLine();

            int rows;
            int cols;

            if (gridSize == "2")
            {
                Console.WriteLine("Starting a custom game!");
                rows = PromptForInt("Enter the rows number *minimun 6: ", 6);
                cols = rows + 1;
            }

            else
            //6*7 grid game
            {
                Console.WriteLine("Starting a basic game!");
                rows = 6; cols = 7;
            }

            var board = new Board(rows, cols);
            board.Print();
        }

        static int PromptForInt(string message, int min)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int number))
                {
                    if (number >= min)
                        return number;

                    else
                    {
                        Console.WriteLine("Error: Minimun is " + min);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a correct number");
                }
            }
        }

        static void LoadGame()
        {
            Console.Clear();
            Console.WriteLine("Loading a saved game....");
            Console.WriteLine("Press Enter to return to the main page");
            Console.ReadLine();
        }

        static void ShowHelpPage()
        {
            Console.Clear();
            Console.WriteLine("This is help page....");
            Console.WriteLine("Press Enter to return to the main page");
            Console.ReadLine();
        }
    }
}

        

