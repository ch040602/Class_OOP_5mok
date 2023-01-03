using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokStandAlone
{
    class Match
    {
        private Player[] players = new Player[2];
        private int turn; // 0번이면 첫 번째 주자, 1번이면 두 번째 주자
        private int playType; // 0번이면 로컬 게임, 1번이면 네트워크 게임
        private OmokBoard playBoard;
        private int boardSize; // 0이면 11x11, 1이면 15x15, 2이면 19x19

        public bool checkWinningCondition(Position pos) {
            int[] counts = new int[8];

            playBoard.countSameColorStones(pos, getCurrentColor(), counts);

            int count = counts[0] + counts[4] + 1;
            if (count == 5) return true;

            count = counts[1] + counts[5] + 1;
            if (count == 5) return true;

            count = counts[2] + counts[6] + 1;
            if (count == 5) return true;

            count = counts[3] + counts[7] + 1;
            if (count == 5) return true;

            return false;
        }
        private bool checkValidity(Position pos)
        {
            StoneType type = playBoard.getStone(pos.getRow(), pos.getColumn());
            if (type == StoneType.None)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int getTurn()
        {
            return turn;
        }
        public void setTurn(int n)
        {
            turn = n;
        }
        public Player getPlayer(int n)
        {
            return players[n];
        }
        public Player getCurrentPlayer()
        {
            return players[turn];
        }
        public OmokBoard getBaord()
        {
            return playBoard;
        }
        public void setBoard(OmokBoard board)
        {
            playBoard = board;
        }
        public StoneType getCurrentColor()
        {
            if (turn == 0) // 첫 번째 주자
            {
                return StoneType.Black;
            }
            else
            {
                return StoneType.White;
            }
        }
        public void setPlayer(int n, Player player)
        {
            players[n] = player;
        }
        public int getPlayType()
        {
            return playType;
        }
        public void setPlayType(int n) {
            if (n == 0 || n == 1) playType = n;
        }
        public int getBoardSize() { return boardSize; }
        public void setBoarSize(int size) { boardSize = size; }
    }
}
