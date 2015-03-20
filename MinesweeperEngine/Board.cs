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
            _squareData = new SquareData[numRows, numCols];

            foreach (var coords in AllCoords)
            {
                _squareData[coords.Row, coords.Col] = new SquareData{IsMine = mines.Contains(coords)};
            }
        }

        public int NumRows
        {
            get { return _squareData.GetLength(0); }
        }

        public int NumCols
        {
            get { return _squareData.GetLength(1); }
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
                Func<SquareData, bool> isIncomplete = sd => sd.IsMine == sd.IsUncovered;
                return !AllCoords.Any(coords => isIncomplete(CoordsToSquareData(coords)));
            }
        }

        public bool IsDetonated {
            get
            {
                Func<SquareData, bool> isDetonated = sd => sd.IsMine && sd.IsUncovered;
                return AllCoords.Any(coords => isDetonated(CoordsToSquareData(coords)));
            }
        }

        private void UncoverNeighbours(Coords coords)
        {
            ForEachNeighbour(coords, neighbourCoords => UncoverSquare(neighbourCoords));
        }

        private void ForEachNeighbour(Coords coords, Action<Coords> action)
        {
            foreach (var neighbourCoords in NeighbourCoords(coords)) action(neighbourCoords);
        }

        private IEnumerable<Coords> NeighbourCoords(Coords coords)
        {
            return
                from row in Enumerable.Range(coords.Row - 1, 3)
                from col in Enumerable.Range(coords.Col - 1, 3)
                where row >= 0 && row < NumRows
                where col >= 0 && col < NumCols
                let neighbourCoords = new Coords(row, col)
                where !neighbourCoords.Equals(coords)
                select neighbourCoords;
        }

        private IEnumerable<Coords> AllCoords
        {
            get
            {
                return
                    from row in Enumerable.Range(0, NumRows)
                    from col in Enumerable.Range(0, NumCols)
                    select new Coords(row, col);
            }
        }

        private bool UncoverSquare(Coords coords)
        {
            var squareData = CoordsToSquareData(coords);

            if (squareData.IsUncovered) return true;

            squareData.IsUncovered = true;

            if (squareData.NumNeighouringMines == null)
            {
                var numNeighouringMines = 0;
                ForEachNeighbour(coords, neighbourCoords => numNeighouringMines += IsMineAt(neighbourCoords) ? 1 : 0);
                squareData.NumNeighouringMines = numNeighouringMines;
            }

            if (squareData.NumNeighouringMines.Value == 0)
            {
                UncoverNeighbours(coords);
            }

            return false;
        }

        private void FlagSquare(Coords coords)
        {
            CoordsToSquareData(coords).IsFlagged = true;
        }

        private bool IsMineAt(Coords coords)
        {
            return CoordsToSquareData(coords).IsMine;
        }

        private SquareData CoordsToSquareData(Coords coords)
        {
            return _squareData[coords.Row, coords.Col];
        }

        private readonly SquareData[,] _squareData;
    }
}
