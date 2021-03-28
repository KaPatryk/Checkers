using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
        }
        private void InitializeBoard()
        {
            Button[,] szachownica = new Button[8, 8];
            int buttonSize = 50;
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    szachownica[column, row] = new Button();
                    szachownica[column, row].Location = new Point(buttonSize * row, buttonSize * column);
                    szachownica[column, row].Size = new Size(buttonSize, buttonSize);
                    if (row % 2 == 0 && column % 2 == 0) szachownica[column, row].BackColor = Color.White;
                    else if (row % 2 != 0 && column % 2 == 0) szachownica[column, row].BackColor = Color.Black;
                    else if (row % 2 != 0 && column % 2 != 0) szachownica[column, row].BackColor = Color.White;
                    else if (row % 2 == 0 && column % 2 != 0) szachownica[column, row].BackColor = Color.Black;
                    this.Controls.Add(szachownica[column, row]);
                }
            }
        }
    }
}
