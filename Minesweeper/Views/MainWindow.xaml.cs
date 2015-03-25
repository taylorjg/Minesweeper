using System;
using Minesweeper.ViewModels;
using MinesweeperEngine;

namespace Minesweeper.Views
{
    public partial class MainWindow
    {
        private const int NumRows = 10;
        private const int NumCols = 10;
        private const int NumMines = 10;

        public MainWindow()
        {
            InitializeComponent();

            var mainWindowViewModel = new MainWindowViewModel(
                NumRows,
                NumCols,
                NumMines,
                new MineLocationGenerator(),
                new DialogService(this));

            mainWindowViewModel.Exit += OnExit;

            DataContext = mainWindowViewModel;

            mainWindowViewModel.NewGameCommand.Execute(null);
        }

        private void OnExit(object _, EventArgs __)
        {
            Close();
        }
    }
}
