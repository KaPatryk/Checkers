/*using System;
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
    class MovesChecker
    {
        public List<BoardButton> validPositionsList { get; private set; } = new List<BoardButton>();
        public List<BoardButton> validExecutionMovesList { get; private set; } = new List<BoardButton>();
        private List<BoardButton> validFiguresToMoveList { get; set; } = new List<BoardButton>();

        public FigureExecutioner figureExecutioner { get; set; } = new FigureExecutioner();

        public BoardButton [,] Checkerboard 
        { 
            get 
            { 
                return Board.Checkerboard; 
            } 
            private set 
            { 

            } 
        }

        public void ValidMenMoves(BoardButton currentButton, Team currentTeam)
        {
            int factor = currentTeam.GetTeamDirection();

            foreach (BoardButton button in Checkerboard)
            {
                if (((button.Row - button.Column) == (currentButton.Row - currentButton.Column) || (button.Row + button.Column) == (currentButton.Row + currentButton.Column)) && Math.Abs(button.Row - currentButton.Row) == 1) //&& (button.Row - currentButton.Row) == factor) //buton.Row - currentButton.Row <= factor - współczynnik w zależności czy damka czy zwykly ma wartość 7 lub 1; Wymyślić jak to rozegrać dla kierunku poruszania się
                {
                    if (IsFieldEmpty(button) && (button.Row - currentButton.Row) == factor) validPositionsList.Add(button);
                    else if (IsFieldOccupiedByOpponent(button, currentTeam))
                    {
                        int rowDirection = button.Row - currentButton.Row;
                        int columnDirection = button.Column - currentButton.Column;
                        try
                        {
                            if (IsFieldEmpty(Board.Checkerboard[button.Column + columnDirection, button.Row + rowDirection]))
                            {

                                figureExecutioner.AddFigureToExecutionList(Checkerboard[button.Column, button.Row]);
                                validExecutionMovesList.Add(Checkerboard[button.Column + columnDirection, button.Row + rowDirection]);
                                validFiguresToMoveList.Add(currentButton);
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        public bool IsSomeoneToExecute()
        {
            return validExecutionMovesList.Count > 0;
        }

        public void ValidKingsMoves(BoardButton currentButton, Team currentTeam)
        {
            validFiguresToMoveList.Add(currentButton);

            Path(currentButton, currentTeam, 1, -1, 0);
            Path(currentButton, currentTeam, -1, 1, 0);
            Path(currentButton, currentTeam, -1, -1, 0);
            Path(currentButton, currentTeam, 1, 1, 0);
        }

        enum Piece
        {
            Man = -1,
            King = 0
        }

        public void Path(BoardButton currentButton, Team currentTeam, int columnFactor, int rowFactor, int status)
        {
            int row = currentButton.Row + rowFactor;
            int column = currentButton.Column + columnFactor;

            try
            {
                if (IsFieldEmpty(Checkerboard[column, row]))
                {
                    if (status == 0)
                    {
                        validPositionsList.Add(Checkerboard[column, row]);
                    }
                    else if (status == 1)
                    {
                        validExecutionMovesList.Add(Checkerboard[column, row]);
                    }

                    Path(Checkerboard[column, row], currentTeam, columnFactor, rowFactor, status);
                }
                else if (IsFieldOccupiedByOpponent(Checkerboard[column, row], currentTeam) && status == 0 && IsFieldEmpty(Checkerboard[column + columnFactor, row + rowFactor]))
                {
                    figureExecutioner.AddFigureToExecutionList(Checkerboard[column, row]);
                    validPositionsList.Clear();

                    status++;
                    Path(Checkerboard[column, row], currentTeam, columnFactor, rowFactor, status);
                }
            }
            catch { }
        }

        public bool FightChecker(Team currentTeam)
        {
            bool fightIndicator = false;

            foreach (BoardButton button in Checkerboard)
            {
                if (button.IsEnabled)
                {
                    if (!button.IsKing)
                        ValidMenMoves(button, currentTeam);
                    else
                        ValidKingsMoves(button, currentTeam);
                }
            }

            if (figureExecutioner.IsFigureToExecute())
            {
                fightIndicator = true;

                DisableAllButtons();
                EnableFiguresToMove();
                validPositionsList.Clear();

                foreach (BoardButton button in figureExecutioner.figuresToExecuteList)
                {
                    EnableButton(button);
                    BoardPainter.MarkButton(button, Color.Red);
                }
            }

            return fightIndicator;
        }

        public bool IsFieldEmpty(BoardButton boardButton)
        {
            return boardButton.Image == null && boardButton.BackColor != Color.White;
        }

        public bool IsFieldOccupiedByOpponent(BoardButton boardButton, Team currentTeam)
        {
            return IsFieldEmpty(boardButton) == false && boardButton.Image != currentTeam.figureImage;
        }

        public void MakeMove(BoardButton oldPosition, BoardButton newPosition, Team currentTeam)
        {
            if (newPosition.IsEnabled)
            {
                newPosition.Image = oldPosition.Image;
                newPosition.IsKing = oldPosition.IsKing;
                MakeTheKing(newPosition, currentTeam);
                TryToSetKingButtonParameters(oldPosition, newPosition);
                oldPosition.Image = null;
                BoardPainter.UnmarkAllButtons(checkerboard);
                DisableAllButtons();
                if (figureExecutioner.Execution(oldPosition, newPosition))
                {
                    currentTeam.AddPoint();
                    newPosition.IsEnabled = true;
                    newPosition.IsChosen = true;
                }
                ClearMarkingLists();
            }
        }
    }
}
*/