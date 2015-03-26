using Minesweeper.ViewModels;
using MinesweeperEngine;

namespace Minesweeper.Views
{
    public partial class MainWindow
    {
        private const int NumRows = 9;
        private const int NumCols = 9;
        private const int NumMines = 10;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(
                NumRows,
                NumCols,
                NumMines,
                new MineLocationGenerator(),
                new DialogService(this));
        }
    }
}
