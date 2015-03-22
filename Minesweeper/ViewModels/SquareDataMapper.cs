using System;
using MinesweeperEngine;

namespace Minesweeper.ViewModels
{
    public static class SquareDataMapper
    {
        public static SquareState MapSquareDataToSquareState(SquareData squareData)
        {
            var squareState = SquareState.Covered;

            if (squareData.IsUncovered)
            {
                if (squareData.IsMine)
                {
                    squareState = SquareState.Exploded;
                }
                else
                {
                    squareState = (squareData.NumNeighouringMines == 0)
                        ? SquareState.UncoveredWithNoSurroundingMines
                        : SquareState.UncoveredWithSurroundingMines;
                }
            }
            else if (squareData.IsFlagged)
            {
                squareState = SquareState.Flagged;
            }

            return squareState;
        }

        public static string MapSquareData(SquareData squareData)
        {
            var squareState = MapSquareDataToSquareState(squareData);

            switch (squareState)
            {
                case SquareState.Covered:
                    return string.Empty;

                case SquareState.UncoveredWithNoSurroundingMines:
                    return "-";

                case SquareState.UncoveredWithSurroundingMines:
                    return squareData.NumNeighouringMines.HasValue ? Convert.ToString(squareData.NumNeighouringMines.Value) : "?";

                case SquareState.Flagged:
                    return "F";

                case SquareState.Exploded:
                    return "Boom!";

                default:
                    return "?";
            }
        }

        // public static Tuple<SquareState, int> MapSquareData(SquareData squareData)
        // {
        //     var squareState = MapSquareDataToSquareState(squareData);
        //     var numSurroundingMines = squareData.NumNeighouringMines ?? -1;
        //     return Tuple.Create(squareState, numSurroundingMines);
        // }
    }
}
