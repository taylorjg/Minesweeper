using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Minesweeper.AttachedProperties
{
    // https://rachel53461.wordpress.com/2011/09/17/wpf-grids-rowcolumn-count-properties/
    public class GridHelpers
    {
        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.RegisterAttached(
                "RowCount",
                typeof(int),
                typeof(GridHelpers),
                new PropertyMetadata(-1, RowCountChanged));

        public static int GetRowCount(DependencyObject obj)
        {
            return (int)obj.GetValue(RowCountProperty);
        }

        public static void SetRowCount(DependencyObject obj, int value)
        {
            obj.SetValue(RowCountProperty, value);
        }

        public static void RowCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || (int)e.NewValue < 0)
                return;

            var grid = (Grid)obj;
            grid.RowDefinitions.Clear();

            for (var i = 0; i < (int)e.NewValue; i++)
                grid.RowDefinitions.Add(new RowDefinition());

            SetStarRows(grid);
        }

        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.RegisterAttached(
                "ColumnCount",
                typeof(int),
                typeof(GridHelpers),
                new PropertyMetadata(-1, ColumnCountChanged));

        public static int GetColumnCount(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnCountProperty);
        }

        public static void SetColumnCount(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnCountProperty, value);
        }

        public static void ColumnCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || (int)e.NewValue < 0)
                return;

            var grid = (Grid)obj;
            grid.ColumnDefinitions.Clear();

            for (var i = 0; i < (int)e.NewValue; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            SetStarColumns(grid);
        }

        public static readonly DependencyProperty StarRowsProperty =
            DependencyProperty.RegisterAttached(
                "StarRows",
                typeof(string),
                typeof(GridHelpers),
                new PropertyMetadata(string.Empty, StarRowsChanged));

        public static string GetStarRows(DependencyObject obj)
        {
            return (string)obj.GetValue(StarRowsProperty);
        }

        public static void SetStarRows(DependencyObject obj, string value)
        {
            obj.SetValue(StarRowsProperty, value);
        }

        public static void StarRowsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || string.IsNullOrEmpty(e.NewValue.ToString()))
                return;

            SetStarRows((Grid)obj);
        }

        public static readonly DependencyProperty StarColumnsProperty =
            DependencyProperty.RegisterAttached(
                "StarColumns",
                typeof(string),
                typeof(GridHelpers),
                new PropertyMetadata(string.Empty, StarColumnsChanged));

        public static string GetStarColumns(DependencyObject obj)
        {
            return (string)obj.GetValue(StarColumnsProperty);
        }

        public static void SetStarColumns(DependencyObject obj, string value)
        {
            obj.SetValue(StarColumnsProperty, value);
        }

        public static void StarColumnsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || string.IsNullOrEmpty(e.NewValue.ToString()))
                return;

            SetStarColumns((Grid)obj);
        }

        private static void SetStarColumns(Grid grid)
        {
            var starColumns = GetStarColumns(grid).Split(',');

            for (var i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                if (starColumns.Contains(i.ToString()))
                    grid.ColumnDefinitions[i].Width =
                        new GridLength(1, GridUnitType.Star);
            }
        }

        private static void SetStarRows(Grid grid)
        {
            var starRows = GetStarRows(grid).Split(',');

            for (var i = 0; i < grid.RowDefinitions.Count; i++)
            {
                if (starRows.Contains(i.ToString()))
                    grid.RowDefinitions[i].Height =
                        new GridLength(1, GridUnitType.Star);
            }
        }
    }
}
