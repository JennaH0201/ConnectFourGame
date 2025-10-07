using System;

namespace ConnectFour
{
    public class PlayerTestRunner
    {
        public static void RunTestSequence(string input, GameInventory inventory)
        {
            string[] moves = input.Split(',');

            DrawGrid grid = new DrawGrid(inventory.Rows, inventory.Columns, inventory);
            inventory.moveCounter = 1;

            foreach (string move in moves)
            {
                try
                {
                    if (move.Length < 2)
                        throw new ArgumentException($"Invalid move format: {move}");

                    char discChar = char.ToUpper(move[0]);
                    int column = int.Parse(move.Substring(1)) - 1; // Converts to 0-based index player knows it starts at column 1 

                    int player = inventory.moveCounter % 2 != 0 ? 1 : 2;

                    char symbol = discChar switch
                    {
                        'O' => player == 1 ? '@' : '#',
                        'B' => player == 1 ? 'B' : 'b',
                        'M' => player == 1 ? 'M' : 'm',
                        _ => throw new ArgumentException($"Unknown disc type: {discChar}")
                    };

                    int dropRow = grid.PlaceDisc(symbol, player, column);

                    if (dropRow == -1)
                    {
                        Console.WriteLine($"Move {move} failed: Column full");
                        continue;
                    }

                    inventory.moveCounter++;
                    grid.DisplayGrid(inventory.moveCounter);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing move '{move}': {ex.Message}");
                }
            }

            Console.WriteLine("Test sequence complete.");
        }
    }
}