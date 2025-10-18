using System.Collections.Generic;

namespace ConnectFour
{


    public abstract class Move
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public char Symbol { get; set; }
        public string DiscType { get; set; }
        public int MoveNumber { get; set; }

        // Abstract method to restore cells removed by special disc effects
        public abstract void RestoreRemovedCells(Disc[,] grid, GameInventory inventory);
    }

    // Move for ordinary discs with no side effects
    public class MoveOrdinary : Move
    {
        public override void RestoreRemovedCells(Disc[,] grid, GameInventory inventory)
        {
            // No cells to restore for ordinary moves
        }
    }

    // Move for special discs that can remove or change other cells
    public class MoveWithEffect : Move
    {
        public List<SavedCell> RemovedCells { get; set; }

        public MoveWithEffect()
        {
            RemovedCells = new List<SavedCell>();
        }

        public override void RestoreRemovedCells(Disc[,] grid, GameInventory inventory)
        {
            // Restore all cells that were removed by the special disc effect
            foreach (SavedCell cell in RemovedCells)
            {
                Disc restoredDisc = Disc.CreateDiscFromSymbol(cell.Symbol);
                grid[cell.Row, cell.Column] = restoredDisc;

                int player = cell.Symbol switch
                {
                    '@' or 'B' or 'M' or 'E' => 1,
                    '#' or 'b' or 'm' or 'e' => 2,
                    _ => 0
                };
                inventory.RestoreDisc(player, cell.DiscType);
            }
        }
    }

    // Stores information about a cell that was removed by a special disc effect
    public class SavedCell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public char Symbol { get; set; }
        public string DiscType { get; set; }
    }

}