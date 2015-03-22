using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheckUtils;

namespace MinesweeperEngine
{
    public class MineLocationGenerator : IMineLocationGenerator
    {
        public IReadOnlyCollection<Coords> GenerateMineLocations(int numRows, int numCols, int numMines)
        {
            var allCoords =
                from row in Enumerable.Range(0, numRows)
                from col in Enumerable.Range(0, numCols)
                select new Coords(row, col);

            var generator = GenExtensions.PickValues(numMines, allCoords);

            return Gen.sample(0, 1, generator).Head;
        }
    }
}
