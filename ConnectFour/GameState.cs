using System;

namespace ConnectFour
{
    public class GameState
    {
        // Core game state
        public GameInventory Inventory { get; set; }

        // Jagged Array: each cell holds a disc symbol or empty string
        public string[][] GridSymbols { get; set; }

        public DateTime SaveTime { get; set; } = DateTime.Now;
        public string CurrentPlayer => Inventory.moveCounter % 2 != 0 ? Inventory.PlayerOneName : Inventory.PlayerTwoName;

        public GameState() { }

        public GameState(GameInventory inventory, DrawGrid grid)
        {
            Inventory = inventory;
            GridSymbols = new string[inventory.Rows][];

            for (int r = 0; r < inventory.Rows; r++)
            {
                GridSymbols[r] = new string[inventory.Columns];
                for (int c = 0; c < inventory.Columns; c++)
                {
                    GridSymbols[r][c] = grid.Grid[r, c]?.Symbol.ToString() ?? "";
                }
            }
        }

        // drawing the DrawGrid from saved symbols
        public DrawGrid RestoreGrid()
        {
            DrawGrid restoredGrid = new DrawGrid(Inventory.Rows, Inventory.Columns, Inventory);

            for (int r = 0; r < Inventory.Rows; r++)
            {
                for (int c = 0; c < Inventory.Columns; c++)
                {
                    string symbol = GridSymbols[r][c];
                    restoredGrid.Grid[r, c] = string.IsNullOrEmpty(symbol) ? null : Disc.CreateDiscFromSymbol(symbol[0]);
                }
            }
            return restoredGrid;
        }
    }
}