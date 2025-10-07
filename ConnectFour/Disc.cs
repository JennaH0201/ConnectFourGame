using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    public class Disc
    {
        public char Symbol { get; set; }

        public Disc(char symbol)
        {
            Symbol = symbol;
        }

        public virtual void ApplyEffect(Disc[,] grid, int row, int col, GameInventory inventory)
        {
            // no effect
            // Overriden in other classes
        }


        public static Disc CreateDiscFromSymbol(char symbol)
        {
            return symbol switch
            {
                '@' => new DiscOrdinary('@'),
                '#' => new DiscOrdinary('#'),
                'B' => new DiscBoring('B'),
                'b' => new DiscBoring('b'),
                'M' => new DiscMagnetic('M'),
                'm' => new DiscMagnetic('m'),
                _ => throw new ArgumentException("Invalid disc symbol.")
            };
        }

        public static string GetDiscTypeFromSymbol(char symbol)
        {
            return symbol switch
            {
                '@' => "Ordinary",
                '#' => "Ordinary",
                'B' => "Boring",
                'b' => "Boring",
                'M' => "Magnetic",
                'm' => "Magnetic",
                _ => throw new ArgumentException("Invalid disc symbol.")
            };
        }


    }
}
