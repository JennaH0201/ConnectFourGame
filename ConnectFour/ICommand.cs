namespace ConnectFour
{
    // Command pattern interface
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    // Command for placing a disc on the grid
    public class PlaceDiscCommand : ICommand
    {
        private DrawGrid grid;
        private GameInventory inventory;
        private char symbol;
        private int player;
        private int column;
        private int placedRow;

        public PlaceDiscCommand(DrawGrid grid, GameInventory inventory, char symbol, int player, int column)
        {
            this.grid = grid;
            this.inventory = inventory;
            this.symbol = symbol;
            this.player = player;
            this.column = column;
            this.placedRow = -1;
        }

        public void Execute()
        {
            placedRow = grid.PlaceDisc(symbol, player, column);
        }

        public void Undo()
        {
            if (placedRow != -1)
            {
                grid.UndoMove(inventory, player);
            }
        }

        public int GetPlacedRow()
        {
            return placedRow;
        }

        public char GetSymbol()
        {
            return symbol;
        }

        public int GetColumn()
        {
            return column;
        }
    }

    // Command for undoing a move
    public class UndoCommand : ICommand
    {
        private DrawGrid grid;
        private GameInventory inventory;
        private int player;
        private bool wasSuccessful;

        public UndoCommand(DrawGrid grid, GameInventory inventory, int player)
        {
            this.grid = grid;
            this.inventory = inventory;
            this.player = player;
            this.wasSuccessful = false;
        }

        public void Execute()
        {
            wasSuccessful = grid.UndoMove(inventory, player);
        }

        public void Undo()
        {
            if (wasSuccessful)
            {
                grid.RedoMove(inventory, player);
            }
        }

        public bool WasSuccessful()
        {
            return wasSuccessful;
        }
    }

    // Command for redoing a previously undone move
    public class RedoCommand : ICommand
    {
        private DrawGrid grid;
        private GameInventory inventory;
        private int player;
        private bool wasSuccessful;

        public RedoCommand(DrawGrid grid, GameInventory inventory, int player)
        {
            this.grid = grid;
            this.inventory = inventory;
            this.player = player;
            this.wasSuccessful = false;
        }

        public void Execute()
        {
            wasSuccessful = grid.RedoMove(inventory, player);
        }

        public void Undo()
        {
            if (wasSuccessful)
            {
                grid.UndoMove(inventory, player);
            }
        }

        public bool WasSuccessful()
        {
            return wasSuccessful;
        }
    }
}
