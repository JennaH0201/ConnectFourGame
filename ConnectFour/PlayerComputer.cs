using System;
using System.Collections.Generic;

namespace ConnectFour
{
    public class PlayerComputer
    {
        private Random rng = new Random();

        public int FindWinningMove(DrawGrid grid, GameInventory inventory)
        {
            int rows = inventory.Rows;
            int columns = inventory.Columns;
            char symbol = '#'; // Player 2 ordinary disc indicating its the computers turn

            for (int col = 0; col < columns; col++)
            {
                int row = grid.GetDropRow(col);
                if (row == -1) continue; // Column full indication

                // Simulate placing the disc
                Disc simulatedDisc = new DiscOrdinary(symbol);
                grid.Grid[row, col] = simulatedDisc;

                bool isWin = grid.CheckWin(row, col);

                // Undo simulation
                grid.Grid[row, col] = null;

                if (isWin)
                    return col;
            }

            return -1; // No winning move found
        }

        public int ChooseColumnWithFallback(DrawGrid grid, GameInventory inventory)
        {
            int winningCol = FindWinningMove(grid, inventory);
            if (winningCol != -1)
                return winningCol;

            // Fallback: pick a random valid column
            List<int> validColumns = new List<int>();
            for (int col = 0; col < inventory.Columns; col++)
            {
                if (grid.GetDropRow(col) != -1)
                    validColumns.Add(col);
            }

            if (validColumns.Count == 0) return -1;
            return validColumns[rng.Next(validColumns.Count)];
        }

        public (Disc, int) MakeMove(DrawGrid grid, GameInventory inventory)
        {
            int column = ChooseColumnWithFallback(grid, inventory);
            if (column == -1)
                throw new InvalidOperationException("No valid columns available.");

            // Choose disc type based on availability
            char symbol;
            string type;

            if (inventory.IsDiscAvailable(2, '#'))
            {
                symbol = '#';
                type = "ordinary";
            }
            else if (inventory.IsDiscAvailable(2, 'b'))
            {
                symbol = 'b';
                type = "boring";
            }
            else if (inventory.IsDiscAvailable(2, 'm'))
            {
                symbol = 'm';
                type = "magnetic";
            }
            else
            {
                throw new InvalidOperationException("No discs available for Player 2.");
            }


            Disc disc = Disc.CreateDiscFromSymbol(symbol);
            return (disc, column);
        }
    }
}