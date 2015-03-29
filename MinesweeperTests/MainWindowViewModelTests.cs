using System.Runtime.InteropServices;
using FakeItEasy;
using Minesweeper.ViewModels;
using MinesweeperEngine;
using NUnit.Framework;

namespace MinesweeperTests
{
    [TestFixture]
    public class MainWindowViewModelTests
    {
        [Test]
        public void UncoveringMineShowsMessageBox()
        {
            var fakeMineLocationGenerator = A.Fake<IMineLocationGenerator>();
            var fakeDialogService = A.Fake<IDialogService>();
            var mainWindowViewModel = new MainWindowViewModel(fakeMineLocationGenerator, fakeDialogService);
            var mineCoords = new Coords(0, 0);
            var boardOptions = new BoardOptions {NumRows = 5, NumCols = 5, NumMines = 1};
            A.CallTo(() => fakeMineLocationGenerator.GenerateMineLocations(
                boardOptions.NumRows,
                boardOptions.NumCols,
                boardOptions.NumMines)).Returns(new[] { mineCoords });
            mainWindowViewModel.SetBoardOptions(boardOptions);
            mainWindowViewModel.UncoverSquare(mineCoords);
            A.CallTo(() => fakeDialogService.ShowMessageBox(A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeDialogService.ShowMessageBox("You lost!")).MustHaveHappened();
        }
    }
}
