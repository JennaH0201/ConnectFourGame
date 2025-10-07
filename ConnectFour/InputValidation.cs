using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    public class InputValidation
    {
        public static int GetValidatedInteger(string prompt, int min, int max)
        {
            int result;
            Console.Write(prompt);
            string input = Console.ReadLine();

            while (!int.TryParse(input, out result) || result < min || result > max)
            {
                Console.Write($"Invalid input. Please enter a number between {min} and {max}: ");
                input = Console.ReadLine();
            }

            return result;
        }


        public static int ComputeColumnsFromRows(int rows)
        {
            return (int)Math.Round(rows * (7.0 / 6.0));
        }

        public static (Disc disc, int column) ParseInput(string input, int moveCounter, GameInventory inventory, int maxColumns)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
                throw new ArgumentException("Invalid input format. Use format like o4 or M7.");

            char typeChar = char.ToLower(input[0]);
            string columnPart = input.Substring(1);

            if (!int.TryParse(columnPart, out int column) || column < 1 || column > maxColumns)
                throw new ArgumentException("Invalid column number.");

            int player = moveCounter % 2 != 0 ? 1 : 2;

            // Map disc type to player-specific symbol
            char symbol = (player, typeChar) switch
            {
                (1, 'o') => '@',
                (2, 'o') => '#',
                (1, 'b') => 'B',
                (2, 'b') => 'b',
                (1, 'm') => 'M',
                (2, 'm') => 'm',
                _ => throw new ArgumentException("Invalid disc type.")
            };

            if (!inventory.IsDiscAvailable(player, symbol))
                throw new InvalidOperationException("No remaining discs of that type.");

            Disc disc = Disc.CreateDiscFromSymbol(symbol);
            return (disc, column - 1); // Convert to 0-based index
        }
  


    }
}
