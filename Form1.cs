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
        private Board board;
        private BoardButton chosenButton = new BoardButton();

        

        private Team redTeam;
        private Team greyTeam;
        private Team currentTeam;

        private int teamIndicator = -1;
        private const int DEFAULT_BUTTON_SIZE_COUNT = 50;
        private const int DEFAULT_BUTTON_IMAGE_MARGIN = 15;
        private static int buttonSize = DEFAULT_BUTTON_SIZE_COUNT;

        private static Image redFigure = new Bitmap(new Bitmap(Path.Combine(Application.ExecutablePath, @"..\Assets\red.png")), new Size(buttonSize - DEFAULT_BUTTON_IMAGE_MARGIN, buttonSize - DEFAULT_BUTTON_IMAGE_MARGIN));
        private static Image greyFigure = new Bitmap(new Bitmap(Path.Combine(Application.ExecutablePath, @"..\Assets\grey.png")), new Size(buttonSize - DEFAULT_BUTTON_IMAGE_MARGIN, buttonSize - DEFAULT_BUTTON_IMAGE_MARGIN));

        public Form1()
        {
            InitializeComponent();

            NewGame();

            SetTeamPointsIndicator();
        }

        public void MoveFigure(object sender, EventArgs e)
        {
            BoardButton currentButton = (BoardButton)sender;

            if (currentButton.Image == currentTeam.figureImage && currentButton.IsEnabled && !board.IsSomeoneToExecute())
            {
                board.ClearMarkingLists();
                board.DisableAllButtons();
                board.EnableAllTeamButtons(currentTeam);
                BoardPainter.UnmarkAllButtons(board.Checkerboard);
                board.ResetIsChosenButtons();
                ChooseTheButton(currentButton);
                
                if (!currentButton.IsKing)
                    board.ValidMenMoves(currentButton, currentTeam);
                else
                    board.ValidKingsMoves(currentButton, currentTeam);

                board.EnableAllValidButtons();
            }
            else if (chosenButton.IsChosen && currentButton.IsEnabled && currentButton.Image == null)
            {
                if(chosenButton.Column != currentButton.Column)
                {
                    board.MakeMove(chosenButton, currentButton, currentTeam);
                    UpdatePoints();
                }

                board.ClearMarkingLists();
                
                if (!board.FightChecker(currentTeam))
                {
                    chosenButton.IsChosen = false;
                    NextTurn();
                }
                else
                {
                    ChooseTheButton(currentButton);
                }
            }
            else if (currentButton.Image == currentTeam.figureImage && currentButton.IsEnabled && board.IsSomeoneToExecute())
            {
                board.ClearMarkingLists();
                board.DisableValidExecutionMovesButtons();
                
                BoardPainter.UnmarkAllGreenButtons(board.Checkerboard);
                ChooseTheButton(currentButton);

                if (!currentButton.IsKing)
                    board.ValidMenMoves(currentButton, currentTeam);
                else
                    board.ValidKingsMoves(currentButton, currentTeam);

                board.ClearValidPositionsList();
                board.EnableAllValidButtons();
            }
            else if (!chosenButton.IsChosen) 
                MessageBox.Show("Choose the valid figure");
        }

        private void ChooseTheButton(BoardButton chosenButton)
        {
            this.chosenButton = chosenButton;
            this.chosenButton.IsChosen = true;
        }

        private void ChangeCurrentTeam()
        {
            teamIndicator *= -1;
            if (teamIndicator == 1)
            {
                currentTeam = redTeam;
            }
            else 
            { 
                currentTeam = greyTeam;
            }
        }

        private void NextTurn()
        {
            ChangeCurrentTeam();

            chosenButton.IsChosen = false;
            board.DisableAllButtons();
            BoardPainter.UnmarkAllButtons(board.Checkerboard);
            UpdateCurrentPlayerLabel();
            
            board.EnableAllTeamButtons(currentTeam);
            board.FightChecker(currentTeam);

            IsGameOver();
        }

        private void NewGame()
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
            BoardPainter.UnmarkAllButtons(board.Checkerboard);
            board.EnableAllTeamButtons(currentTeam);
            UpdateCurrentPlayerLabel();
        }

        private void ResetTeamIndicator()
        {
            teamIndicator = -1;
        }

        private void UpdatePoints()
        {
            greysTeamPointIndicator.Text = $"{greyTeam.Points}";
            redsTeamPointsIndicator.Text = $"{redTeam.Points}";
        }

        private void UpdateCurrentPlayerLabel()
        {
            if (teamIndicator == 1)
            {
                currentTeam = redTeam;
                redTurnIndicatorLabel.Visible = true;
                greyTurnIndicatorLabel.Visible = false;
            }
            else
            {
                currentTeam = greyTeam;
                redTurnIndicatorLabel.Visible = false;
                greyTurnIndicatorLabel.Visible = true;
            }
        }

        private bool IsGameOver()
        {
            int maxPoints = (board.Columns / 2) * 3;
            if(greyTeam.Points == maxPoints)
            {
                MessageBox.Show($"Grey Team wins!" +
                    $"\nCongratulation!");
                return true;
            }
            else if (redTeam.Points == maxPoints)
            {
                MessageBox.Show($"Red Team wins!" +
                    $"\nCONGRATULATIONS!");
                return true;
            }
            else if(board.validExecutionMovesList.Count == 0 && board.validPositionsList.Count == 0)
            {
                MessageBox.Show($"{currentTeam.Name} draws");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetTeamPointsIndicator()
        {
            greysTeamPointIndicator.Image = new Bitmap(new Bitmap(Path.Combine(Application.ExecutablePath, @"..\Assets\grey.png")), new Size(greysTeamPointIndicator.Size.Height - 15, greysTeamPointIndicator.Size.Width - 15));
            redsTeamPointsIndicator.Image = new Bitmap(new Bitmap(Path.Combine(Application.ExecutablePath, @"..\Assets\red.png")), new Size(greysTeamPointIndicator.Size.Height - 15, greysTeamPointIndicator.Size.Width - 15));
        }

        private void newGameMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
            SetTeamPointsIndicator();
        }
    }
}
