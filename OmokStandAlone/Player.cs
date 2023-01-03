using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokStandAlone
{
    public class Player
    {
        private int id, order, playerType;
        double winningRate;
        string name;

        public Player(int id, string name, int order, int playerType)
        {
            this.id = id;
            this.name = name;
            this.order = order;
            this.playerType = playerType; // 0이면 사람, 1이면 컴퓨터
        }
        public int getId() { return id; }
        public string getName() { return name; }
        public int getOrder() { return order; }
        public double getWinningRate() { return winningRate; } // ?
        public int getPlayerType() { return playerType; }
        public void setPlayerType(int type) { playerType = type; }
        public void setName(String name) { this.name = name; }
    }
}
