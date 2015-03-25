using System;
using Minesweeper.ViewModels;
using MinesweeperEngine;

namespace Minesweeper.Mappers
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

        public static string MapSquareDataToString(SquareData squareData)
        {
            var squareState = MapSquareDataToSquareState(squareData);
            return MapSquareStateToString(squareState, squareData.NumNeighouringMines);
        }

        public static SquareViewModel MapSquareDataToSquareViewModel(SquareData squareData)
        {
            var squareState = MapSquareDataToSquareState(squareData);
            return new SquareViewModel(squareState, squareData.NumNeighouringMines ?? -1, squareData.IsMine, null);
        }

        public static SquareViewModel MapSquareDataToSquareViewModel(SquareData squareData, Coords coords)
        {
            var squareState = MapSquareDataToSquareState(squareData);
            return new SquareViewModel(squareState, squareData.NumNeighouringMines ?? -1, squareData.IsMine, coords);
        }

        public static string MapSquareStateToString(SquareState squareState, int? numNeighouringMines)
        {
            switch (squareState)
            {
                case SquareState.Covered:
                    return string.Empty;

                case SquareState.UncoveredWithNoSurroundingMines:
                    return "-";

                case SquareState.UncoveredWithSurroundingMines:
                    return numNeighouringMines.HasValue ? Convert.ToString(numNeighouringMines.Value) : "?";

                case SquareState.Flagged:
                    return "F";

                case SquareState.Exploded:
                    return "Boom!";

                default:
                    return "?";
            }
        }
    }
}
