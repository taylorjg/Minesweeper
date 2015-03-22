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
            var board = Board.Create(2, 2, new Coords[] {});
            Assert.That(board.IsCleared, Is.False, "Expected board not to be cleared");
        }

        [Test]
        public void BoardIsInitiallyNotDetonated()
        {
            var board = Board.Create(2, 2, new Coords[] {});
            Assert.That(board.IsDetonated, Is.False, "Expected board not to be detonated");
        }

        [Test]
        public void UncoveringSquareOnBoardWithOneSquareAndNoMinesIsCleared()
        {
            var board = Board.Create(1, 1, new Coords[] {});
            board.UncoverSquare(new Coords(0, 0));
            Assert.That(board.IsCleared, Is.True, "Expected board to be cleared");
            Assert.That(board.IsDetonated, Is.False, "Expected board not to be detonated");
        }

        [Test]
        public void UncoveringSquareOnBoardWithTwoSquaresAndNoMinesIsCleared()
        {
            var board = Board.Create(1, 2, new Coords[] {});
            board.UncoverSquare(new Coords(0, 0));
            Assert.That(board.IsCleared, Is.True, "Expected board to be cleared");
            Assert.That(board.IsDetonated, Is.False, "Expected board not to be detonated");
        }

        [Test]
        public void UncoveringSquareOnBoardWithLotsOfSquaresAndNoMinesIsCleared()
        {
            var board = Board.Create(5, 5, new Coords[] {});
            board.UncoverSquare(new Coords(0, 0));
            Assert.That(board.IsCleared, Is.True, "Expected board to be cleared");
            Assert.That(board.IsDetonated, Is.False, "Expected board not to be detonated");
        }

        [Test]
        public void UncoveringMineOnBoardWithOneSquareAndOneMinesIsDetonated()
        {
            var board = Board.Create(1, 1, new[] {new Coords(0, 0)});
            board.UncoverSquare(new Coords(0, 0));
            Assert.That(board.IsDetonated, Is.True, "Expected board to be detonated");
        }

        [Test]
        public void BoardWithOneMineAndOneEmptySquareCanBeCleared()
        {
            var board = Board.Create(2, 1, new[] {new Coords(0, 0)});
            board.FlagSquare(new Coords(0, 0));
            board.UncoverSquare(new Coords(1, 0));
            Assert.That(board.IsCleared, Is.True, "Expected board to be cleared");
            Assert.That(board.IsDetonated, Is.False, "Expected board not to be detonated");
        }

        [Test]
        public void BoardShouldBeClearedWhenAllEmptySquaresHaveBeenUncovered()
        {
            var board = Board.Create(2, 1, new[] {new Coords(0, 0)});
            board.UncoverSquare(new Coords(1, 0));
            Assert.That(board.IsCleared, Is.True, "Expected board to be cleared");
            Assert.That(board.IsDetonated, Is.False, "Expected board not to be detonated");
        }

        [Test]
        public void UncoveringAMineDoesNotUncoverAnythingElse()
        {
            var board = Board.Create(2, 2, new[] { new Coords(0, 0) });
            board.UncoverSquare(new Coords(0, 0));
            Assert.That(board[new Coords(0, 1)].IsUncovered, Is.False, "Expected other squares to still be covered");
            Assert.That(board[new Coords(1, 0)].IsUncovered, Is.False, "Expected other squares to still be covered");
            Assert.That(board[new Coords(1, 1)].IsUncovered, Is.False, "Expected other squares to still be covered");
            Assert.That(board[new Coords(0, 1)].NumNeighouringMines, Is.Null, "Expected other squares to have not calculated neighbouring mines");
            Assert.That(board[new Coords(1, 0)].NumNeighouringMines, Is.Null, "Expected other squares to have not calculated neighbouring mines");
            Assert.That(board[new Coords(1, 1)].NumNeighouringMines, Is.Null, "Expected other squares to have not calculated neighbouring mines");
        }

        [Test]
        public void CanUnflagASquare()
        {
            var coords = new Coords(0, 0);
            var board = Board.Create(1, 1, new[] { coords });
            board.FlagSquare(coords);
            Assert.That(board[coords].IsFlagged, Is.True, "Expected square to be flagged now");
            board.FlagSquare(coords);
            Assert.That(board[coords].IsFlagged, Is.False, "Expected square to be unflagged now");
        }

        [Test]
        public void CountOfUnflaggedMinesIsInitiallyEqualToTheNumberOfMines()
        {
            var board = Board.Create(2, 2, new[] { new Coords(0, 0) });
            Assert.That(board.UnflaggedMineCount, Is.EqualTo(1), "Incorrect UnflaggedMineCount");
        }

        [Test]
        public void CountOfUnflaggedMinesIsDecrementedAfterFlaggingASquare()
        {
            var board = Board.Create(2, 2, new[] { new Coords(0, 0) });
            Assert.That(board.UnflaggedMineCount, Is.EqualTo(1), "Incorrect UnflaggedMineCount");
            board.FlagSquare(new Coords(0, 0));
            Assert.That(board.UnflaggedMineCount, Is.EqualTo(0), "Incorrect UnflaggedMineCount");
        }

        [Test]
        public void CountOfUnflaggedMinesIsIncrementedAfterUnflaggingASquare()
        {
            var coords = new Coords(0, 0);
            var board = Board.Create(2, 2, new[] { coords });
            Assert.That(board.UnflaggedMineCount, Is.EqualTo(1), "Incorrect UnflaggedMineCount");
            board.FlagSquare(coords);
            Assert.That(board.UnflaggedMineCount, Is.EqualTo(0), "Incorrect UnflaggedMineCount");
            board.FlagSquare(coords);
            Assert.That(board.UnflaggedMineCount, Is.EqualTo(1), "Incorrect UnflaggedMineCount");
        }

        [Test]
        public void CountOfUnflaggedMinesCanGoNegative()
        {
            var board = Board.Create(2, 2, new[] { new Coords(0, 0) });
            Assert.That(board.UnflaggedMineCount, Is.EqualTo(1), "Incorrect UnflaggedMineCount");
            board.FlagSquare(new Coords(0, 0));
            Assert.That(board.UnflaggedMineCount, Is.EqualTo(0), "Incorrect UnflaggedMineCount");
            board.FlagSquare(new Coords(1, 0));
            Assert.That(board.UnflaggedMineCount, Is.EqualTo(-1), "Incorrect UnflaggedMineCount");
        }
    }
}
