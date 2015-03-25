using System.Windows.Media;
using Minesweeper.Mappers;
using MinesweeperEngine;

namespace Minesweeper.ViewModels
{
    public class SquareViewModel
    {
        public SquareViewModel(SquareState squareState, Coords coords, int numSurroundingMines, bool isMine)
        {
            _squareState = squareState;
            _numSurroundingMines = numSurroundingMines;
            _coords = coords;
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

        public Coords Coords
        {
            get { return _coords; }
        }

        private readonly SquareState _squareState;
        private readonly int _numSurroundingMines;
        private readonly Coords _coords;
        private readonly string _displayText;
        private readonly Brush _color;
    }
}
