using System.Linq;
using System.Windows;
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

        public MainWindow()
        {
            InitializeComponent();
            var mines = new MineLocationGenerator().GenerateMineLocations(NumRows, NumCols, 10);
            DataContext = new BoardViewModel(Board.Create(NumRows, NumCols, mines));
            //Loaded += (_, __) => InitialiseBoardGrid();
            InitialiseBoardGrid();
            NewGameMenuItem.Click += NewGameMenuItemOnClick;
            ExitMenuItem.Click += ExitMenuItemOnClick;
        }

        private void NewGameMenuItemOnClick(object _, RoutedEventArgs __)
        {
        }

        private void ExitMenuItemOnClick(object _, RoutedEventArgs __)
        {
            Close();
        }

        public void InitialiseBoardGrid()
        {
            BoardGrid.RowDefinitions.Clear();
            BoardGrid.ColumnDefinitions.Clear();
            BoardGrid.Children.Clear();

            foreach (var _ in Enumerable.Range(0, NumRows)) BoardGrid.RowDefinitions.Add(new RowDefinition());
            foreach (var _ in Enumerable.Range(0, NumCols)) BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());

            foreach (var row in Enumerable.Range(0, NumRows))
            {
                foreach (var col in Enumerable.Range(0, NumCols))
                {
                    var squareButton = new Button();
                    squareButton.SetValue(Grid.RowProperty, row);
                    squareButton.SetValue(Grid.ColumnProperty, col);
                    squareButton.Click += SquareButtonOnClick;
                    squareButton.MouseRightButtonUp += SquareButtonOnMouseRightButtonUp;
                    squareButton.Tag = new Coords(row, col);
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

        private void SquareButtonOnClick(object sender, RoutedEventArgs _)
        {
            var button = (Button) sender;
            var coords = (Coords) button.Tag;
            var boardViewModel = (BoardViewModel) DataContext;
            boardViewModel.UncoverSquare(coords);
        }

        private void SquareButtonOnMouseRightButtonUp(object sender, MouseButtonEventArgs _)
        {
            var button = (Button) sender;
            var coords = (Coords) button.Tag;
            var boardViewModel = (BoardViewModel) DataContext;
            boardViewModel.FlagSquare(coords);
        }
    }
}
