using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokStandAlone
{
    class Position
    {
        private int row, column;

        public Position(int r, int c)
        {
            row = r;
            column = c;
        }

        public int getRow() { return row; }
        public int getColumn() { return column; }
    }
}
