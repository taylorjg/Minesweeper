using System;
using System.Collections.Generic;
using System.Linq;

namespace MinesweeperEngine
{
    public class Board
    {
        public static Board Create(int numRows, int numCols, IReadOnlyCollection<Coords> mines)
        {
            return new Board(numRows, numCols, mines);
        }

        private Board(int numRows, int numCols, IReadOnlyCollection<Coords> mines)
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

        public void UncoverSquare(Coords coords)
        {
            UncoverSquareInternal(coords);
        }

        public void FlagSquare(Coords coords)
        {
            FlagSquareInternal(coords);
        }

        public bool IsCleared
        {
            get
            {
                Func<SquareData, bool> isBad = sd => sd.IsMine == sd.IsUncovered;
                return !AllCoords.Any(coords => isBad(CoordsToSquareData(coords)));
            }
        }

        public bool IsDetonated {
            get
            {
                Func<SquareData, bool> isDetonated = sd => sd.IsMine && sd.IsUncovered;
                return AllCoords.Any(coords => isDetonated(CoordsToSquareData(coords)));
            }
        }

        public SquareData this[Coords coords]
        {
            get { return CoordsToSquareData(coords); }
        }

        public int UnflaggedMineCount {
            get
            {
                var numTotalMines = AllCoords.Sum(coords => CoordsToSquareData(coords).IsMine ? 1 : 0);
                var numFlaggedSquares = AllCoords.Sum(coords => CoordsToSquareData(coords).IsFlagged ? 1 : 0);
                return numTotalMines - numFlaggedSquares;
            }
        }

        private void UncoverNeighbours(Coords coords)
        {
            if (NumNeighouringMines(coords) > 0) return;
            ForEachNeighbour(coords, UncoverSquareInternal);
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

        private void UncoverSquareInternal(Coords coords)
        {
            var squareData = CoordsToSquareData(coords);
            if (squareData.IsUncovered) return;
            squareData.IsUncovered = true;
            if (squareData.IsMine) return;
            UncoverNeighbours(coords);
        }

        private void FlagSquareInternal(Coords coords)
        {
            var squareData = CoordsToSquareData(coords);
            squareData.IsFlagged = !squareData.IsFlagged;
        }

        private bool IsMineAt(Coords coords)
        {
            return CoordsToSquareData(coords).IsMine;
        }

        private int NumNeighouringMines(Coords coords)
        {
            var squareData = CoordsToSquareData(coords);

            if (squareData.NumNeighouringMines == null)
            {
                Func<Coords, int> f = c => IsMineAt(c) ? 1 : 0;
                squareData.NumNeighouringMines = NeighbourCoords(coords).Sum(f);
            }

            return squareData.NumNeighouringMines.Value;
        }

        private SquareData CoordsToSquareData(Coords coords)
        {
            return _squareData[coords.Row, coords.Col];
        }

        private readonly SquareData[,] _squareData;
    }
}
