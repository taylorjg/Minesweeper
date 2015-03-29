using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
        public MainWindowViewModel(IMineLocationGenerator mineLocationGenerator, IDialogService dialogService)
        {

            _mineLocationGenerator = mineLocationGenerator;
            _dialogService = dialogService;
            _board = null;
        }

        public void SetBoardOptions(BoardOptions boardOptions)
        {
            _numRows = boardOptions.NumRows;
            _numCols = boardOptions.NumCols;
            _numMines = boardOptions.NumMines;
            RaisePropertyChanged(() => NumRows);
            RaisePropertyChanged(() => NumCols);
            RaisePropertyChanged(() => UnflaggedMineCount);
            StartNewGame();
        }

        public void StartNewGame()
        {
            var mines = _mineLocationGenerator.GenerateMineLocations(_numRows, _numCols, _numMines);
            _board = Board.Create(_numRows, _numCols, mines);
            RaisePropertyChanged(() => Squares);
            RaisePropertyChanged(() => UnflaggedMineCount);
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
                if (_board == null) return Enumerable.Empty<SquareViewModel>();

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
            get
            {
                return _board == null ? 0 : _board.UnflaggedMineCount;
            }
        }

        public ICommand NewGameCommand
        {
            get { return _newGameCommand ?? (_newGameCommand = new RelayCommand(OnNewGame)); }
        }

        public ICommand SetBoardOptionsCommand
        {
            get { return _setBoardOptionsCommand ?? (_setBoardOptionsCommand = new RelayCommand<BoardOptions>(OnSetBoardOptionsCommand)); }
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
            if (_board.IsCleared)
            {
                _dialogService.ShowMessageBox(Resources.YouWonMessage);
                OnNewGame();
            }

            if (_board.IsDetonated)
            {
                _dialogService.ShowMessageBox(Resources.YouLostMessage);
                OnNewGame();
            }
        }

        private void OnNewGame()
        {
            StartNewGame();
        }

        private void OnSetBoardOptionsCommand(BoardOptions boardOptions)
        {
            SetBoardOptions(boardOptions);
        }

        private static void OnExit()
        {
            Application.Current.MainWindow.Close();
        }

        private void OnUncoverSquare(Coords coords)
        {
            UncoverSquare(coords);
        }

        private void OnFlagSquare(Coords coords)
        {
            FlagSquare(coords);
        }

        private int _numRows;
        private int _numCols;
        private int _numMines;
        private readonly IMineLocationGenerator _mineLocationGenerator;
        private readonly IDialogService _dialogService;
        private Board _board;
        private RelayCommand _newGameCommand;
        private RelayCommand<BoardOptions> _setBoardOptionsCommand;
        private RelayCommand _exitCommand;
        private RelayCommand<Coords> _uncoverSquareCommand;
        private RelayCommand<Coords> _flagSquareCommand;
    }
}
