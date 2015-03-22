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
            DataContext = new BoardViewModel(Board.Create(NumRows, NumCols, new []{new Coords(0, 0)}));
            Loaded += (_, __) => InitialiseBoardGrid();
        }

        public void InitialiseBoardGrid()
        {
            BoardGrid.RowDefinitions.Clear();
            BoardGrid.ColumnDefinitions.Clear();
            BoardGrid.Children.Clear();

            foreach (var rowDefinition in Enumerable.Range(0, NumRows)
                .Select(_ => new RowDefinition {Height = new GridLength(0, GridUnitType.Auto)}))
            {
                BoardGrid.RowDefinitions.Add(rowDefinition);
            }

            foreach (var columnDefinition in Enumerable.Range(0, NumCols)
                .Select(_ => new ColumnDefinition {Width = new GridLength(0, GridUnitType.Auto)}))
            {
                BoardGrid.ColumnDefinitions.Add(columnDefinition);
            }

            var squareWidth = BoardGrid.ActualWidth / NumRows;
            var squareHeight = BoardGrid.ActualHeight / NumCols;

            foreach (var row in Enumerable.Range(0, NumRows))
            {
                foreach (var col in Enumerable.Range(0, NumCols))
                {
                    var squareButton = new Button { Width = squareWidth, Height = squareHeight };
                    squareButton.SetValue(Grid.RowProperty, row);
                    squareButton.SetValue(Grid.ColumnProperty, col);
                    squareButton.Click += SquareButtonOnClick;
                    squareButton.MouseRightButtonUp += SquareButtonOnMouseRightButtonUp;
                    squareButton.Tag = new Coords(row, col);
                    var path = string.Format("[{0},{1}]", row, col);
                    var binding = new Binding(path);
                    squareButton.SetBinding(ContentProperty, binding);
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
