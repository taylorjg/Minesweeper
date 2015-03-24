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
    public class BoardViewModel : ViewModelBase
    {
        public BoardViewModel(Board board)
        {
            _board = board;
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

        private void CheckForEndOfGame()
        {
            ConditionallyRaiseEvent(YouWon, _board.IsCleared);
            ConditionallyRaiseEvent(YouLost, _board.IsDetonated);
        }

        private void ConditionallyRaiseEvent(EventHandler eventHandler, bool condition)
        {
            if (!condition || eventHandler == null) return;
            eventHandler.Invoke(this, EventArgs.Empty);
        }

        private void RaisePropertyChangedForIndexer()
        {
            var handler = PropertyChangedHandler;
            if (handler != null) handler(this, new PropertyChangedEventArgs(Binding.IndexerName));
        }

        private void OnNewGame()
        {
            ConditionallyRaiseEvent(NewGame, true);
        }

        private void OnExit()
        {
            ConditionallyRaiseEvent(Exit, true);
        }

        private readonly Board _board;
        private RelayCommand _newGameCommand;
        private RelayCommand _exitCommand;
    }
}
