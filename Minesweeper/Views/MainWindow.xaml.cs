using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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

            mainWindowViewModel.NewGame += OnNewGame;
            mainWindowViewModel.Exit += OnExit;

            DataContext = mainWindowViewModel;

            mainWindowViewModel.NewGameCommand.Execute(null);
        }

        private void InitialiseBoardGrid()
        {
            BoardGrid.RowDefinitions.Clear();
            BoardGrid.ColumnDefinitions.Clear();
            BoardGrid.Children.Clear();

            // ReSharper disable UnusedVariable
            foreach (var _ in Enumerable.Range(0, NumRows)) BoardGrid.RowDefinitions.Add(new RowDefinition());
            foreach (var _ in Enumerable.Range(0, NumCols)) BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            // ReSharper restore UnusedVariable

            var mainWindowViewModel = (MainWindowViewModel) DataContext;

            foreach (var row in Enumerable.Range(0, NumRows))
            {
                foreach (var col in Enumerable.Range(0, NumCols))
                {
                    var coords = new Coords(row, col);
                    var squareButton = new Button();
                    squareButton.SetValue(Grid.RowProperty, row);
                    squareButton.SetValue(Grid.ColumnProperty, col);
                    squareButton.Tag = coords;

                    squareButton.Command = mainWindowViewModel.UncoverSquareCommand;
                    squareButton.CommandParameter = coords;

                    squareButton.InputBindings.Add(new MouseBinding
                    {
                        Gesture = new MouseGesture(MouseAction.RightClick),
                        Command = mainWindowViewModel.FlagSquareCommand,
                        CommandParameter = coords
                    });

                    var contentPath = string.Format("[{0},{1}].DisplayText", row, col);
                    var contentBinding = new Binding(contentPath);
                    squareButton.SetBinding(ContentProperty, contentBinding);

                    var backgroundPath = string.Format("[{0},{1}].Color", row, col);
                    var backgroundBinding = new Binding(backgroundPath);
                    squareButton.SetBinding(BackgroundProperty, backgroundBinding);

                    BoardGrid.Children.Add(squareButton);
                }
            }
        }

        private void OnNewGame(object _, EventArgs __)
        {
            InitialiseBoardGrid();
        }

        private void OnExit(object _, EventArgs __)
        {
            Close();
        }
    }
}
