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

        private static bool ValidateArrays(IEnumerable<IEnumerable<int>> array)
        {
            return array.All(x => x.Distinct().All(y => y < 10 && y > 0) && x.Count() == 9);
        }
    }
}
