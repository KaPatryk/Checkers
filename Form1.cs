using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Checkers
{
    
    public partial class Form1 : Form
    {
        Board board;
        Team redTeam;
        Team greyTeam;
        Team currentTeam;
        int teamIndicator = -1;

        static int buttonSize = 50;
        static Image redFigure = new Bitmap(new Bitmap(Path.Combine(Application.ExecutablePath, @"..\Assets\red.png")), new Size(buttonSize - 15, buttonSize - 15));
        static Image greyFigure = new Bitmap(new Bitmap(Path.Combine(Application.ExecutablePath, @"..\Assets\grey.png")), new Size(buttonSize - 15, buttonSize - 15));

        BoardButton chosenButton = new BoardButton();

        public Form1()
        {
            InitializeComponent();
            NewGame();
        }

        public void MoveFigure(object sender, EventArgs e)
        {
            BoardButton currentButton = (BoardButton)sender;

            if (chosenButton.IsChosen && currentButton.IsEnabled && currentButton.Image == null)
            {
                board.MakeMove(chosenButton, currentButton, currentTeam);
                UpdatePoints();

                if ((chosenButton.Row == (currentButton.Row - 1)) || (chosenButton.Row == (currentButton.Row + 1)))
                {
                    NextTurn();
                }

                else if (!board.PossibleFights(currentButton, currentTeam))
                {
                    NextTurn();
                }
                else
                {
                    chosenButton = currentButton;
                }

            }
            else if (currentButton.Image == currentTeam.figureImage && currentButton.IsEnabled)
            {
                board.DisableAllButtons();
                board.EnableAllTeamButtons(currentTeam);
                board.ClearMarkingLists();
                board.ResetIsChosenButtons();
                chosenButton = currentButton;
                board.UnmarkAllButtons();
                board.PickUpButton(currentButton, currentTeam);
                if (!board.PossibleFights(currentButton, currentTeam))
                    board.ValidMoves(currentButton, currentTeam);
            }
        }

        public void ChangeCurrentTeam()
        {
            teamIndicator *= -1;
            if (teamIndicator == 1) currentTeam = redTeam;
            else currentTeam = greyTeam;
        }

        public void NextTurn()
        {
            ChangeCurrentTeam();

            board.DisableAllButtons();
            board.UnmarkAllButtons();
            board.EnableAllTeamButtons(currentTeam);
            UpdateCurrentPlayerIcon();
            IsGameOver();
        }

        public void NewGame()
        {
            this.Controls.Clear();
            
            redTeam = new Team("Red", redFigure, "Up");
            greyTeam = new Team("Grey", greyFigure, "Down");
            board = new Board(this, greyTeam, redTeam);

            currentTeam = greyTeam;

            InitializeComponent();

            ResetTeamIndicator();
            UpdatePoints();
            board.DisableAllButtons();
            board.UnmarkAllButtons();
            board.EnableAllTeamButtons(currentTeam);
            UpdateCurrentPlayerIcon();
        }

        private void ResetTeamIndicator()
        {
            teamIndicator = -1;
        }

        public void UpdatePoints()
        {
            greyPointsLabel.Text = $"GreyTeam has {greyTeam.Points} points";
            redPointsLabel.Text = $"RedTeam has {redTeam.Points} points";
        }

        public void UpdateCurrentPlayerIcon()
        {
            turnIndicatorLabel.Text = $"{currentTeam.Name}Team's turn";
        }

        public bool IsGameOver()
        {
            int teamFiguresQty = 0;
            foreach (BoardButton button in board.GetCheckerboard())
            {
                if (button.Image == currentTeam.figureImage)
                {
                    teamFiguresQty++;
                }
            }
            if (teamFiguresQty == 0)
            {
                MessageBox.Show($"{currentTeam.Name}Team is the looser!");
                return true;
            }
            else
                return false;
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            NewGame();
        }
    }
}
