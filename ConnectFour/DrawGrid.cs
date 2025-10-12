using ConnectFour;
using System.Numerics;

public class DrawGrid
{
    public int Rows { get; private set; }
    public int Columns { get; private set; }
    public Disc[,] Grid { get; private set; }

    private GameInventory inventory;

    public DrawGrid(int rows, int columns, GameInventory gameInventory)
    {
        Rows = rows;
        Columns = columns;
        Grid = new Disc[rows, columns];
        inventory = gameInventory;
    }

    //determines how far a disc will fall in a given column — i.e., the lowest available row.
    public int GetDropRow(int column)
    {
        if (column < 0 || column >= Columns)
            throw new ArgumentOutOfRangeException(nameof(column), "Column index out of bounds.");

        for (int row = 0; row < Rows; row++)
        {
            if (Grid[row, column] == null)
                return row;
        }

        return -1; // when Column is full when called in PlaceDisc method
    }


    //This method takes the disc symbol, player number, and column, then places the disc if possibl
    public int PlaceDisc(char symbol, int player, int column)
    {
        int dropRow = GetDropRow(column);

        if (dropRow == -1)
        {
            Console.WriteLine("Column is full.");
            return -1; // Indicates failure
        }

        // Check disc availability
        if (!inventory.IsDiscAvailable(player, symbol))
        {
            Console.WriteLine("No remaining discs of that type.");
            return -1;
        }

        // Create disc and place it
        Disc disc = Disc.CreateDiscFromSymbol(symbol);
        Grid[dropRow, column] = disc;

        // Decrement inventory
        string discType = Disc.GetDiscTypeFromSymbol(symbol);
        inventory.UseDisc(inventory.moveCounter, discType);

        //Applying effects for Magnetic, Exploding and Boring Discs. implements in their respective classes using polymorfism
        disc.ApplyEffect(Grid, dropRow, column, inventory);

        return dropRow; // Success: retur the row where the disc was placedd
    }


    public void DisplayGrid(int moveCounter)
    {
        Console.Clear();
        Console.WriteLine();

        string currentPlayer = moveCounter % 2 != 0 ? "Player 1" : "Player 2";
        string symbol = currentPlayer == "Player 1" ? inventory.PlayerOneName : inventory.PlayerTwoName;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Move #{moveCounter} — {currentPlayer}'s turn ({symbol})");
        Console.ResetColor();
        Console.WriteLine();

        for (int row = Rows - 1; row >= 0; row--)
        {
            Console.Write($"{row + 1,2} ");
            for (int col = 0; col < Columns; col++)
            {
                Disc disc = Grid[row, col];
                char cellSymbol;
                if (disc != null)
                {
                    cellSymbol = disc.Symbol;
                }
                else
                {
                    cellSymbol = ' ';
                }
                Console.Write($"| {cellSymbol} ");
            }
            Console.WriteLine("|");
        }

        Console.Write("   ");
        for (int col = 0; col < Columns; col++)
        {
            Console.Write($"  {col + 1} ");
        }

        Console.WriteLine(Environment.NewLine);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Disc Inventory:");
        Console.WriteLine($"Player 1 ({inventory.PlayerOneName}) → Ordinary: {inventory.PlayerOneOrdinaryDiscs}, Boring: {inventory.PlayerOneBoringDiscs}, Magnetic: {inventory.PlayerOneMagneticDiscs}, Explode: {inventory.PlayerOneExplodeDiscs}");
        Console.WriteLine($"Player 2 ({inventory.PlayerTwoName}) → Ordinary: {inventory.PlayerTwoOrdinaryDiscs}, Boring: {inventory.PlayerTwoBoringDiscs}, Magnetic: {inventory.PlayerTwoMagneticDiscs}, Explode: {inventory.PlayerTwoExplodeDiscs}");
        Console.ResetColor();

        Console.WriteLine();
    }

    public bool CheckWin(int row, int col)
    {
        Disc disc = Grid[row, col];
        if (disc == null) return false;

        char symbol = disc.Symbol;

        return CheckWinDirection(row, col, symbol, 0, 1)  
            || CheckWinDirection(row, col, symbol, 1, 0)   
            || CheckWinDirection(row, col, symbol, 1, 1)   
            || CheckWinDirection(row, col, symbol, 1, -1); 
    }

    private bool CheckWinDirection(int row, int col, char symbol, int rowDirection, int colDirection)
    {
        int count = 1;

        count += CountConsecutive(row, col, symbol, rowDirection, colDirection);
        count += CountConsecutive(row, col, symbol, -rowDirection, -colDirection);

        return count >= 4;
    }

    private int CountConsecutive(int row, int col, char symbol, int rowDelta, int colDelta)
    {
        int r = row + rowDelta;
        int c = col + colDelta;
        int count = 0;

        while (r >= 0 && r < Rows && c >= 0 && c < Columns)
        {
            Disc nextDisc = Grid[r, c];
            if (nextDisc == null || nextDisc.Symbol != symbol)
                break;

            count++;
            r += rowDelta;
            c += colDelta;
        }

        return count;
    }

    public bool CheckDraw()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                if (Grid[row, col] == null)
                    return false;
            }
        }
        return true;
    }

    public bool CheckWinStatus(int row, int col, out string winnerSymbol)
    {
        winnerSymbol = null;

        if (CheckWin(row, col))
        {
            Disc disc = Grid[row, col];
            winnerSymbol = disc != null ? disc.Symbol.ToString() : null;
            return true;
        }

        return false;
    }

    // Rotates the grid 90° clockwise
    public void RotateGridClockwise()
    {
        int newRows = Columns;
        int newCols = Rows;
        Disc[,] rotatedGrid = new Disc[newRows, newCols];

        // 90° clockwise rotation:
        // Position [r,c] moves to [Columns-1-c, r]
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                // Map old position to new position after 90° clockwise rotation
                int newRow = Columns - 1 - c;
                int newCol = r;
                rotatedGrid[newRow, newCol] = Grid[r, c];
            }
        }

        // Update grid dimensions and reference
        Grid = rotatedGrid;
        Rows = newRows;
        Columns = newCols;
    }

    // Reapplies gravity after rotation - all discs fall to the new bottom
    public void ApplyGravity()
    {
        for (int col = 0; col < Columns; col++)
        {
            int writeRow = 0; // Position to place the next falling disc

            // Collect all non-null discs in this column from bottom to top
            for (int row = 0; row < Rows; row++)
            {
                if (Grid[row, col] != null)
                {
                    // If disc is not already at the write position, move it down
                    if (row != writeRow)
                    {
                        Grid[writeRow, col] = Grid[row, col];
                        Grid[row, col] = null;
                    }
                    writeRow++;
                }
            }
        }
    }

    // Performs full rotation with gravity for LineUp Spin
    public void PerformRotation(int moveCounter)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        
        Console.WriteLine("ROTATION TRIGGERED! (After every 5th move)");
        Console.WriteLine("Grid rotating 90° clockwise...");
       
        Console.ResetColor();

        Console.WriteLine("BEFORE ROTATION:");
        DisplayGridWithoutClear(moveCounter);
        Console.WriteLine("\nPress any key to see the rotation...");
        Console.ReadKey();

        RotateGridClockwise();
        ApplyGravity();

        Console.ForegroundColor = ConsoleColor.Green;
        
        Console.WriteLine("ROTATION COMPLETE! Gravity reapplied.");
        
        Console.ResetColor();

        Console.WriteLine("AFTER ROTATION:");
        DisplayGridWithoutClear(moveCounter);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    // Helper method to display grid without clearing console
    private void DisplayGridWithoutClear(int moveCounter)
    {
        Console.WriteLine();

        string currentPlayer = moveCounter % 2 != 0 ? "Player 1" : "Player 2";
        string symbol = currentPlayer == "Player 1" ? inventory.PlayerOneName : inventory.PlayerTwoName;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Move #{moveCounter} — {currentPlayer}'s turn ({symbol})");
        Console.ResetColor();
        Console.WriteLine();

        for (int row = Rows - 1; row >= 0; row--)
        {
            Console.Write($"{row + 1,2} ");
            for (int col = 0; col < Columns; col++)
            {
                Disc disc = Grid[row, col];
                char cellSymbol;
                if (disc != null)
                {
                    cellSymbol = disc.Symbol;
                }
                else
                {
                    cellSymbol = ' ';
                }
                Console.Write($"| {cellSymbol} ");
            }
            Console.WriteLine("|");
        }

        Console.Write("   ");
        for (int col = 0; col < Columns; col++)
        {
            Console.Write($"  {col + 1} ");
        }

        Console.WriteLine(Environment.NewLine);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Disc Inventory:");
        Console.WriteLine($"Player 1 ({inventory.PlayerOneName}) → Ordinary: {inventory.PlayerOneOrdinaryDiscs}, Boring: {inventory.PlayerOneBoringDiscs}, Magnetic: {inventory.PlayerOneMagneticDiscs}");
        Console.WriteLine($"Player 2 ({inventory.PlayerTwoName}) → Ordinary: {inventory.PlayerTwoOrdinaryDiscs}, Boring: {inventory.PlayerTwoBoringDiscs}, Magnetic: {inventory.PlayerTwoMagneticDiscs}");
        Console.ResetColor();

        Console.WriteLine();
    }

}