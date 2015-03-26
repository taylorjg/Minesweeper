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
            var mainWindowViewModel = new MainWindowViewModel(5, 5, 1, fakeMineLocationGenerator, fakeDialogService);
            var mineCoords = new Coords(0, 0);
            A.CallTo(() => fakeMineLocationGenerator.GenerateMineLocations(5, 5, 1)).Returns(new[] { mineCoords });
            mainWindowViewModel.StartNewGame();
            mainWindowViewModel.UncoverSquare(mineCoords);
            A.CallTo(() => fakeDialogService.ShowMessageBox(A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeDialogService.ShowMessageBox("You lost!")).MustHaveHappened();
        }
    }
}
