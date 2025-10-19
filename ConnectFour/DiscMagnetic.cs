using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    public class DiscMagnetic : Disc
    {
        public DiscMagnetic(char symbol) : base(symbol) { }


        public override void ApplyEffect(Disc[,] grid, int row, int col, GameInventory inventory)
        {
            int player = Symbol == 'M' ? 1 : 2;
            char ordinarySymbol = player == 1 ? '@' : '#';

            // Find the nearest ordinary disc of the same player (search upward from magnetic disc)
            int samePlayerRow = -1;
            for (int r = row - 1; r >= 0; r--)
            {
                Disc target = grid[r, col];
                if (target != null && target.Symbol == ordinarySymbol)
                {
                    samePlayerRow = r;
                    break;
                }
            }

            // Pull the player's disc up one position toward the magnetic disc
            if (samePlayerRow != -1)
            {
                int targetRow = samePlayerRow + 1;
                if (targetRow < grid.GetLength(0))
                {
                    // Save what's in the target position
                    Disc discInTarget = grid[targetRow, col];

                    // Move the player's disc up
                    Disc samePlayerDisc = grid[samePlayerRow, col];
                    grid[targetRow, col] = samePlayerDisc;

                    // If there was a disc in the target position, push it down (swap)
                    if (discInTarget != null)
                    {
                        grid[samePlayerRow, col] = discInTarget;
                    }
                    else
                    {
                        grid[samePlayerRow, col] = null;
                    }
                }
            }

            // Convert the magnetic disc to an ordinary disc at its original position
            grid[row, col] = new DiscOrdinary(ordinarySymbol);
        }

    }
}