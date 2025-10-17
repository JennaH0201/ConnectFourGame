using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    public class DiscExplode : Disc
    {
        public DiscExplode(char symbol) : base(symbol) { }


        public override void ApplyEffect(Disc[,] grid, int row, int col, GameInventory inventory)
        {
            int[] dr = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dc = { -1, 0, 1, -1, 1, -1, 0, 1 };

            grid[row, col] = null; // Remove the exploding disc itself

            for (int i = 0; i < dr.Length; i++)
            {
                int r = row + dr[i];
                int c = col + dc[i];

                if (r >= 0 && r < grid.GetLength(0) && c >= 0 && c < grid.GetLength(1))
                {
                    grid[r, c] = null;
                }
            }

            // Reapply gravity to affected columns
            for (int c = col - 1; c <= col + 1; c++)
            {
                if (c >= 0 && c < grid.GetLength(1))
                {
                    List<Disc> columnDiscs = new List<Disc>();
                    for (int r = 0; r < grid.GetLength(0); r++)
                    {
                        if (grid[r, c] != null)
                            columnDiscs.Add(grid[r, c]);
                    }

                    for (int r = 0; r < grid.GetLength(0); r++)
                        grid[r, c] = r < columnDiscs.Count ? columnDiscs[r] : null;
                }
            }






        }
    }
}
