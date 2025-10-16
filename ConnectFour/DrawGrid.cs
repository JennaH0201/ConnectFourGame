using ConnectFour;
using System.Numerics;
using System.Collections.Generic;

public class DrawGrid
{
    public int Rows { get; private set; }
    public int Columns { get; private set; }
    public Disc[,] Grid { get; private set; }

    private GameInventory inventory;
    private List<Move> moveHistory;
    private int currentMoveIndex;
    private List<int> undoneMoves;

    public DrawGrid(int rows, int columns, GameInventory gameInventory)
    {
        Rows = rows;
        Columns = columns;
        Grid = new Disc[rows, columns];
        inventory = gameInventory;
        moveHistory = new List<Move>();
        currentMoveIndex = -1;
        undoneMoves = new List<int>();
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


    public int PlaceDisc(char symbol, int player, int column)
    {
        int dropRow = GetDropRow(column);

        if (dropRow == -1)
        {
            Console.WriteLine("Column is full.");
            return -1;
        }

        if (!inventory.IsDiscAvailable(player, symbol))
        {
            Console.WriteLine("No remaining discs of that type.");
            return -1;
        }

        // Create appropriate move type based on disc type
        string discType = Disc.GetDiscTypeFromSymbol(symbol);
        Move move;

        if (discType == "Ordinary")
        {
            move = new MoveOrdinary
            {
                Row = dropRow,
                Column = column,
                Symbol = symbol,
                DiscType = discType,
                MoveNumber = inventory.moveCounter
            };
        }
        else
        {
            move = new MoveWithEffect
            {
                Row = dropRow,
                Column = column,
                Symbol = symbol,
                DiscType = discType,
                MoveNumber = inventory.moveCounter
            };
        }

        // Save grid state before applying special disc effects
        CaptureGridBeforeEffect(move, dropRow, column);

        Disc disc = Disc.CreateDiscFromSymbol(symbol);
        Grid[dropRow, column] = disc;

        inventory.UseDisc(inventory.moveCounter, discType);

        disc.ApplyEffect(Grid, dropRow, column, inventory);

        // Compare grid before and after to track removed cells
        CaptureGridAfterEffect(move, dropRow, column);
        RecordMove(move);

        return dropRow;
    }

    // Temporary grid to compare before and after special disc effects
    private Disc[,] tempGrid;

    // Save current grid state before applying disc effect
    private void CaptureGridBeforeEffect(Move move, int placedRow, int placedCol)
    {
        tempGrid = new Disc[Rows, Columns];
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                tempGrid[r, c] = Grid[r, c];
            }
        }
    }

    // Compare grid before and after to find removed or changed cells
    private void CaptureGridAfterEffect(Move move, int placedRow, int placedCol)
    {
        if (move is MoveWithEffect moveWithEffect)
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    if (r == placedRow && c == placedCol)
                        continue;

                    // Cell was removed
                    if (tempGrid[r, c] != null && Grid[r, c] == null)
                    {
                        SavedCell saved = new SavedCell
                        {
                            Row = r,
                            Column = c,
                            Symbol = tempGrid[r, c].Symbol,
                            DiscType = Disc.GetDiscTypeFromSymbol(tempGrid[r, c].Symbol)
                        };
                        moveWithEffect.RemovedCells.Add(saved);
                    }
                    // Cell was changed
                    else if (tempGrid[r, c] != null && Grid[r, c] != null &&
                             tempGrid[r, c].Symbol != Grid[r, c].Symbol)
                    {
                        SavedCell saved = new SavedCell
                        {
                            Row = r,
                            Column = c,
                            Symbol = tempGrid[r, c].Symbol,
                            DiscType = Disc.GetDiscTypeFromSymbol(tempGrid[r, c].Symbol)
                        };
                        moveWithEffect.RemovedCells.Add(saved);
                    }
                }
            }
        }
    }

    // Add move to history and clear any redo moves
    private void RecordMove(Move move)
    {
        if (currentMoveIndex < moveHistory.Count - 1)
        {
            moveHistory.RemoveRange(currentMoveIndex + 1, moveHistory.Count - currentMoveIndex - 1);
        }
        moveHistory.Add(move);
        currentMoveIndex++;
        undoneMoves.Clear();
    }

    // Get player number from disc symbol
    private int GetPlayerFromSymbol(char symbol)
    {
        return symbol switch
        {
            '@' or 'b' or 'M' or 'E' => 1,
            '#' or 'B' or 'm' or 'e' => 2,
            _ => 0
        };
    }

    // Check if player has moves to undo
    public bool CanUndo(int player)
    {
        for (int i = currentMoveIndex; i >= 0; i--)
        {
            if (undoneMoves.Contains(moveHistory[i].MoveNumber))
                continue;

            if (GetPlayerFromSymbol(moveHistory[i].Symbol) == player)
                return true;
        }
        return false;
    }

    // Check if player has moves to redo
    public bool CanRedo(int player)
    {
        if (undoneMoves.Count == 0) return false;

        foreach (int moveNum in undoneMoves)
        {
            Move m = moveHistory.Find(mv => mv.MoveNumber == moveNum);
            if (m != null && GetPlayerFromSymbol(m.Symbol) == player)
                return true;
        }
        return false;
    }

    // Undo the player's last move
    public bool UndoMove(GameInventory inventory, int player)
    {
        for (int i = currentMoveIndex; i >= 0; i--)
        {
            Move move = moveHistory[i];

            if (undoneMoves.Contains(move.MoveNumber))
                continue;

            if (GetPlayerFromSymbol(move.Symbol) == player)
            {
                // Remove the disc placed by this move
                Grid[move.Row, move.Column] = null;

                // Restore any cells removed by special disc effects
                move.RestoreRemovedCells(Grid, inventory);

                // Return the disc to player's inventory
                inventory.RestoreDisc(player, move.DiscType);
                undoneMoves.Add(move.MoveNumber);
                return true;
            }
        }
        return false;
    }

    // Redo a previously undone move
    public bool RedoMove(GameInventory inventory, int player)
    {
        foreach (int moveNum in undoneMoves.ToList())
        {
            Move move = moveHistory.Find(m => m.MoveNumber == moveNum);
            if (move != null && GetPlayerFromSymbol(move.Symbol) == player)
            {
                // Place the disc again
                Disc disc = Disc.CreateDiscFromSymbol(move.Symbol);
                Grid[move.Row, move.Column] = disc;

                inventory.UseDisc(move.MoveNumber, move.DiscType);

                // Reapply special disc effects
                disc.ApplyEffect(Grid, move.Row, move.Column, inventory);

                undoneMoves.Remove(moveNum);
                return true;
            }
        }
        return false;
    }

    public List<Move> GetMoveHistory() => moveHistory;
    public int GetCurrentMoveIndex() => currentMoveIndex;
    public void RestoreMoveHistory(List<Move> history, int index, List<int> undone)
    {
        moveHistory = history;
        currentMoveIndex = index;
        undoneMoves = undone;
    }


    public void DisplayGrid(int moveCounter)
    {
        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════════");

        string currentPlayer = moveCounter % 2 != 0 ? "Player 1" : "Player 2";
        string symbol = currentPlayer == "Player 1" ? inventory.PlayerOneName : inventory.PlayerTwoName;
        int playerNum = moveCounter % 2 != 0 ? 1 : 2;

        Console.ForegroundColor = playerNum == 1 ? ConsoleColor.Yellow : ConsoleColor.Cyan;
        Console.WriteLine($"MOVE #{moveCounter} >>> {currentPlayer.ToUpper()}'S TURN (Symbol: {symbol})");
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

        if (inventory.GameVariant == "LineUp Basic" || inventory.GameVariant == "LineUp Spin")
        {
            // LineUp Basic and Spin: Only show ordinary discs
            Console.WriteLine($"Player 1 ({inventory.PlayerOneName}) → Ordinary: {inventory.PlayerOneOrdinaryDiscs}");
            Console.WriteLine($"Player 2 ({inventory.PlayerTwoName}) → Ordinary: {inventory.PlayerTwoOrdinaryDiscs}");
        }
        else
        {
            // LineUp Classic: Show all disc types
            Console.WriteLine($"Player 1 ({inventory.PlayerOneName}) → Ordinary: {inventory.PlayerOneOrdinaryDiscs}, Boring: {inventory.PlayerOneBoringDiscs}, Magnetic: {inventory.PlayerOneMagneticDiscs}, Explode: {inventory.PlayerOneExplodeDiscs}");
            Console.WriteLine($"Player 2 ({inventory.PlayerTwoName}) → Ordinary: {inventory.PlayerTwoOrdinaryDiscs}, Boring: {inventory.PlayerTwoBoringDiscs}, Magnetic: {inventory.PlayerTwoMagneticDiscs}, Explode: {inventory.PlayerTwoExplodeDiscs}");
        }

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

        if (inventory.GameVariant == "LineUp Basic" || inventory.GameVariant == "LineUp Spin")
        {
            // LineUp Basic and Spin: Only show ordinary discs
            Console.WriteLine($"Player 1 ({inventory.PlayerOneName}) → Ordinary: {inventory.PlayerOneOrdinaryDiscs}");
            Console.WriteLine($"Player 2 ({inventory.PlayerTwoName}) → Ordinary: {inventory.PlayerTwoOrdinaryDiscs}");
        }
        else
        {
            // LineUp Classic: Show all disc types
            Console.WriteLine($"Player 1 ({inventory.PlayerOneName}) → Ordinary: {inventory.PlayerOneOrdinaryDiscs}, Boring: {inventory.PlayerOneBoringDiscs}, Magnetic: {inventory.PlayerOneMagneticDiscs}, Explode: {inventory.PlayerOneExplodeDiscs}");
            Console.WriteLine($"Player 2 ({inventory.PlayerTwoName}) → Ordinary: {inventory.PlayerTwoOrdinaryDiscs}, Boring: {inventory.PlayerTwoBoringDiscs}, Magnetic: {inventory.PlayerTwoMagneticDiscs}, Explode: {inventory.PlayerTwoExplodeDiscs}");
        }

        Console.ResetColor();

        Console.WriteLine();
    }

}