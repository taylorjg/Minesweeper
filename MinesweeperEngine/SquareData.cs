namespace MinesweeperEngine
{
    public class SquareData
    {
        public int? NumNeighouringMines { get; set; }
        public bool IsMine { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
    }
}
