using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Fifteen
{

    public partial class MainWindow
    {
        private Tuple<int, int> _clearCell;
        private const int LengthRowColumn = 4;
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            Cells = new ObservableCollection<Cell>();
            for (var i = 1; i < LengthRowColumn * LengthRowColumn; i++)
            {
                var cell = new Cell { Text = $"{i}" };
                cell.MouseLeftButtonDown += (o, e) => CellClick((Cell)o);
                Cells.Add(cell);
            }
            RandomInitialize();
  //        CellsAscending();
        }

        private void RandomInitialize()
        {
            var tuples = new List<Tuple<int, int>>();
            for (var col = 0; col < LengthRowColumn; col++)
                for (var row = 0; row < LengthRowColumn; row++)
                    tuples.Add(new Tuple<int, int>(col, row));
            var index = 0;
            var random = new Random(DateTime.Now.Millisecond);
            do
            {
                var tuple = tuples[random.Next(tuples.Count)];
                var cell = Cells[index];
                cell.Column = tuple.Item1;
                cell.Row = tuple.Item2;
                tuples.Remove(tuple);
                index++;
            } while (tuples.Count > 1);
            _clearCell = tuples[0];
        }

        // win in one step for convenient testing
        private void CellsAscending()
        {
            var orderedCells = Cells.OrderBy(item => Convert.ToByte(item.Text)).ToList();
            Cells.Clear();
            foreach (var cell in orderedCells)
            {
                cell.Row = orderedCells.IndexOf(cell) / LengthRowColumn;
                cell.Column = orderedCells.IndexOf(cell) % LengthRowColumn;
                Cells.Add(cell);
            }

            Cells[LengthRowColumn * LengthRowColumn - 2].Column = LengthRowColumn - 1;
            _clearCell = new Tuple<int, int>(LengthRowColumn - 2, LengthRowColumn - 1);
        }

        private void CellClick(Cell cell)
        {
            if (_clearCell.Item1 == cell.Column - 1 && _clearCell.Item2 == cell.Row)
            {
                _clearCell = new Tuple<int, int>(cell.Column, _clearCell.Item2);
                cell.Column -= 1;
            }
            else if (_clearCell.Item1 == cell.Column + 1 && _clearCell.Item2 == cell.Row)
            {
                _clearCell = new Tuple<int, int>(cell.Column, _clearCell.Item2);
                cell.Column += 1;
            }
            else if (_clearCell.Item1 == cell.Column && _clearCell.Item2 == cell.Row - 1)
            {
                _clearCell = new Tuple<int, int>(_clearCell.Item1, cell.Row);
                cell.Row -= 1;
            }
            else if (_clearCell.Item1 == cell.Column && _clearCell.Item2 == cell.Row + 1)
            {
                _clearCell = new Tuple<int, int>(_clearCell.Item1, cell.Row);
                cell.Row += 1;
            }

            CheckResult();
        }

        private void CheckResult()
        {
            var isWin = Cells.All(cell => cell.Row == Cells.IndexOf(cell) / LengthRowColumn
                                         && cell.Column == Cells.IndexOf(cell) % LengthRowColumn
                                         && Cells.IndexOf(cell) + 1 == Convert.ToByte(cell.Text));

            if (isWin) MessageBox.Show("Вы победитель! Поздравления!", "Информация", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        #region Cells...
        private static readonly DependencyPropertyKey CellsPropertyKey = DependencyProperty.RegisterReadOnly("Cells", typeof(ObservableCollection<Cell>), typeof(MainWindow), new UIPropertyMetadata(default(ObservableCollection<Cell>)));
        public static readonly DependencyProperty CellsProperty = CellsPropertyKey.DependencyProperty;
        public ObservableCollection<Cell> Cells
        {
            get => (ObservableCollection<Cell>)GetValue(CellsProperty);
            private set => SetValue(CellsPropertyKey, value);
        }
        #endregion
    }
}
