namespace Minesweeper.ViewModels
{
    public enum SquareState
    {
        Covered,
        UncoveredWithNoSurroundingMines,
        UncoveredWithSurroundingMines,
        Flagged,
        Exploded
    }
}
