namespace MinesweeperEngine
{
    public class SquareData
    {
        public bool IsMine { get; internal set; }
        public bool IsUncovered { get; internal set; }
        public bool IsFlagged { get; internal set; }
        public int? NumNeighouringMines { get; internal set; }
    }
}
