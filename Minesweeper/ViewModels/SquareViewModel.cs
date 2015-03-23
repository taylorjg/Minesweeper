using System.Windows.Media;
using Minesweeper.Mappers;

namespace Minesweeper.ViewModels
{
    public class SquareViewModel
    {
        public SquareViewModel(SquareState squareState, int numSurroundingMines, bool isMine)
        {
            _squareState = squareState;
            _numSurroundingMines = numSurroundingMines;
            _displayText = SquareDataMapper.MapSquareStateToString(_squareState, _numSurroundingMines);
            _color = new SolidColorBrush((isMine) ? Colors.Red : Colors.Beige);
        }

        public SquareState SquareState
        {
            get { return _squareState; }
        }

        public int NumSurroundingMines
        {
            get { return _numSurroundingMines; }
        }

        public string DisplayText
        {
            get { return _displayText; }
        }

        public Brush Color
        {
            get { return _color; }
        }

        private readonly SquareState _squareState;
        private readonly int _numSurroundingMines;
        private readonly string _displayText;
        private readonly Brush _color;
    }
}
