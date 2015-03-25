using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Minesweeper.Mappers;
using MinesweeperEngine;

namespace Minesweeper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(int numRows, int numCols, int numMines, IMineLocationGenerator mineLocationGenerator, DialogService dialogService)
        {
            _numRows = numRows;
            _numCols = numCols;
            _numMines = numMines;
            _mineLocationGenerator = mineLocationGenerator;
            _board = null;
            _dialogService = dialogService;
        }

        public event EventHandler NewGame;
        public event EventHandler Exit;
        public event EventHandler YouWon;
        public event EventHandler YouLost;

        public void UncoverSquare(Coords coords)
        {
            _board.UncoverSquare(coords);
            RaisePropertyChangedForIndexer();
            CheckForEndOfGame();
        }

        public void FlagSquare(Coords coords)
        {
            _board.FlagSquare(coords);
            RaisePropertyChangedForIndexer();
            RaisePropertyChanged(() => UnflaggedMineCount);
            CheckForEndOfGame();
        }

        public SquareViewModel this[int row, int col]
        {
            get
            {
                var coords = new Coords(row, col);
                var squareData = _board[coords];
                return SquareDataMapper.MapSquareDataToSquareViewModel(squareData);
            }
        }

        public int UnflaggedMineCount
        {
            get { return _board.UnflaggedMineCount; }
        }

        public ICommand NewGameCommand
        {
            get { return _newGameCommand ?? (_newGameCommand = new RelayCommand(OnNewGame)); }
        }

        public ICommand ExitCommand
        {
            get { return _exitCommand ?? (_exitCommand = new RelayCommand(OnExit)); }
        }

        public ICommand UncoverSquareCommand
        {
            get { return _uncoverSquareCommand ?? (_uncoverSquareCommand = new RelayCommand<Coords>(OnUncoverSquare)); }
        }

        public ICommand FlagSquareCommand
        {
            get { return _flagSquareCommand ?? (_flagSquareCommand = new RelayCommand<Coords>(OnFlagSquare)); }
        }

        private void CheckForEndOfGame()
        {
            ConditionallyRaiseEvent(YouWon, _board.IsCleared, () =>
            {
                ShowMessageBox("You won!");
                OnNewGame();
            });

            ConditionallyRaiseEvent(YouLost, _board.IsDetonated, () =>
            {
                ShowMessageBox("You lost!");
                OnNewGame();
            });
        }

        private void ConditionallyRaiseEvent(EventHandler eventHandler, bool condition, Action action = null)
        {
            if (!condition) return;
            if (eventHandler != null) eventHandler.Invoke(this, EventArgs.Empty);
            if (action != null) action();
        }

        private void RaisePropertyChangedForIndexer()
        {
            var handler = PropertyChangedHandler;
            if (handler != null) handler(this, new PropertyChangedEventArgs(Binding.IndexerName));
        }

        // TODO: could also have an override with parameters object containing new numRows/numCols/numMines
        private void OnNewGame()
        {
            var mines = _mineLocationGenerator.GenerateMineLocations(_numRows, _numCols, _numMines);
            _board = Board.Create(_numRows, _numCols, mines);
            ConditionallyRaiseEvent(NewGame, true);
        }

        private void OnExit()
        {
            ConditionallyRaiseEvent(Exit, true);
        }

        private void OnUncoverSquare(Coords coords)
        {
            System.Diagnostics.Debug.WriteLine("OnUncoverSquare - ({0},{1})", coords.Row, coords.Col);
            UncoverSquare(coords);
        }

        private void OnFlagSquare(Coords coords)
        {
            System.Diagnostics.Debug.WriteLine("OnFlagSquare - ({0},{1})", coords.Row, coords.Col);
            FlagSquare(coords);
        }

        private void ShowMessageBox(string messageText)
        {
            _dialogService.ShowMessageBox(messageText);
        }

        private readonly int _numRows;
        private readonly int _numCols;
        private readonly int _numMines;
        private readonly IMineLocationGenerator _mineLocationGenerator;
        private Board _board;
        private readonly DialogService _dialogService;
        private RelayCommand _newGameCommand;
        private RelayCommand _exitCommand;
        private RelayCommand<Coords> _uncoverSquareCommand;
        private RelayCommand<Coords> _flagSquareCommand;
    }
}
