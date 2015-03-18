using System.Linq;
using MinesweeperEngine;
using NUnit.Framework;

namespace MinesweeperEngineTests
{
    [TestFixture]
    public class BoardTests
    {
        [Test]
        public void BoardIsInitiallyNotCleared()
        {
            var board = Board.Create(2, 2, Enumerable.Empty<Coords>());
            Assert.That(board.IsCleared, Is.False, "Expected board not to be cleared");
        }

        [Test]
        public void UncoveringSquareOnBoardWithOneSquareAndNoMinesIsCleared()
        {
            var board = Board.Create(1, 1, Enumerable.Empty<Coords>());
            var newBoard = board.Uncover(new Coords(0, 0));
            Assert.That(newBoard.IsCleared, Is.True, "Expected board to be cleared");
        }

        [Test]
        public void UncoveringSquareOnBoardWithTwoSquaresAndNoMinesIsCleared()
        {
            var board = Board.Create(1, 2, Enumerable.Empty<Coords>());
            var newBoard = board.Uncover(new Coords(0, 0));
            Assert.That(newBoard.IsCleared, Is.True, "Expected board to be cleared");
        }
    }
}
