using System.Numerics;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConnectFour
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "ConnectFour - IFN584 Edition";
            Console.ForegroundColor = ConsoleColor.Cyan;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("===============================================");
                Console.WriteLine("Welcome to ConnectFour!");
                Console.WriteLine("Developed by team 18 for IFN584");
                Console.WriteLine("Student: Philip, Jaeeun, Jennifer, and Hamza");
                Console.WriteLine("==============================================");

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine("Select Game Mode:");
                Console.WriteLine("1. Human vs Human");
                Console.WriteLine("2. Human vs Computer");
                Console.WriteLine("3. Restore Saved Game  [Auto-Save is ON]");
                Console.WriteLine("4. Test Mode");
                Console.WriteLine("5. Exit");

                int mode = InputValidation.GetValidatedInteger("\nEnter your choice (1-5): ", 1, 5);

                if (mode == 5)
                {
                    Console.WriteLine("Thanks for playing ConnectFour!");
                    break;
                }

                GameInventory gameInventory = new GameInventory();

                switch (mode)
                {
                    case 1:
                        gameInventory.GameMode = "Human vs Human";
                        break;
                    case 2:
                        gameInventory.GameMode = "Human vs Computer";
                        break;
                    case 3:
                        gameInventory.GameMode = "Restore Saved Game";
                        break;
                    case 4:
                        gameInventory.GameMode = "Test Mode";
                        break;
                }

                try
                {
                    if (mode == 3)
                    {
                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string filePath = Path.Combine(desktopPath, "ConnectFour_AutoSave.json");

                        if (!File.Exists(filePath))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Save file not found at: {filePath}");
                            Console.WriteLine("Please make sure a saved game exists before restoring.");
                            Console.ResetColor();
                            Console.WriteLine("Press any key to return to the main menu...");
                            Console.ReadKey();
                            continue;
                        }

                        string json = File.ReadAllText(filePath);
                        GameState loadedState = JsonSerializer.Deserialize<GameState>(json);

                        gameInventory = loadedState.Inventory;
                        DrawGrid grid = loadedState.RestoreGrid();

                        Console.WriteLine("Game successfully restored.");
                        grid.DisplayGrid(gameInventory.moveCounter);

                        // Continue gameplay from restored state
                        await RunGameLoop(gameInventory, grid);
                    }
                    else if (mode == 4)
                    {
                        Console.Write("Enter test sequence (e.g. O4,O5,O3,...): ");
                        string testInput = Console.ReadLine();

                        gameInventory.Rows = 10;
                        gameInventory.Columns = InputValidation.ComputeColumnsFromRows(gameInventory.Rows);
                        gameInventory.PlayerOneName = "@";
                        gameInventory.PlayerTwoName = "#";
                        gameInventory.InitializeDiscInventory();

                        PlayerTestRunner.RunTestSequence(testInput, gameInventory);
                        Console.WriteLine("Press any key to return to the main menu...");
                        Console.ReadKey();
                    }
                    else
                    {
                        // Select game variant for new games
                        Console.WriteLine(Environment.NewLine);
                        Console.WriteLine("Select Game Variant:");
                        Console.WriteLine("1. LineUp Classic");
                        Console.WriteLine("2. LineUp Basic");
                        Console.WriteLine("3. LineUp Spin (Grid rotates every 5 turns)");
                        int variant = InputValidation.GetValidatedInteger("\nEnter your choice (1-3): ", 1, 3);

                        gameInventory.GameVariant = variant switch
                        {
                            1 => "LineUp Classic",
                            2 => "LineUp Basic",
                            3 => "LineUp Spin",
                            _ => "LineUp Basic"
                        };

                        await RunGameLoop(gameInventory);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                }
            }
        }

        static async Task RunGameLoop(GameInventory gameInventory, DrawGrid restoredGrid = null)
        {
            DrawGrid grid;

            if (restoredGrid == null)
            {
                // New game setup
                Console.WriteLine("\nYou can select from 4 to 10 rows");
                Console.WriteLine("Columns will be automatically set based on rows\n");
                gameInventory.Rows = InputValidation.GetValidatedInteger("Enter number of rows: ", 4, 10);
                gameInventory.Columns = InputValidation.ComputeColumnsFromRows(gameInventory.Rows);
                gameInventory.PlayerOneName = "@";
                gameInventory.PlayerTwoName = "#";

                gameInventory.InitializeDiscInventory(); // Only for new games -  to solve disc count errors during restoration
                gameInventory.DisplaySummary();
                //gameInventory.DisplayDiscSummary();

                grid = new DrawGrid(gameInventory.Rows, gameInventory.Columns, gameInventory);
                gameInventory.moveCounter = 1;
            }
            else
            {
                // Restored game — inventory already populated
                grid = restoredGrid;
                Console.WriteLine("Game successfully restored.");

                //Display restored inventory
                gameInventory.DisplaySummary();
                gameInventory.DisplayDiscSummary();
            }

            grid.DisplayGrid(gameInventory.moveCounter);

            while (true)
            {
                try
                {
                    int player = gameInventory.moveCounter % 2 != 0 ? 1 : 2;
                    Disc disc;
                    int column;

                    if (gameInventory.GameMode == "Human vs Computer" && player == 2)
                    {
                        PlayerComputer computer = new PlayerComputer();
                        (disc, column) = computer.MakeMove(grid, gameInventory);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"[COMPUTER] Played {GetDiscTypeName(disc.Symbol)} disc '{disc.Symbol}' in column {column + 1}");
                        Console.ResetColor();
                        await Task.Delay(1000);
                    }
                    else
                    {
                        Console.Write("Enter move (e.g. o3, M4 or b7) or type 'help': ");

                        string input = Console.ReadLine().Trim().ToLower();

                        // show help menu
                        if (input == "help")
                        {
                            ShowHelp(gameInventory);
                            continue;
                        }

                        // go back to main menu
                        if (input == "menu")
                        {
                            break;
                        }

                        // quit game
                        if (input == "quit")
                        {
                            Console.WriteLine("Exiting the game...");
                            Environment.Exit(0);     // immediately close the program
                        }

                        //string input = Console.ReadLine();
                        (disc, column) = InputValidation.ParseInput(input, gameInventory.moveCounter, gameInventory, gameInventory.Columns);
                    }

                    int dropRow = grid.PlaceDisc(disc.Symbol, player, column);

                    if (dropRow != -1)
                    {
                        // Show what move was just made
                        if (gameInventory.GameMode != "Human vs Computer" || player == 1)
                        {
                            Console.ForegroundColor = player == 1 ? ConsoleColor.Yellow : ConsoleColor.Cyan;
                            Console.WriteLine($"[PLAYER {player}] Played {GetDiscTypeName(disc.Symbol)} disc '{disc.Symbol}' in column {column + 1}");
                            Console.ResetColor();
                        }

                        gameInventory.moveCounter++;

                        // Check if rotation should occur (every 5th turn for LineUp Spin)
                        if (gameInventory.GameVariant == "LineUp Spin" && gameInventory.moveCounter % 5 == 1 && gameInventory.moveCounter > 1)
                        {
                            grid.PerformRotation(gameInventory.moveCounter);

                            // Update GameInventory dimensions after rotation
                            gameInventory.Rows = grid.Rows;
                            gameInventory.Columns = grid.Columns;
                        }

                        grid.DisplayGrid(gameInventory.moveCounter);

                        GameState currentState = new GameState(gameInventory, grid);
                        string saveJson = JsonSerializer.Serialize(currentState, new JsonSerializerOptions { WriteIndented = true });
                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string filePath = Path.Combine(desktopPath, "ConnectFour_AutoSave.json");
                        File.WriteAllText(filePath, saveJson);
                        Console.WriteLine($"[Auto-Save is ON]");

                        if (grid.CheckWin(dropRow, column))
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            string winner = player == 1 ? "Player 1" : "Player 2";
                            Console.WriteLine($"**** {winner} wins the game! ****");
                            Console.ResetColor();
                            break;
                        }

                        if (grid.CheckDraw())
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("It's a draw! No more moves left.");
                            Console.ResetColor();
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Move not successful. Try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
        static string GetDiscTypeName(char symbol)
        {
            return symbol switch
            {
                '@' or '#' => "Ordinary",
                'b' or 'B' => "Boring",
                'M' or 'm' => "Magnetic",
                'E' or 'e' => "Exploding",
                _ => "Unknown"
            };
        }

        // HELP MENU
        private static void ShowHelp(GameInventory gameInventory)
        {
            Console.WriteLine("\nAvailable commands:");
            Console.WriteLine($"  o1-{gameInventory.Columns} : Drop ordinary disc");
            Console.WriteLine($"  b1-{gameInventory.Columns} : Drop Boring disc (clears column)");
            Console.WriteLine($"  m1-{gameInventory.Columns} : Drop Magnetic disc (lifts nearest disc belonging to he player up 1 position and converts to ordinary if lifted)");
            Console.WriteLine($"  e1-{gameInventory.Columns} : Drop Exploding disc (destroys surrounding discs and self)");
            Console.WriteLine("  save : auto-save is ON");
            Console.WriteLine("  menu : Return to main menu");
            Console.WriteLine("  quit : Exit game\n");
        }
    }
}
