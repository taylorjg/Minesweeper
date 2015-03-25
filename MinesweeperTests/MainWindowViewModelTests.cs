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
        public void UncoveringMineRaisesYouLostEvent()
        {
            var fakeMineLocationGenerator = A.Fake<IMineLocationGenerator>();
            var fakeDialogService = A.Fake<IDialogService>();
            var mainWindowViewModel = new MainWindowViewModel(5, 5, 1, fakeMineLocationGenerator, fakeDialogService);
            var mineCoords = new Coords(0, 0);
            var eventRaised = false;
            A.CallTo(() => fakeMineLocationGenerator.GenerateMineLocations(5, 5, 1)).Returns(new[]{mineCoords});
            mainWindowViewModel.YouLost += (_, __) => { eventRaised = true; };
            mainWindowViewModel.StartNewGame();
            mainWindowViewModel.UncoverSquare(mineCoords);
            Assert.That(eventRaised, Is.True, "Expected the YouLost event to have been raised");
        }
    }
}
