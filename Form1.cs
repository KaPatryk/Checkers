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
        BoardButton[,] szachownica = new BoardButton[8, 8];
        ChosenFigure? chosenFigure;
        int whichTurn = 1;
        int greyPoints = 0;
        int redPoints = 0;

        
        static int buttonSize = 50;

        Image redFigure = new Bitmap(new Bitmap("C:\\Users\\megak\\source\\repos\\Checkers\\Checkers\\Assets\\red.png"), new Size(buttonSize-15, buttonSize-15));
        Image greyFigure = new Bitmap(new Bitmap("C:\\Users\\megak\\source\\repos\\Checkers\\Checkers\\Assets\\grey.png"), new Size(buttonSize - 15, buttonSize - 15));

        public Form1()
        {
            chosenFigure = null;
            InitializeComponent();
            InitializeBoard(szachownica);
            NewGame(szachownica);
            EnableButtons();
            UpdatePoints(redPoints, greyPoints);
            TurnIndicator(whichTurn);
        }

        private void InitializeBoard(BoardButton [,] szachownica)
        {
            
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    szachownica[column, row] = new BoardButton();
                    szachownica[column, row].Location = new Point(buttonSize * column, buttonSize * row);
                    szachownica[column, row].Size = new Size(buttonSize, buttonSize);
                    if (row % 2 == 0 && column % 2 == 0) szachownica[column, row].BackColor = Color.White;
                    else if (row % 2 != 0 && column % 2 == 0) szachownica[column, row].BackColor = Color.Black;
                    else if (row % 2 != 0 && column % 2 != 0) szachownica[column, row].BackColor = Color.White;
                    else if (row % 2 == 0 && column % 2 != 0) szachownica[column, row].BackColor = Color.Black;

                    szachownica[column, row].Column = column;
                    szachownica[column, row].Row = row;
                    this.Controls.Add(szachownica[column, row]);
                    szachownica[column, row].Click += new System.EventHandler(this.Move);
                    
                }
            }
        }
        


        private void NewGame(BoardButton [,] szachownica)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (row % 2 == 0 && column % 2 != 0) szachownica[column, row].Image = redFigure;
                    else if (row % 2 != 0 && column % 2 == 0) szachownica[column, row].Image = redFigure;
                }
            }

            for (int row = 5; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (row % 2 == 0 && column % 2 != 0) szachownica[column, row].Image = greyFigure;
                    else if (row % 2 != 0 && column % 2 == 0) szachownica[column, row].Image = greyFigure;
                }
            }
            
        }

        void Move(object sender, EventArgs e)
        {
            BoardButton currentButton = (BoardButton)sender;
            
            if (currentButton.Image != null && currentButton.IsEnabled == true)
            {
                chosenFigure = new ChosenFigure(currentButton.Column, currentButton.Row, currentButton.Image);
                PosibblePossitions(chosenFigure);
            }
            
            if(currentButton.Image == null && chosenFigure !=null && chosenFigure.IsChosen == true && currentButton.BackColor == Color.Black && currentButton.IsEnabled == true)
            {
                currentButton.Image = chosenFigure.Figure;

                int ecuation = currentButton.Column - chosenFigure.Column;

                RemoveFromOldPosition(chosenFigure);
                if (ecuation > 1)
                {
                    ExecuteFigure(szachownica[chosenFigure.Column + 1, chosenFigure.Row + whichTurn]);
                    if (IsSomebodyToBeat(currentButton))
                    {
                        chosenFigure = new ChosenFigure(currentButton.Column, currentButton.Row, currentButton.Image);
                        PosibblePossitions(chosenFigure);
                    }
                }
                else if (ecuation < -1)
                {
                    ExecuteFigure(szachownica[chosenFigure.Column - 1, chosenFigure.Row + whichTurn]);
                    if (IsSomebodyToBeat(currentButton))
                    {
                        chosenFigure = new ChosenFigure(currentButton.Column, currentButton.Row, currentButton.Image);
                        PosibblePossitions(chosenFigure);
                    }
                }
                else NextTurn();
                
                
            }
        }

        void RemoveFromOldPosition(ChosenFigure chosenFigure)
        {
            int column = chosenFigure.Column;
            int row = chosenFigure.Row;
            szachownica[column, row].Image = null;
            chosenFigure.IsChosen = false;
            ResetPossiblePositions();
        }

        enum Team
        {
            Red, Grey
        }

        void PosibblePossitions(ChosenFigure chosenFigure)
        {
            ResetPossiblePositions();
            if(chosenFigure.IsChosen == true)
            {
                try{
                    if (szachownica[chosenFigure.Column + 1, chosenFigure.Row + whichTurn].Image == null)
                    {
                        szachownica[chosenFigure.Column + 1, chosenFigure.Row + whichTurn].FlatStyle = FlatStyle.Flat;
                        szachownica[chosenFigure.Column + 1, chosenFigure.Row + whichTurn].FlatAppearance.BorderSize = 5;
                        szachownica[chosenFigure.Column + 1, chosenFigure.Row + whichTurn].FlatAppearance.BorderColor = Color.Green;
                        szachownica[chosenFigure.Column + 1, chosenFigure.Row + whichTurn].IsEnabled = true;
                    }
                    if(szachownica[chosenFigure.Column + 1, chosenFigure.Row + whichTurn].Image != chosenFigure.Figure && szachownica[chosenFigure.Column + 1, chosenFigure.Row + whichTurn].Image != null)
                    {
                        if (szachownica[chosenFigure.Column + 2* 1, chosenFigure.Row + 2* whichTurn].Image == null)
                        {
                            szachownica[chosenFigure.Column + 2* 1, chosenFigure.Row + 2* whichTurn].FlatStyle = FlatStyle.Flat;
                            szachownica[chosenFigure.Column + 2* 1, chosenFigure.Row + 2* whichTurn].FlatAppearance.BorderSize = 5;
                            szachownica[chosenFigure.Column + 2* 1, chosenFigure.Row + 2* whichTurn].FlatAppearance.BorderColor = Color.Green;
                            szachownica[chosenFigure.Column + 2 * 1, chosenFigure.Row + 2 * whichTurn].IsEnabled = true;
                        }
                    }
                    if (szachownica[chosenFigure.Column + 1, chosenFigure.Row - whichTurn].Image != chosenFigure.Figure && szachownica[chosenFigure.Column + 1, chosenFigure.Row + whichTurn].Image != null)
                    {
                        if (szachownica[chosenFigure.Column + 2 * 1, chosenFigure.Row - 2 * whichTurn].Image == null)
                        {
                            szachownica[chosenFigure.Column + 2 * 1, chosenFigure.Row - 2 * whichTurn].FlatStyle = FlatStyle.Flat;
                            szachownica[chosenFigure.Column + 2 * 1, chosenFigure.Row - 2 * whichTurn].FlatAppearance.BorderSize = 5;
                            szachownica[chosenFigure.Column + 2 * 1, chosenFigure.Row - 2 * whichTurn].FlatAppearance.BorderColor = Color.Green;
                            szachownica[chosenFigure.Column + 2 * 1, chosenFigure.Row - 2 * whichTurn].IsEnabled = true;
                        }
                    }

                }
                catch { }
                try{
                    if (szachownica[chosenFigure.Column - 1, chosenFigure.Row + whichTurn].Image == null)
                    {
                        szachownica[chosenFigure.Column - 1, chosenFigure.Row + whichTurn].FlatStyle = FlatStyle.Flat;
                        szachownica[chosenFigure.Column - 1, chosenFigure.Row + whichTurn].FlatAppearance.BorderSize = 5;
                        szachownica[chosenFigure.Column - 1, chosenFigure.Row + whichTurn].FlatAppearance.BorderColor = Color.Green;
                        szachownica[chosenFigure.Column - 1, chosenFigure.Row + whichTurn].IsEnabled = true;
                    }
                    if (szachownica[chosenFigure.Column - 1, chosenFigure.Row + whichTurn].Image != chosenFigure.Figure && szachownica[chosenFigure.Column - 1, chosenFigure.Row + whichTurn].Image != null)
                    {
                        if(szachownica[chosenFigure.Column - 2* 1, chosenFigure.Row + 2* whichTurn].Image == null)
                        {
                            szachownica[chosenFigure.Column - 2* 1, chosenFigure.Row + 2* whichTurn].FlatStyle = FlatStyle.Flat;
                            szachownica[chosenFigure.Column - 2* 1, chosenFigure.Row + 2* whichTurn].FlatAppearance.BorderSize = 5;
                            szachownica[chosenFigure.Column - 2* 1, chosenFigure.Row + 2* whichTurn].FlatAppearance.BorderColor = Color.Green;
                            szachownica[chosenFigure.Column - 2 * 1, chosenFigure.Row + 2 * whichTurn].IsEnabled = true;
                        }
                    }
                    if (szachownica[chosenFigure.Column - 1, chosenFigure.Row - whichTurn].Image != chosenFigure.Figure && szachownica[chosenFigure.Column - 1, chosenFigure.Row + whichTurn].Image != null)
                    {
                        if (szachownica[chosenFigure.Column - 2 * 1, chosenFigure.Row - 2 * whichTurn].Image == null)
                        {
                            szachownica[chosenFigure.Column - 2 * 1, chosenFigure.Row - 2 * whichTurn].FlatStyle = FlatStyle.Flat;
                            szachownica[chosenFigure.Column - 2 * 1, chosenFigure.Row - 2 * whichTurn].FlatAppearance.BorderSize = 5;
                            szachownica[chosenFigure.Column - 2 * 1, chosenFigure.Row - 2 * whichTurn].FlatAppearance.BorderColor = Color.Green;
                            szachownica[chosenFigure.Column - 2 * 1, chosenFigure.Row - 2 * whichTurn].IsEnabled = true;
                        }
                    }
                }
                catch { }
            }
            else
            {
                ResetPossiblePositions();
            }
            
        }

        void ResetPossiblePositions()
        {
            foreach(BoardButton position in szachownica)
            {
                position.FlatStyle = FlatStyle.Standard;
            }
        }

        private void NextTurn()
        {
            if (whichTurn == 1) whichTurn = -1;
            else if (whichTurn == -1) whichTurn = 1;
            EnableButtons();
            TurnIndicator(whichTurn);
        }


        public void EnableButtons()
        {
            if(whichTurn == 1)
            {
                foreach (BoardButton button in szachownica)
                {
                    if (button.Image == redFigure) button.IsEnabled = true;
                    else button.IsEnabled = false;
                }
            }
            else if (whichTurn == -1)
            {
                foreach (BoardButton button in szachownica)
                {
                    if (button.Image == greyFigure) button.IsEnabled = true;
                    else button.IsEnabled = false;
                }
            }

        }

        public void ExecuteFigure(BoardButton figureToExecute)
        {
            figureToExecute.Image = null;
            if (whichTurn == 1) redPoints += 1;
            else if (whichTurn == -1) greyPoints += 1;
            UpdatePoints(redPoints, greyPoints);
        }

        public void UpdatePoints(int redPoints, int greyPoints)
        {
            redPointsLabel.Text = $"Red player: {redPoints} point(s)";
            greyPointsLabel.Text = $"Grey player: {greyPoints} point(s)";
        }

        public void TurnIndicator(int whichTurn)
        {
            if(whichTurn == 1)
            {
                turnIndicatorLabel.Text = "RED PLAYER";
            }
            else if (whichTurn == -1)
            {
                turnIndicatorLabel.Text = "GREY PLAYER";
            }
        }

        public bool IsSomebodyToBeat(BoardButton chosenFigure)
        {
            try
            {
                if (szachownica[chosenFigure.Column + whichTurn, chosenFigure.Row + whichTurn].Image == null)
                {
                    return false;
                }
                else if (szachownica[chosenFigure.Column + whichTurn, chosenFigure.Row + whichTurn].Image != chosenFigure.Image && szachownica[chosenFigure.Column + 2 * whichTurn, chosenFigure.Row + 2 * whichTurn].Image == null)
                {
                        return true;
                }
            }
            catch { return false; }
            try
            {
                if (szachownica[chosenFigure.Column - whichTurn, chosenFigure.Row + whichTurn].Image == null)
                {
                    return false;
                }
                else if (szachownica[chosenFigure.Column - whichTurn, chosenFigure.Row + whichTurn].Image != chosenFigure.Image && szachownica[chosenFigure.Column - 2 * whichTurn, chosenFigure.Row + 2 * whichTurn].Image == null)
                {
                    return true;
                }
            }
            catch { return false; }
            return false;
        }
    }
    
    public class BoardButton : Button
    {
        public bool IsChosen { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public bool IsEnabled { get; set; }


    }

    public class ChosenFigure ///: IDisposable
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public Image Figure { get; set; }

        public bool IsChosen { get; set; }

        public ChosenFigure(int column, int row , Image figure)
        {
            Column = column;
            Row = row;
            Figure = figure;
            IsChosen = true;
        }


        ///public IDisposable Dispose()
        ///{
        ///    return ;
        ///}
    }

}
