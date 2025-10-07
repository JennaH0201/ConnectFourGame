using System;
using System.Text.Json.Serialization;
using System.Threading;

namespace ConnectFour
{
    public class GameInventory
    {
        public string GameMode { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string PlayerOneName { get; set; }
        public string PlayerTwoName { get; set; }
        public int moveCounter { get; set; }


        // Disc allocation logic
        public int TotalCells => Rows * Columns;
        public int DiscsPerPlayer => (int)Math.Floor((Rows * Columns) / 2.0);


        // Player 1 disc counts
        [JsonInclude] public int PlayerOneOrdinaryDiscs { get; private set; }
        [JsonInclude] public int PlayerOneBoringDiscs { get; private set; } = 2;
        [JsonInclude] public int PlayerOneMagneticDiscs { get; private set; } = 2;

        // Player 2 disc counts
        [JsonInclude] public int PlayerTwoOrdinaryDiscs { get; private set; }
        [JsonInclude] public int PlayerTwoBoringDiscs { get; private set; } = 2;
        [JsonInclude] public int PlayerTwoMagneticDiscs { get; private set; } = 2;



        public bool IsDiscAvailable(int player, char symbol)
        {
            return symbol switch
            {
                '@' => PlayerOneOrdinaryDiscs > 0,
                '#' => PlayerTwoOrdinaryDiscs > 0,
                'B' => PlayerOneBoringDiscs > 0,
                'b' => PlayerTwoBoringDiscs > 0,
                'M' => PlayerOneMagneticDiscs > 0,
                'm' => PlayerTwoMagneticDiscs > 0,
                _ => false
            };
        }

        public void InitializeDiscInventory()
        {
            if (PlayerOneOrdinaryDiscs > 0 || PlayerTwoOrdinaryDiscs > 0)
                return; // Already initialized or restored

            int ordinary = DiscsPerPlayer - (PlayerOneBoringDiscs + PlayerOneMagneticDiscs);
            PlayerOneOrdinaryDiscs = ordinary;
            PlayerTwoOrdinaryDiscs = ordinary;
        }

        public bool UseDisc(int moveCounter, string type)
        {
            bool isPlayerOne = moveCounter % 2 != 0;

            switch (type.ToLower())
            {
                case "ordinary":
                    if (isPlayerOne && PlayerOneOrdinaryDiscs > 0)
                    {
                        PlayerOneOrdinaryDiscs--;
                        return true;
                    }
                    else if (!isPlayerOne && PlayerTwoOrdinaryDiscs > 0)
                    {
                        PlayerTwoOrdinaryDiscs--;
                        return true;
                    }
                    break;

                case "boring":
                    if (isPlayerOne && PlayerOneBoringDiscs > 0)
                    {
                        PlayerOneBoringDiscs--;
                        return true;
                    }
                    else if (!isPlayerOne && PlayerTwoBoringDiscs > 0)
                    {
                        PlayerTwoBoringDiscs--;
                        return true;
                    }
                    break;

                case "magnetic":
                    if (isPlayerOne && PlayerOneMagneticDiscs > 0)
                    {
                        PlayerOneMagneticDiscs--;
                        return true;
                    }
                    else if (!isPlayerOne && PlayerTwoMagneticDiscs > 0)
                    {
                        PlayerTwoMagneticDiscs--;
                        return true;
                    }
                    break;
            }

            return false; // Invalid disc type or no discs left
        }

        public void DisplaySummary()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Game Summary:");
            Console.WriteLine($"Mode: {GameMode}");
            Console.WriteLine($"Grid Size: {Rows} rows × {Columns} columns");
            Console.WriteLine($"Player 1: {PlayerOneName}");
            Console.WriteLine($"Player 2: {PlayerTwoName}");
            Console.WriteLine();

            Console.ResetColor();
            Thread.Sleep(1500); // Pause
        }

        public void DisplayDiscSummary()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine($"Each player receives {DiscsPerPlayer} discs:");
            Console.WriteLine($"Player 1 → Ordinary: {PlayerOneOrdinaryDiscs}, Boring: {PlayerOneBoringDiscs}, Magnetic: {PlayerOneMagneticDiscs}");
            Console.WriteLine($"Player 2 → Ordinary: {PlayerTwoOrdinaryDiscs}, Boring: {PlayerTwoBoringDiscs}, Magnetic: {PlayerTwoMagneticDiscs}");
            Console.WriteLine();

            Console.ResetColor();
        }

        public void RestoreDisc(int player, string type)
        {
            switch (type.ToLower())
            {
                case "ordinary":
                    if (player == 1) PlayerOneOrdinaryDiscs++;
                    else PlayerTwoOrdinaryDiscs++;
                    break;

                case "boring":
                    if (player == 1) PlayerOneBoringDiscs++;
                    else PlayerTwoBoringDiscs++;
                    break;

                case "magnetic":
                    if (player == 1) PlayerOneMagneticDiscs++;
                    else PlayerTwoMagneticDiscs++;
                    break;
            }
        }

    }
}