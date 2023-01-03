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
    public partial class setttings : Form
    {
        private int playType, boardSize;
        private String fPlayer, sPlayer;
        private bool black_chk = true;

        public setttings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton6.Checked == true)
            {
                if (radioButton1.Checked == true) playType = 0; // 사람 대 사람
                else playType = 1; // 사람 대 컴퓨터
            }
            else playType = 2;
            if (radioButton3.Checked == true) boardSize = 0; // 11x11
            else if (radioButton4.Checked == true) boardSize = 1; // 15x15
            else boardSize = 2; // 19x19

            if (radioButton7.Checked == true) black_chk = true;
            else black_chk = false;

            fPlayer = textBox1.Text;
            sPlayer = textBox2.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public int getPlayType()
        {
            return playType;
        }
        public int getBoardSize()
        {
            return boardSize;
        }
        public bool get_Bchk() { 
            return black_chk; 
        }

        public String getFirstPlayerName()
        {
            return fPlayer;
        }
        public String getSecondPlayerName()
        {
            return sPlayer;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "인공지능";
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "승훈이";
        }

     
    }
}
