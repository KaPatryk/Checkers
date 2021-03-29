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
        Button[,] szachownica = new Button[8, 8];
        static int buttonSize = 50;

        Image redFigure = new Bitmap(new Bitmap("C:\\Users\\megak\\source\\repos\\Checkers\\Checkers\\Assets\\red.png"), new Size(buttonSize-15, buttonSize-15));
        Image greyFigure = new Bitmap(new Bitmap("C:\\Users\\megak\\source\\repos\\Checkers\\Checkers\\Assets\\grey.png"), new Size(buttonSize - 15, buttonSize - 15));

        public Form1()
        {
            InitializeComponent();
            InitializeBoard(szachownica);
            NewGame(szachownica);

            
            
        }
        private void InitializeBoard(Button [,] szachownica)
        {
            
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
                    szachownica[column,row].Click += new System.EventHandler(this.Move);
                }
            }
        }
        


        private void NewGame(Button [,] szachownica)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (row % 2 == 0 && column % 2 != 0) szachownica[row, column].Image = redFigure;
                    else if (row % 2 != 0 && column % 2 == 0) szachownica[row, column].Image = redFigure;
                }
            }

            for (int row = 5; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (row % 2 == 0 && column % 2 != 0) szachownica[row, column].Image = greyFigure;
                    else if (row % 2 != 0 && column % 2 == 0) szachownica[row, column].Image = greyFigure;
                }
            }
        }

        void Move(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;
            currentButton.Enabled = true;
        }

    }
}
