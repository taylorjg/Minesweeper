using System;
using System.Collections.Generic;
using System.Linq;

namespace MinesweeperEngine
{
    public class Board
    {
        public static Board Create(int numRows, int numCols, IEnumerable<Coords> mines)
        {
            return new Board(numRows, numCols, mines);
        }

        private Board(int numRows, int numCols, IEnumerable<Coords> mines)
        {
            _numRows = numRows;
            _numCols = numCols;
            _squareData = mines.ToDictionary(coords => coords, coords => new SquareData {IsMine = true});
        }

        public void Uncover(Coords coords)
        {
            UncoverSquare(coords);
        }

        public void Flag(Coords coords)
        {
            FlagSquare(coords);
        }

        public bool IsCleared
        {
            get
            {
                Func<SquareData, bool> correctlyUncovered = sd => sd.IsUncovered && !sd.IsMine;
                Func<SquareData, bool> correctlyFlagged = sd => sd.IsFlagged && sd.IsMine;

                foreach (var row in Enumerable.Range(0, _numRows))
                {
                    foreach (var col in Enumerable.Range(0, _numCols))
                    {
                        SquareData squareData;
                        if (!_squareData.TryGetValue(new Coords(row, col), out squareData)) return false;
                        if (!correctlyUncovered(squareData) && !correctlyFlagged(squareData)) return false;
                    }
                }

                return true;
            }
        }

        public bool IsDetonated {
            get
            {
                foreach (var row in Enumerable.Range(0, _numRows))
                {
                    foreach (var col in Enumerable.Range(0, _numCols))
                    {
                        SquareData squareData;
                        if (_squareData.TryGetValue(new Coords(row, col), out squareData))
                        {
                            if (squareData.IsUncovered && squareData.IsMine) return true;
                        }
                    }
                }

                return false;
            }
        }

        private void UncoverNeighbours(Coords coords)
        {
            ForEachNeighbour(coords, neighbourCoords => UncoverSquare(neighbourCoords));
        }

        private void ForEachNeighbour(Coords coords, Action<Coords> action)
        {
            foreach (var neighbour in Neighbours(coords)) action(neighbour);
        }

        private IEnumerable<Coords> Neighbours(Coords coords)
        {
            return
                from row in Enumerable.Range(coords.Row - 1, 3)
                from col in Enumerable.Range(coords.Col - 1, 3)
                where row >= 0 && row < _numRows
                where col >= 0 && col < _numCols
                let neighbourCoords = new Coords(row, col)
                where !neighbourCoords.Equals(coords)
                select neighbourCoords;
        }

        private bool UncoverSquare(Coords coords)
        {
            return DoSquareDataFunc(coords, squareData =>
            {
                if (squareData.IsUncovered) return true;

                squareData.IsUncovered = true;

                if (squareData.NumNeighouringMines == null)
                {
                    var numNeighouringMines = 0;
                    ForEachNeighbour(coords, neighbourCoords => numNeighouringMines += IsMine(neighbourCoords) ? 1 : 0);
                    squareData.NumNeighouringMines = numNeighouringMines;
                }

                if (squareData.NumNeighouringMines.Value == 0)
                {
                    UncoverNeighbours(coords);
                }

                return false;
            });
        }

        private void FlagSquare(Coords coords)
        {
            DoSquareDataAction(coords, squareData => squareData.IsFlagged = true);
        }

        private void DoSquareDataAction(Coords coords, Action<SquareData> action)
        {
            DoSquareDataFunc(coords, squareData =>
            {
                action(squareData);
                return 0;
            });
        }

        private T DoSquareDataFunc<T>(Coords coords, Func<SquareData, T> func)
        {
            SquareData squareData;

            if (!_squareData.TryGetValue(coords, out squareData))
            {
                _squareData[coords] = squareData = new SquareData();
            }

            return func(squareData);
        }

        private bool IsMine(Coords coords)
        {
            SquareData squareData;
            return (_squareData.TryGetValue(coords, out squareData)) && squareData.IsMine;
        }

        private readonly int _numRows;
        private readonly int _numCols;
        private readonly IDictionary<Coords, SquareData> _squareData;
    }
}
