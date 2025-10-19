namespace ConnectFour
{
    // Observer pattern interface
    public interface IGameObserver
    {
        void OnDiscPlaced(int player, char symbol, int row, int column);
        void OnMoveUndone(int player);
        void OnMoveRedone(int player);
        void OnGridRotated();
        void OnGameWon(int player);
    }

    // Concrete observer that logs game events to console
    public class ConsoleLogger : IGameObserver
    {
        public void OnDiscPlaced(int player, char symbol, int row, int column)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[LOG] Player {player} placed '{symbol}' at ({row}, {column})");
            Console.ResetColor();
        }

        public void OnMoveUndone(int player)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[LOG] Player {player} undid their move");
            Console.ResetColor();
        }

        public void OnMoveRedone(int player)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[LOG] Player {player} redid their move");
            Console.ResetColor();
        }

        public void OnGridRotated()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[LOG] Grid rotated 90° clockwise");
            Console.ResetColor();
        }

        public void OnGameWon(int player)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[LOG] Player {player} won the game!");
            Console.ResetColor();
        }
    }

    // Concrete observer that tracks game statistics
    public class StatisticsTracker : IGameObserver
    {
        private int totalMoves = 0;
        private int player1Moves = 0;
        private int player2Moves = 0;
        private int undoCount = 0;
        private int redoCount = 0;

        public void OnDiscPlaced(int player, char symbol, int row, int column)
        {
            totalMoves++;
            if (player == 1)
                player1Moves++;
            else
                player2Moves++;
        }

        public void OnMoveUndone(int player)
        {
            undoCount++;
        }

        public void OnMoveRedone(int player)
        {
            redoCount++;
        }

        public void OnGridRotated()
        {
            // Statistics tracking for rotation if needed
        }

        public void OnGameWon(int player)
        {
            // Could track win statistics
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("\n--- Game Statistics ---");
            Console.WriteLine($"Total Moves: {totalMoves}");
            Console.WriteLine($"Player 1 Moves: {player1Moves}");
            Console.WriteLine($"Player 2 Moves: {player2Moves}");
            Console.WriteLine($"Undo Count: {undoCount}");
            Console.WriteLine($"Redo Count: {redoCount}");
            Console.WriteLine("-----------------------\n");
        }
    }
}
