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
       class Game
        {
            Board board;
            Team redTeam;
            Team greyTeam;
            Team currentTeam;
            int teamIndicator = -1;

            static int buttonSize = 50;
            static Image redFigure = new Bitmap(new Bitmap("C:\\Users\\megak\\source\\repos\\Checkers\\Checkers\\Assets\\red.png"), new Size(buttonSize - 15, buttonSize - 15));
            static Image greyFigure = new Bitmap(new Bitmap("C:\\Users\\megak\\source\\repos\\Checkers\\Checkers\\Assets\\grey.png"), new Size(buttonSize - 15, buttonSize - 15));

            BoardButton chosenButton = new BoardButton();

            public void MoveFigure(object sender, EventArgs e)
            {
                BoardButton currentButton = (BoardButton)sender;

                if (chosenButton.IsChosen && currentButton.IsEnabled && currentButton.Image == null)
                {
                    board.MakeMove(chosenButton, currentButton, currentTeam);

                    if (!IsSomeoneElseToExecute(currentButton, currentTeam))
                        NextTurn();
                }
                else if (currentButton.Image == currentTeam.figureImage)
                {
                    board.DisableAllButtons();
                    board.EnableAllTeamButtons(currentTeam);
                    board.ResetIsChosenButtons();
                    chosenButton = currentButton;
                    board.UnmarkAllButtons();
                    board.PickUpButton(currentButton, currentTeam);
                    board.PossibleMoves(currentButton, currentTeam);
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
            }

            public void NewGame()
            {

            }

            public void UpdatePoints()
            {

            }

            public void UpdateCurrentPlayerIcon()
            {

            }

            public bool GameOver()
            {
                return false;
            }

            public bool IsSomeoneElseToExecute(BoardButton newPosition, Team currentTeam)
            {
                board.PossibleMoves(newPosition, currentTeam);

                if (board.markedFiguresToExecuteList == null)
                {
                    return false;
                }
                else
                {
                    board.EnableButton(newPosition);
                    return true;
                }

            }


        }
    }
