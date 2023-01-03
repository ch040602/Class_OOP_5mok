using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokStandAlone
{
    class ComputerPlayer : Player
    {
        public ComputerPlayer(int id, string name, int order) : base(id, name, order, 1)
        {            
        }

        public Position Play(OmokBoard board)
        {
            int rows = board.getRowCount();
            int cols = board.getColCount();

            int maxVal=0, maxRow=0, maxCol=0, curVal; //줄, 열 구분
            for(int r=0; r < rows; r++) {
                for(int c=0; c < cols; c++) {
                    if (board.getStone(r, c) == StoneType.None) curVal = evaluatePosition(board, r, c);
                    else curVal = 0;

                    if (curVal > maxVal)
                    {
                        maxVal = curVal;
                        maxRow = r;
                        maxCol = c;
                    }
                }
            }

            return new Position(maxRow, maxCol);
        }

        private int evaluatePosition(OmokBoard board, int r, int c)
        {
            StoneType comColor = StoneType.White;
            int[] stoneNum = new int[8];
            bool[] endingType = new bool[8];

            board.countSameColorStones(new Position(r, c), comColor, stoneNum, endingType);

            int[] stoneCnt = new int[4];
            stoneCnt[0] = stoneNum[0] + stoneNum[4] + 1; // 수평방향 돌 갯수
            stoneCnt[1] = stoneNum[1] + stoneNum[5] + 1; // 수평방향 돌 갯수
            stoneCnt[2] = stoneNum[2] + stoneNum[6] + 1; // 수평방향 돌 갯수
            stoneCnt[3] = stoneNum[3] + stoneNum[7] + 1; // 수평방향 돌 갯수

            int max = 0, maxDir = 0;
            for (int i = 0; i < 4; i++)
            {
                if (stoneCnt[i] > max)
                {
                    max = stoneCnt[i];
                    maxDir = i;
                }
            }

            int evalCom=0;
            if (max == 5) evalCom = 100;
            else if (max == 4 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalCom = 80;
            else if (max == 4 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalCom = 70;
            else if (max == 3 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalCom = 60;
            else if (max == 3 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalCom = 50;
            else if (max == 2 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalCom = 40;
            else if (max == 2 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalCom = 30;
            else if (max == 1 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalCom = 20;
            else if (max == 1 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalCom = 10;

            StoneType partnerColor = StoneType.Black;
            board.countSameColorStones(new Position(r, c), partnerColor, stoneNum, endingType);

            stoneCnt[0] = stoneNum[0] + stoneNum[4] + 1; // 수평방향 돌 갯수
            stoneCnt[1] = stoneNum[1] + stoneNum[5] + 1; // 수평방향 돌 갯수
            stoneCnt[2] = stoneNum[2] + stoneNum[6] + 1; // 수평방향 돌 갯수
            stoneCnt[3] = stoneNum[3] + stoneNum[7] + 1; // 수평방향 돌 갯수

            for (int i = 0; i < 4; i++)
            {
                if (stoneCnt[i] > max)
                {
                    max = stoneCnt[i];
                    maxDir = i;
                }
            }

            int evalPartner=0;
            if (max == 5) evalPartner = 90;
            else if (max == 4 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalPartner = 75;
            else if (max == 4 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalPartner = 65;
            else if (max == 3 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalPartner = 55;
            else if (max == 3 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalPartner = 45;
            else if (max == 2 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalPartner = 35;
            else if (max == 2 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalPartner = 25;
            else if (max == 1 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalPartner = 15;
            else if (max == 1 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalPartner = 5;

            int tmp;
            if (evalCom > evalPartner) tmp = evalCom;
            else tmp = evalPartner;

            int centerX = board.getColCount() / 2;
            int centerY = board.getRowCount() / 2;

            double distance = Math.Sqrt((c - centerX) * (c - centerX) + (r - centerY) * (r - centerY));
            double maxDist = Math.Sqrt((0 - centerX) * (0 - centerX) + (0 - centerY) * (0 - centerY));

            return (int)(tmp + (maxDist - distance));


        }
    }
}
