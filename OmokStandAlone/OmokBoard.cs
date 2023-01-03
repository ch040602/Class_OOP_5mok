using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokStandAlone
{
    enum StoneType {None = 0, Black = 1, White=2};

    class OmokBoard
    {
        int ROWS, COLS;

        private StoneType [,]  matrix ;

        public OmokBoard()
        {
            ROWS = 19; COLS = 19;
            matrix = new StoneType[ROWS, COLS];
        }
        public OmokBoard(int rows, int cols) {
            ROWS = rows;
            COLS = cols;
            matrix = new StoneType[ROWS, COLS];
        }
        public void putStone(int r, int c, StoneType stone)
        {
            matrix[r, c] = stone;
        }
        public void removeStone(int r, int c)
        {
            matrix[r, c] = StoneType.None;
        }

        public void resetboard()
        {
            for (int i = 0; i < COLS; i++)
            {
                for (int j = 0; j < ROWS; j++)
                {
                    removeStone(j, i);
                }
            }
        }

        public StoneType getStone(int r, int c)
        {
            return matrix[r, c];
        }
        public void clear()
        {
            for (int r = 0; r < ROWS; r++)
            {
                for (int c = 0; c < COLS; c++)
                {
                    matrix[r, c] = StoneType.None;
                }
            }
        }
        public int getRowCount() { return ROWS; }
        public int getColCount() { return COLS; }

        public void countSameColorStones(Position m, StoneType color, int[] stoneNum)
        {
            bool [] arr = new bool[8];

            countSameColorStones(m, color, stoneNum, arr);
        }

        public void countSameColorStones(Position m, StoneType color, int[] stoneNum, bool[] ending)
        {
            // 0 방향
            int r = m.getRow();
            int c = m.getColumn() + 1;
            int cnt = 0;

            for (int k = c; k < COLS; k++)
            {
                if (matrix[r, k] == color)
                {
                    cnt++;
                }
                else
                {
                    if (matrix[r, k] == StoneType.None) ending[0] = true;
                    else ending[0] = false;
                    break;
                }
            }
            stoneNum[0] = cnt;

            // 1 방향
            cnt = 0;
            r = m.getRow() - 1;
            c = m.getColumn() + 1;

            for (int l = r, k = c; l >= 0 && k < COLS; l--, k++)
            {
                if (matrix[l, k] == color)
                {
                    cnt++;
                }
                else
                {
                    if (matrix[l, k] == StoneType.None) ending[1] = true;
                    else ending[1] = false;
                    break;
                }
            }
            stoneNum[1] = cnt;

            // 2방향
            cnt = 0;
            r = m.getRow() - 1;  
            c = m.getColumn(); 

            for (int l = r; l >= 0; l--)
            {
                if (matrix[l, c] == color)
                {
                    cnt++;
                }
                else
                {
                    if (matrix[l, c] == StoneType.None) ending[2] = true;
                    else ending[2] = false;
                    break;
                }
            }
            stoneNum[2] = cnt;


            // 3방향
            cnt = 0;
            r = m.getRow() - 1;
            c = m.getColumn() - 1; 

            for(int l = r, k = c; l >= 0 && k >= 0; l--, k++)
            {
                if (matrix[l, k] == color)
                {
                    cnt++;
                }
                else
                {
                    if (matrix[l, k] == StoneType.None) ending[3] = true;
                    else ending[3] = false;
                    break;
                }
            }
            stoneNum[3] = cnt;

            // 4방향
            cnt = 0;
            r = m.getRow();
            c = m.getColumn() - 1;

            for (int k = c; k >= 0; k--)
            {
                if (matrix[r, k] == color)
                {
                    cnt++;
                }
                else
                {
                    if (matrix[r, k] == StoneType.None) ending[4] = true;
                    else ending[4] = false;
                    break;
                }
            }
            stoneNum[4] = cnt;

            // 5방향
            cnt = 0;
            r = m.getRow() + 1;
            c = m.getColumn() - 1;

            for (int l= r, k = c; l < ROWS && k >= 0; l++, k--)
            {
                if (matrix[l, k] == color)
                {
                    cnt++;
                }
                else
                {
                    if (matrix[l, k] == StoneType.None) ending[5] = true;
                    else ending[5] = false;
                    break;
                }
            }
            stoneNum[5] = cnt;

            // 6방향
            cnt = 0;
            r = m.getRow() + 1;
            c = m.getColumn();

            for (int l = r; l < ROWS ; l++)
            {
                if (matrix[l, c] == color)
                {
                    cnt++;
                }
                else
                {
                    if (matrix[l, c] == StoneType.None) ending[6] = true;
                    else ending[6] = false;
                    break;
                }
            }
            stoneNum[6] = cnt;


            // 7방향
            cnt = 0;
            r = m.getRow() + 1;
            c = m.getColumn() + 1;

            for (int l = r, k = c; l < ROWS && k < COLS; l++, k++)
            {
                if (matrix[l, k] == color)
                {
                    cnt++;
                }
                else
                {
                    if (matrix[l, k] == StoneType.None) ending[7] = true;
                    else ending[7] = false;
                    break;
                }
            }
            stoneNum[7] = cnt;
        }
    }
}
