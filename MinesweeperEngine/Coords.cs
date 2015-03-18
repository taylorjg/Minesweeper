namespace MinesweeperEngine
{
    public class Coords
    {
        public Coords(int row, int col)
        {
            _col = col;
            _row = row;
        }

        public int Row
        {
            get { return _row; }
        }

        public int Col
        {
            get { return _col; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            var rhs = obj as Coords;
            if (rhs == null) return false;
            return (rhs.Row == Row && rhs.Col == Col);
        }

        public override int GetHashCode()
        {
            return Col + Row;
        }

        private readonly int _row;
        private readonly int _col;
    }
}
