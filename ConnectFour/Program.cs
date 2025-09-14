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
                Console.WriteLine("4. Test Mode");
                Console.WriteLine("5. Exit");

                Console.Write("Select an option (1-5): ");
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
                        Console.WriteLine("This is the test mode.");
                        break;
                    case 5:
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

            Console.Clear();

            Console.WriteLine("Starting a new game!\n");
            Console.WriteLine("1. Basic Grid (6x7)");
            Console.WriteLine("2. Custom Grid");
            Console.Write("Select an option (1-2): ");
            string? gridSize = Console.ReadLine();

            int rows;
            int cols;

            if (gridSize == "2")
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Starting a customized game!");
                rows = PromptForInt("Enter the rows number *minimun 6: ", 6);
                Console.WriteLine();
                cols = rows + 1;
            }

            else
            //6*7 grid game
            {
                Console.WriteLine();
                Console.WriteLine("Starting a basic game!");
                Console.WriteLine();
                rows = 6; cols = 7;
            }

            var board = new Board(rows, cols);
            char player1Symbol = '@';
            char player2Symbol = '#';
            var player1Disc = new OrdinaryDisc(player1Symbol.ToString());
            var player2Disc = new OrdinaryDisc(player2Symbol.ToString());

            board.Print();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Player 1's turn (@): ");
                string? input1 = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input1) || input1.Length < 2)
                {
                    Console.WriteLine("Invalid input. Enter the correct move.");
                    continue;

                }
                char discType1 = char.ToLowerInvariant(input1[0]);
                if (discType1 != 'o' && discType1 != 'b' && discType1 != 'm')
                {
                    Console.WriteLine("Invalid disc type. Enter 'o', 'b', or 'm'.");
                    continue;
                }
                string colStr = input1.Substring(1);
                
                if (!int.TryParse(colStr, out int colInput1) || colInput1 < 1 || colInput1 > cols)
                {
                    Console.WriteLine("Invalid column. Enter a column number correctly.");
                    continue;
                }
                int colBased1 = colInput1 - 1;
                
                bool succeed = player1Disc.DropDisc(board, colBased1);
                if (!succeed)
                {
                        Console.WriteLine("Column is full. Try a different column.");
                        continue;
                }

                //winning check
                /*if (board.CheckForConnectFour('@'))
                {
                    board.Print();
                    Console.WriteLine("Player 1 (@) wins! Congratulations!");
                    break;
                } */

                Console.Clear();
                board.Print(); 

                //player 2.
                Console.WriteLine();
                Console.WriteLine("Player 2's turn (#): ");
                string? input2 = Console.ReadLine();

              
                if (string.IsNullOrEmpty(input2) || input2.Length < 2)
                {
                    Console.WriteLine("Invalid input. Enter a correct move.");
                    continue;
                }
                char discType2 = char.ToLowerInvariant(input2[0]);
                
                if (discType2 != 'o' && discType2 != 'b' && discType2 != 'm')
                {
                    Console.WriteLine("Invalid disc type. Enter 'o', 'b', or 'm'.");
                    continue;
                }
                
                string colStr2 = input2.Substring(1);
                if (!int.TryParse(colStr2, out int colInput2) || colInput2 < 1 || colInput2 > cols)
                {
                    Console.WriteLine("Invalid column. Enter a column number correctly.");
                    continue;
                }
                int colbased2 = colInput2 - 1;

                
                bool succeed2 = player2Disc.DropDisc(board, colbased2);
                if (!succeed2)
                {
                    Console.WriteLine("Column is full. Try the different column.");
                    continue;
                }
                
                //winning check
                /*if (board.CheckForConnectFour('#'))
                {
                    board.Print();
                    Console.WriteLine("Player 2 (#) wins! Congratulations!");
                    break;
                } */ 
                Console.Clear();
                board.Print(); 
            }

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