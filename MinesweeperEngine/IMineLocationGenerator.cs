using System.Collections.Generic;

namespace MinesweeperEngine
{
    public interface IMineLocationGenerator
    {
        IReadOnlyCollection<Coords> GenerateMineLocations(int numRows, int numCols, int numMines);
    }
}
