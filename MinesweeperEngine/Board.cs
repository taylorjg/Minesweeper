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
            UncoverNeighbours(coords);
        }

        public bool IsCleared {
            get
            {
                foreach (var row in Enumerable.Range(0, _numRows))
                {
                    foreach (var col in Enumerable.Range(0, _numCols))
                    {
                        SquareData squareData;
                        if (!_squareData.TryGetValue(new Coords(row, col), out squareData)) return false;
                        if (!squareData.IsRevealed) return false;
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
                            if (squareData.IsRevealed && squareData.IsMine) return true;
                        }
                    }
                }

                return false;
            }
        }

        private void UncoverNeighbours(Coords coords)
        {
            ForEachNeighbour(coords, neighbourCoords =>
            {
                var wasUncovered = UncoverSquare(neighbourCoords);
                if (!wasUncovered) UncoverNeighbours(neighbourCoords);
            });
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
            SquareData squareData;

            if (_squareData.TryGetValue(coords, out squareData))
            {
                if (squareData.IsRevealed) return true;
                squareData.IsRevealed = true;
            }
            else
            {
                _squareData[coords] = new SquareData { IsRevealed = true };
            }

            return false;
        }

        private readonly int _numRows;
        private readonly int _numCols;
        private readonly IDictionary<Coords, SquareData> _squareData;
    }
}
