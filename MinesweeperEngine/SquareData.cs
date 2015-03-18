namespace MinesweeperEngine
{
    public class SquareData
    {
        public bool IsMine { get; set; }
        public bool IsUncovered { get; set; }
        public bool IsFlagged { get; set; }
        public int? NumNeighouringMines { get; set; }
    }
}
