using System;

namespace ConnectFour
{
    
    //6*7 grid and customized grid
    public class Board
    {
        public int Rows { get; }
        public int Cols { get; }

        // 호출부가 (rows, cols)로 만든다면 반드시 이 생성자가 있어야 함
        public Board(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            // TODO: 내부 그리드 초기화는 나중에
        }

        // 호출부가 board.Print()를 부르면 시그니처만 맞춰서 임시 구현
        public void Print()
        {
            Console.WriteLine($"[DEBUG] Board {Rows}x{Cols} (printing stub)");
            // TODO: 실제 보드 렌더링은 나중에
        }
    }
}

