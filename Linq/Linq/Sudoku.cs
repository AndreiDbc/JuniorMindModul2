using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class Sudoku
    {
        private readonly int[,] matrix;

        public Sudoku(int[,] matrix)
        {
            this.matrix = matrix;
        }

        public bool ValidateSudokuBoard()
        {
            var rows = Enumerable.Range(0, 9).Select(x => Enumerable.Range(0, 9).Select(y => matrix[x, y]));
            var columns = Enumerable.Range(0, 9).Select(x => Enumerable.Range(0, 9).Select(y => matrix[y, x]));
            var squares = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 3).SelectMany(j =>
                Enumerable.Range(0, 3).Select(k => matrix[i / 3 * 3 + j, i % 3 * 3 + k])));

            return rows.Concat(columns).Concat(squares).All(ValidateArray);
        }

        private static bool ValidateArray(IEnumerable<int> array)
        {
            return array.All(y => y < 10 && y > 0) && array.Distinct().Count() == 9;
        }
    }
}
