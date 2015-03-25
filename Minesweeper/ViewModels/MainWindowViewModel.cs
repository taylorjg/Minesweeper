using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Minesweeper.Mappers;
using Minesweeper.Properties;
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
            var mines = _mineLocationGenerator.GenerateMineLocations(NumRows, NumCols, _numMines);
            _board = Board.Create(NumRows, NumCols, mines);
            RaisePropertyChanged(() => Squares);
            ConditionallyRaiseEvent(NewGame, true);
        }

        public void UncoverSquare(Coords coords)
        {
            _board.UncoverSquare(coords);
            RaisePropertyChanged(() => Squares);
            CheckForEndOfGame();
        }

        public void FlagSquare(Coords coords)
        {
            _board.FlagSquare(coords);
            RaisePropertyChanged(() => Squares);
            RaisePropertyChanged(() => UnflaggedMineCount);
            CheckForEndOfGame();
        }

        public IEnumerable<SquareViewModel> Squares
        {
            get
            {
                var allCoords =
                    from row in Enumerable.Range(0, NumRows)
                    from col in Enumerable.Range(0, NumCols)
                    select new Coords(row, col);

                return allCoords.Select(coords =>
                {
                    var squareData = _board[coords];
                    return SquareDataMapper.MapSquareDataToSquareViewModel(squareData, coords);
                });
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

        public int NumRows
        {
            get { return _numRows; }
        }

        public int NumCols
        {
            get { return _numCols; }
        }

        private void CheckForEndOfGame()
        {
            ConditionallyRaiseEvent(YouWon, _board.IsCleared, () =>
            {
                _dialogService.ShowMessageBox(Resources.YouWonMessage);
                OnNewGame();
            });

            ConditionallyRaiseEvent(YouLost, _board.IsDetonated, () =>
            {
                _dialogService.ShowMessageBox(Resources.YouLostMessage);
                OnNewGame();
            });
        }

        private void ConditionallyRaiseEvent(EventHandler eventHandler, bool condition, Action action = null)
        {
            if (!condition) return;
            if (eventHandler != null) eventHandler.Invoke(this, EventArgs.Empty);
            if (action != null) action();
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
