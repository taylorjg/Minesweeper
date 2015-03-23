using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Minesweeper.Annotations;
using Minesweeper.Mappers;
using MinesweeperEngine;

namespace Minesweeper.ViewModels
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        public BoardViewModel(Board board)
        {
            _board = board;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UncoverSquare(Coords coords)
        {
            _board.UncoverSquare(coords);
            OnPropertyChanged(Binding.IndexerName);
        }

        public void FlagSquare(Coords coords)
        {
            _board.FlagSquare(coords);
            OnPropertyChanged(Binding.IndexerName);
            OnPropertyChanged("UnflaggedMineCount");
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Board _board;
    }
}
