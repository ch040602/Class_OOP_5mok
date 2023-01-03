using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmokStandAlone
{
    public partial class Form1 : Form
    {
        Match omokMatch;
        Boolean isStarted = false;
        private bool black_chk = true;
        double wInterval, hInterval;

        public Form1()
        {
            InitializeComponent();
        }

        public void set_black1()
        {
            black_chk = true;
        }
        public void set_white1()
        {
            black_chk = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            omokMatch = new Match();
            omokMatch.setTurn(0); // 첫 번째 주자 
            omokMatch.setPlayType(0);  // 로컬 게임
            Player p1 = new Player(1001, "bongbong", 0, 0); 
            omokMatch.setPlayer(0, p1);
            Player p2 = new Player(1002, "soosoo", 1, 0);
            omokMatch.setPlayer(1, p2);
            omokMatch.setBoarSize(2);
            omokMatch.setBoard(new OmokBoard());
            omokMatch.setPlayer(1, new HumanPlayer2(1234, "hum2", 1));
            omokMatch.setPlayer(0, new HumanPlayer(3456, "hum", 0));
        }

        private void 프로그램종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setttings settingForm = new setttings();

            if (settingForm.ShowDialog() == DialogResult.OK)
            {
                if (settingForm.getPlayType() == 0)
                {
                    Player p2 = new Player(1002, "채승훈", 1, 0);
                    omokMatch.setPlayer(1, p2);
                }
                else if (settingForm.getPlayType() == 1)
                {
                    omokMatch.getPlayer(1).setPlayerType(1);
                    omokMatch.setPlayer(1, new ComputerPlayer(1234, "com", 1));
                    omokMatch.setPlayer(0, new HumanPlayer(3456, "hum", 0));
                }

                else if(settingForm.getPlayType() == 2)
                {
                    omokMatch.setPlayType(1);
                    omokMatch.setPlayer(1, null);
                }

                omokMatch.setBoarSize(settingForm.getBoardSize());
                black_chk = settingForm.get_Bchk();
                if(black_chk == true)
                {
                    omokMatch.getPlayer(0).setName(settingForm.getFirstPlayerName());
                    omokMatch.getPlayer(1).setName(settingForm.getSecondPlayerName());
                }
                else
                {
                    omokMatch.getPlayer(1).setName(settingForm.getFirstPlayerName());
                    omokMatch.getPlayer(0).setName(settingForm.getSecondPlayerName());
                }
                pictureBox1.Invalidate();
                displayCurrentPlayerInfo();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;

            int lines, size, start, interval;

            size = omokMatch.getBoardSize();
            if (size == 0)
            {
                lines = 11;start = 2;interval = 4;
            }

            else if (size == 1)
            {
                lines = 15;start = 3;interval = 5;
            }
            else
            {
                lines = 19;start = 4;interval = 6;
            }

            wInterval = (double)width / (lines + 1);
            hInterval = (double)height / (lines + 1);

            Pen blackPen = new Pen(Color.Black, 1);
            for (int i = 1; i <= lines; i++) // 수직선
            {
                g.DrawLine(blackPen, new Point((int) (i * wInterval),(int) hInterval), new Point((int) (i * wInterval), (int) (height - hInterval)));
            }

            for (int i = 1; i <= lines; i++) // 수평선
            {
                g.DrawLine(blackPen, new Point((int)wInterval, (int)(i * hInterval)), new Point((int)(width - wInterval), (int) (i * hInterval)));
            }

            // 화점 그리기
            for (int r = start; r <= lines; r = r + interval)
            {
                for (int c = start; c <= lines; c = c + interval)
                {
                    g.FillEllipse(new SolidBrush(Color.Black), (int) (r*wInterval - 5), (int) (c*hInterval - 5), 10, 10);
                }
            }

            // 사용자 돌 그리기
            OmokBoard board = omokMatch.getBaord();
            StoneType dol;
            for (int r = 1; r <= lines; r++)
            {
                for (int c = 1; c <= lines; c++)
                {
                    dol = board.getStone(r-1, c-1);
                    if (dol == StoneType.Black)
                    {
                        g.FillEllipse(new SolidBrush(Color.Black), (int)(r * wInterval - 10), (int)(c * hInterval - 10), 20, 20);
                    }
                    else if (dol == StoneType.White) {
                        g.FillEllipse(new SolidBrush(Color.White), (int)(r * wInterval - 10), (int)(c * hInterval - 10), 20, 20);
                    }
                }
            }

            g.FillEllipse(new SolidBrush(Color.Red), (int)(lastRow * wInterval - 3), (int)(lastColumn * hInterval - 3), 5, 5); //마지막 돌 표시

            //Pen wow = new Pen(Color.Blue, 3);
            g.DrawEllipse(Pens.Blue, (int)(BlueX * wInterval + 10), (int)(BlueY * hInterval + 10), 20, 20); //사용자 위치 추천 표시

        }



        private void 게임시작ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (black_chk == true) omokMatch.setTurn(0);
            else omokMatch.setTurn(1);

            displayCurrentPlayerInfo();
            pictureBox1.Invalidate();
            isStarted = true;

            if (omokMatch.getCurrentPlayer().getPlayerType() == 1)
            {
                pictureBox1_MouseClick(null, null);
            }
            게임종료ToolStripMenuItem.Enabled = true;
            게임시작ToolStripMenuItem.Enabled = false;
            설정ToolStripMenuItem.Enabled = false;
        }

        private void displayCurrentPlayerInfo()
        {
            label1.Text = omokMatch.getCurrentPlayer().getName();

            if (omokMatch.getCurrentColor() == StoneType.Black) button1.BackColor = Color.Black;
            else if (omokMatch.getCurrentColor() == StoneType.White) button1.BackColor = Color.White;
            
        }
        
        //전역변수
        public int newStoneX = 0;
        public int newStoneY = 0;
        public int AiStoneX = 0;
        public int AiStoneY = 0;
        public int lastRow = -1;
        public int lastColumn = -1;
        public int BlueX = -10;
        public int BlueY = -10;

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int r=0;
            int c=0;
            if (isStarted == true)
            {
                
                if (omokMatch.getCurrentPlayer().getPlayerType() == 0)
                {
                     r = (int)((e.X + wInterval * 0.5) / wInterval)-1;
                     c = (int)((e.Y + hInterval * 0.5) / hInterval)-1;
                     newStoneX = r;
                     newStoneY = c;
                }
                else
                {
                    Position p = ((ComputerPlayer)omokMatch.getCurrentPlayer()).Play(omokMatch.getBaord());
                    r = p.getRow();
                    c = p.getColumn();
                    AiStoneX = r;
                    AiStoneY = c;
                }

                lastRow = r + 1;
                lastColumn = c + 1;

                if (omokMatch.getBaord().getStone(r, c) == StoneType.None)
                {
                    BlueX = -10;
                    BlueY = -10;
                    omokMatch.getBaord().putStone(r, c, omokMatch.getCurrentColor());
                    // 바둑판 다시 그리기
                    pictureBox1.Invalidate();
                    if (omokMatch.checkWinningCondition(new Position(r, c)) == true)
                    {
                        MessageBox.Show("축하!!!"+omokMatch.getCurrentPlayer().getName()+"(이)가 승리하셨습니다.");
                        게임종료ToolStripMenuItem_Click(null, null);
                        return;
                    }
                    int curTurn = omokMatch.getTurn();
                    int nextTurn = curTurn == 0 ? 1 : 0;


                    // 다음 순서로
                    omokMatch.setTurn(nextTurn);
                    if(omokMatch.getCurrentPlayer().getPlayerType()==1)
                    {
                        BlueX = -10;
                        BlueY = -10;
                        pictureBox1_MouseClick(null,null);
                    }
                    displayCurrentPlayerInfo();
                }
                else
                {
                    MessageBox.Show("선택 위치에는 이미 돌이 놓여 있습니다. 다른 곳을 선택해 주세요.");
                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void 게임종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            omokMatch.getBaord().resetboard();

            pictureBox1.Invalidate();

            isStarted = false;
            게임종료ToolStripMenuItem.Enabled = false;
            게임시작ToolStripMenuItem.Enabled = true;
            설정ToolStripMenuItem.Enabled = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isStarted)
            {
                if (omokMatch.getBaord().getStone(newStoneX, newStoneY) == StoneType.Black)
                {
                    omokMatch.setTurn(0);
                    displayCurrentPlayerInfo();

                }
                else if (omokMatch.getBaord().getStone(newStoneX, newStoneY) == StoneType.White)
                {
                    omokMatch.setTurn(1);
                    displayCurrentPlayerInfo();

                }
                omokMatch.getBaord().removeStone(newStoneX, newStoneY);
                omokMatch.getBaord().removeStone(AiStoneX, AiStoneY);

                pictureBox1.Invalidate();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            omokMatch.getBaord().resetboard();
            if (black_chk == true) omokMatch.setTurn(0);
            else if (omokMatch.getPlayer(1).getPlayerType() == 1) omokMatch.setTurn(1);

            displayCurrentPlayerInfo();
            pictureBox1.Invalidate();
            isStarted = true;
            if (omokMatch.getCurrentPlayer().getPlayerType() == 1)
            {
                pictureBox1_MouseClick(null, null);
            }
            게임종료ToolStripMenuItem.Enabled = true;
            게임시작ToolStripMenuItem.Enabled = false;
            설정ToolStripMenuItem.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        public int leftHint = 3;
        public int rightHint = 3;

        private void button2_Click_1(object sender, EventArgs e)
        {
            int m = 0;
            int n = 0;
            
            if (isStarted == true && leftHint > 0 && omokMatch.getTurn() == 0)
            {
                if (omokMatch.getCurrentPlayer().getPlayerType() == 0)
                {
                    if (omokMatch.getCurrentColor() == StoneType.Black)
                    {
                        Position h = ((HumanPlayer)omokMatch.getCurrentPlayer()).Play(omokMatch.getBaord());
                        m = h.getRow();
                        n = h.getColumn();
                        BlueX = m;
                        BlueY = n;
                    }
                    if (omokMatch.getCurrentColor() == StoneType.White)
                    {
                        Position h = ((HumanPlayer2)omokMatch.getCurrentPlayer()).Play(omokMatch.getBaord());
                        m = h.getRow();
                        n = h.getColumn();
                        BlueX = m;
                        BlueY = n;
                    }
                }
                pictureBox1.Invalidate();
                leftHint--;
            }
            else if (leftHint <= 0)
            {
                MessageBox.Show("힌트 다썼어.");
            }

            if (isStarted == true && rightHint > 0 && omokMatch.getTurn() == 1)
            {
                if (omokMatch.getCurrentPlayer().getPlayerType() == 0)
                {
                    if (omokMatch.getCurrentColor() == StoneType.Black)
                    {
                        Position h = ((HumanPlayer)omokMatch.getCurrentPlayer()).Play(omokMatch.getBaord());
                        m = h.getRow();
                        n = h.getColumn();
                        BlueX = m;
                        BlueY = n;
                    }
                    if (omokMatch.getCurrentColor() == StoneType.White)
                    {
                        Position h = ((HumanPlayer2)omokMatch.getCurrentPlayer()).Play(omokMatch.getBaord());
                        m = h.getRow();
                        n = h.getColumn();
                        BlueX = m;
                        BlueY = n;
                    }
                }
                pictureBox1.Invalidate();
                rightHint--;
            }
            else if (rightHint <= 0)
            {
                MessageBox.Show("힌트 다썼어.");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
