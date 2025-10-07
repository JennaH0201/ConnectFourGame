using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    public class DiscOrdinary : Disc
    {
        public DiscOrdinary(char symbol) : base(symbol) { }


        public override void ApplyEffect(Disc[,] grid, int row, int col, GameInventory inventory)
        {
            // no effect
        }

    }
}