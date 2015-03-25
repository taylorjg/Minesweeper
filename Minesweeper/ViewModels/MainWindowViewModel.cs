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
        public MainWindowViewModel(int numRows, int numCols, int numMines, IMineLocationGenerator mineLocationGenerator, IDialogService dialogService)
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

        public void StartNewGame()
        {
            var mines = _mineLocationGenerator.GenerateMineLocations(_numRows, _numCols, _numMines);
            _board = Board.Create(_numRows, _numCols, mines);
            ConditionallyRaiseEvent(NewGame, true);
        }

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
                _dialogService.ShowMessageBox(Properties.Resources.YouWonMessage);
                OnNewGame();
            });

            ConditionallyRaiseEvent(YouLost, _board.IsDetonated, () =>
            {
                _dialogService.ShowMessageBox(Properties.Resources.YouLostMessage);
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

        private void OnNewGame()
        {
            StartNewGame();
        }

        private void OnExit()
        {
            ConditionallyRaiseEvent(Exit, true);
        }

        private void OnUncoverSquare(Coords coords)
        {
            UncoverSquare(coords);
        }

        private void OnFlagSquare(Coords coords)
        {
            FlagSquare(coords);
        }

        private readonly int _numRows;
        private readonly int _numCols;
        private readonly int _numMines;
        private readonly IMineLocationGenerator _mineLocationGenerator;
        private readonly IDialogService _dialogService;
        private Board _board;
        private RelayCommand _newGameCommand;
        private RelayCommand _exitCommand;
        private RelayCommand<Coords> _uncoverSquareCommand;
        private RelayCommand<Coords> _flagSquareCommand;
    }
}
