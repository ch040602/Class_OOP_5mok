using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokStandAlone
{
    class HumanPlayer2 : Player
    {
        public HumanPlayer2(int id, string name, int order) : base(id, name, order, 0)
        {
        }
        public Position Play(OmokBoard board) // 돌 두기
        {
            int rows = board.getRowCount();
            int cols = board.getColCount();

            int maxVal = 0, maxRow = 0, maxCol = 0, curVal; //줄, 열 구분
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (board.getStone(r, c) == StoneType.None) // stone type 이 null 이면 없는 거니까 놓을 수 있는 거
                        curVal = evaluatePosition(board, r, c);

                    else curVal = 0;

                    if (curVal > maxVal) // 오류 잡으려고 넣은 거니까 신경ㄴㄴ
                    {
                        maxVal = curVal;
                        maxRow = r;
                        maxCol = c;
                    }
                }
            }

            return new Position(maxRow, maxCol);
        }

        private int evaluatePosition(OmokBoard board, int r, int c) // 포지션 입력
        {
            StoneType humColor = StoneType.Black;
            int[] stoneNum = new int[8];
            bool[] endingType = new bool[8];

            board.countSameColorStones(new Position(r, c), humColor, stoneNum, endingType);

            int[] stoneCnt = new int[4];
            int max = 0, maxDir = 0;

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

            int evalPartner = 0;
            if (max == 5) evalPartner = 90;
            else if (max == 4 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalPartner = 75;
            else if (max == 4 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalPartner = 65;
            else if (max == 3 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalPartner = 55;
            else if (max == 3 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalPartner = 45;
            else if (max == 2 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalPartner = 35;
            else if (max == 2 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalPartner = 25;
            else if (max == 1 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalPartner = 15;
            else if (max == 1 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalPartner = 5;

            StoneType partnerColor = StoneType.White;
            board.countSameColorStones(new Position(r, c), partnerColor, stoneNum, endingType);

            stoneCnt[0] = stoneNum[0] + stoneNum[4] + 1; // 수평방향 돌 갯수
            stoneCnt[1] = stoneNum[1] + stoneNum[5] + 1; // 수평방향 돌 갯수
            stoneCnt[2] = stoneNum[2] + stoneNum[6] + 1; // 수평방향 돌 갯수
            stoneCnt[3] = stoneNum[3] + stoneNum[7] + 1; // 수평방향 돌 갯수

            //int max = 0, maxDir = 0;
            for (int i = 0; i < 4; i++)
            {
                if (stoneCnt[i] > max)
                {
                    max = stoneCnt[i];
                    maxDir = i;
                }
            }

            int evalHum = 0;
            if (max == 5) evalHum = 100;
            else if (max == 4 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalHum = 80;
            else if (max == 4 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalHum = 70;
            else if (max == 3 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalHum = 60;
            else if (max == 3 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalHum = 50;
            else if (max == 2 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalHum = 40;
            else if (max == 2 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalHum = 30;
            else if (max == 1 && endingType[maxDir] == true && endingType[maxDir + 4] == true) evalHum = 20;
            else if (max == 1 && (endingType[maxDir] == true || endingType[maxDir + 4] == true)) evalHum = 10;

            int tmp;
            if (evalHum > evalPartner) tmp = evalHum;
            else tmp = evalPartner;

            int centerX = board.getColCount() / 2;
            int centerY = board.getRowCount() / 2;

            double distance = Math.Sqrt((c - centerX) * (c - centerX) + (r - centerY) * (r - centerY));
            double maxDist = Math.Sqrt((0 - centerX) * (0 - centerX) + (0 - centerY) * (0 - centerY));

            return (int)(tmp + (maxDist - distance));

        }
    }
}