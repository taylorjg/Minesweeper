using System;
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
        private const int NumMines = 10;

        public MainWindow()
        {
            InitializeComponent();
            NewGameMenuItem.Click += NewGameMenuItemOnClick;
            ExitMenuItem.Click += ExitMenuItemOnClick;
            StartNewGame();
        }

        public void StartNewGame()
        {
            var mines = new MineLocationGenerator().GenerateMineLocations(NumRows, NumCols, NumMines);
            var boardViewModel = new BoardViewModel(Board.Create(NumRows, NumCols, mines));
            boardViewModel.YouWon += OnYouWon;
            boardViewModel.YouLost += OnYouLost;
            DataContext = boardViewModel;
            InitialiseBoardGrid();
        }

        public void InitialiseBoardGrid()
        {
            BoardGrid.RowDefinitions.Clear();
            BoardGrid.ColumnDefinitions.Clear();
            BoardGrid.Children.Clear();

            // ReSharper disable UnusedVariable
            foreach (var _ in Enumerable.Range(0, NumRows)) BoardGrid.RowDefinitions.Add(new RowDefinition());
            foreach (var _ in Enumerable.Range(0, NumCols)) BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            // ReSharper restore UnusedVariable

            foreach (var row in Enumerable.Range(0, NumRows))
            {
                foreach (var col in Enumerable.Range(0, NumCols))
                {
                    var squareButton = new Button();
                    squareButton.SetValue(Grid.RowProperty, row);
                    squareButton.SetValue(Grid.ColumnProperty, col);
                    squareButton.Click += OnLeftClickSqusare;
                    squareButton.MouseRightButtonUp += OnRightClickSquare;
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

        private void OnLeftClickSqusare(object sender, RoutedEventArgs _)
        {
            var button = (Button) sender;
            var coords = (Coords) button.Tag;
            var boardViewModel = (BoardViewModel) DataContext;
            boardViewModel.UncoverSquare(coords);
        }

        private void OnRightClickSquare(object sender, MouseButtonEventArgs _)
        {
            var button = (Button) sender;
            var coords = (Coords) button.Tag;
            var boardViewModel = (BoardViewModel) DataContext;
            boardViewModel.FlagSquare(coords);
        }

        private void NewGameMenuItemOnClick(object _, RoutedEventArgs __)
        {
            StartNewGame();
        }

        private void ExitMenuItemOnClick(object _, RoutedEventArgs __)
        {
            Close();
        }

        private void OnYouWon(object sender, EventArgs eventArgs)
        {
            ShowMyMessageBox("You won!");
            StartNewGame();
        }

        private void OnYouLost(object sender, EventArgs eventArgs)
        {
            ShowMyMessageBox("Game over!");
            StartNewGame();
        }

        private void ShowMyMessageBox(string messageText)
        {
            var myMessageBox = new MyMessageBox { Owner = this, MessageText = messageText };
            myMessageBox.ShowDialog();
        }
    }
}
