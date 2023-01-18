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
            var row = Enumerable.Range(0, 9).Select(x => Enumerable.Range(0, 9).Select(y => matrix[x, y]));
            var column = Enumerable.Range(0, 9).Select(x => Enumerable.Range(0, 9).Select(y => matrix[y, x]));
            IEnumerable<IEnumerable<int>> temp = new[] { new int[0] };
            var square = Enumerable.Range(0, 3).Aggregate(temp, (a, b) =>
            {
                return a.Concat(Enumerable.Range(0, 3).Select(x =>
                            Enumerable.Range(0, 3).SelectMany(c => Enumerable.Range(0, 3).Select(d =>
                                    matrix[c + 3 * b, d + (3 * x)]))));
            }).Skip(1);

            return ValidateArrays(row) && ValidateArrays(column) && ValidateArrays(square);
        }

        private static bool ValidateArrays(IEnumerable<IEnumerable<int>> array)
        {
            return array.All(x => x.Distinct().All(y => y < 10 && y > 0) && x.Count() == 9);
        }
    }
}
