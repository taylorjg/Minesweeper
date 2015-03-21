﻿using MinesweeperEngine;
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
            board.Uncover(new Coords(0, 0));
            Assert.That(board.IsCleared, Is.True, "Expected board to be cleared");
            Assert.That(board.IsDetonated, Is.False, "Expected board not to be detonated");
        }

        [Test]
        public void UncoveringSquareOnBoardWithTwoSquaresAndNoMinesIsCleared()
        {
            var board = Board.Create(1, 2, new Coords[] {});
            board.Uncover(new Coords(0, 0));
            Assert.That(board.IsCleared, Is.True, "Expected board to be cleared");
            Assert.That(board.IsDetonated, Is.False, "Expected board not to be detonated");
        }

        [Test]
        public void UncoveringSquareOnBoardWithLotsOfSquaresAndNoMinesIsCleared()
        {
            var board = Board.Create(5, 5, new Coords[] {});
            board.Uncover(new Coords(0, 0));
            Assert.That(board.IsCleared, Is.True, "Expected board to be cleared");
            Assert.That(board.IsDetonated, Is.False, "Expected board not to be detonated");
        }

        [Test]
        public void UncoveringMineOnBoardWithOneSquareAndOneMinesIsDetonated()
        {
            var board = Board.Create(1, 1, new[] {new Coords(0, 0)});
            board.Uncover(new Coords(0, 0));
            Assert.That(board.IsDetonated, Is.True, "Expected board to be detonated");
        }

        [Test]
        public void BoardWithOneMineAndOneEmptySquareCanBeCleared()
        {
            var board = Board.Create(2, 1, new[] {new Coords(0, 0)});
            board.Flag(new Coords(0, 0));
            board.Uncover(new Coords(1, 0));
            Assert.That(board.IsCleared, Is.True, "Expected board to be cleared");
            Assert.That(board.IsDetonated, Is.False, "Expected board not to be detonated");
        }

        [Test]
        public void BoardShouldBeClearedWhenAllEmptySquaresHaveBeenUncovered()
        {
            var board = Board.Create(2, 1, new[] {new Coords(0, 0)});
            board.Uncover(new Coords(1, 0));
            Assert.That(board.IsCleared, Is.True, "Expected board to be cleared");
            Assert.That(board.IsDetonated, Is.False, "Expected board not to be detonated");
        }
    }
}
