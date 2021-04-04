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
    public class Board
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int ButtonSize { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public BoardButton[,] checkerboard;
        public List<BoardButton> markedPositionsList = new List<BoardButton>();
        public List<BoardButton> markedFiguresToExecuteList = new List<BoardButton>();


        public Board(Form1 form, Team firstTeam, Team secondTeam)
        {
            ButtonSize = 50;
            Columns = 8;
            Rows = 8;
            checkerboard = new BoardButton[Columns, Rows];

            BuildBoard(Columns, Rows, ButtonSize, form);
            ResetPlayerPositions(checkerboard, firstTeam);
            ResetPlayerPositions(checkerboard, secondTeam);
        }

        enum StartingPosition
        {
            Up = 1,
            Down = -1
        }

        public void BuildBoard(int columns, int rows, int buttonSize, Form1 form)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    checkerboard[column, row] = CreateNewButton(buttonSize);
                    PlaceButton(checkerboard[column, row], column, row);
                    SetButtonBackColor(checkerboard[column, row], column, row);

                    form.Controls.Add(checkerboard[column, row]);
                    AddButtonClickTo_EventHandler(checkerboard[column, row], form);
                }
            }
        }

        public void AddButtonClickTo_EventHandler(BoardButton boardButton, Form1 form)
        {
            boardButton.Click += new System.EventHandler(form.MoveFigure);
        }

        public BoardButton CreateNewButton(int buttonSize)
        {
            BoardButton boardButton = new BoardButton();
            boardButton.Size = new Size(buttonSize, buttonSize);

            return boardButton;
        }

        public void PlaceButton(BoardButton boardButton, int column, int row)
        {

            boardButton.Location = new Point(boardButton.Size.Width * column, boardButton.Size.Height * row);
            boardButton.Column = column;
            boardButton.Row = row;

        }

        public void SetButtonBackColor(BoardButton boardButton, int column, int row)
        {
            if (row % 2 == 0 && column % 2 == 0) boardButton.BackColor = Color.White;
            else if (row % 2 != 0 && column % 2 == 0) boardButton.BackColor = Color.Black;
            else if (row % 2 != 0 && column % 2 != 0) boardButton.BackColor = Color.White;
            else if (row % 2 == 0 && column % 2 != 0) boardButton.BackColor = Color.Black;

        }

        public void ResetPlayerPositions(BoardButton[,] checkerboard, Team team)
        {
            int startingPosition = team.GetStartingPosition();

            switch (startingPosition)
            {
                case (int)StartingPosition.Up:

                    for (int row = 0; row < 3; row++)
                    {
                        for (int column = 0; column < Columns; column++)
                        {
                            if (row % 2 == 0 && column % 2 != 0) checkerboard[column, row].Image = team.figureImage;
                            else if (row % 2 != 0 && column % 2 == 0) checkerboard[column, row].Image = team.figureImage;
                        }
                    }

                    break;

                case (int)StartingPosition.Down:

                    for (int row = (Rows - 3); row < Rows; row++)
                    {
                        for (int column = 0; column < Columns; column++)
                        {
                            if (row % 2 == 0 && column % 2 != 0) checkerboard[column, row].Image = team.figureImage;
                            else if (row % 2 != 0 && column % 2 == 0) checkerboard[column, row].Image = team.figureImage;
                        }
                    }

                    break;
            }
        }

        public void PickUpButton(BoardButton boardButton, Team currentTeam)
        {
            ResetIsChosenButtons();

            if (boardButton.Image != null && boardButton.Image == currentTeam.figureImage)
                boardButton.IsChosen = true;

        }

        public void ResetIsChosenButtons()
        {
            foreach (BoardButton button in checkerboard)
                button.IsChosen = false;
        }

        public void PossibleMoves(BoardButton boardButton, Team currentTeam)
        {
            int teamIndicator = TeamIndicatorChecker(currentTeam);


            if (boardButton.IsChosen)
            {
                for (int row = (boardButton.Row - 1); row <= (boardButton.Row + 1); row++)
                {
                    for (int column = (boardButton.Column - 1); column <= (boardButton.Column + 1); column++)
                    {
                        if (boardButton.Column != column && boardButton.Row != row)
                        {
                            try
                            {
                                if (isFieldEmpty(checkerboard[column, row]) && (boardButton.Row - row) == -teamIndicator)
                                {
                                    EnableButton(checkerboard[column, row]);
                                    MarkButton(checkerboard[column, row]);
                                }
                               /* else if (isFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && isFieldEmpty(checkerboard[column + (column - boardButton.Column), row + (row - boardButton.Row)]))
                                {
                                    EnableButton(checkerboard[column + (column - boardButton.Column), row + (row - boardButton.Row)]);
                                    MarkButton(checkerboard[column + (column - boardButton.Column), row + (row - boardButton.Row)]);
                                    markedPositionsList.Add(checkerboard[column + (column - boardButton.Column), row + (row - boardButton.Row)]);
                                    markedFiguresToExecuteList.Add(checkerboard[column, row]);
                                }*/

                            }
                            catch { }

                        }
                    }
                }
            }

        }



        public bool isFieldEmpty(BoardButton boardButton)
        {
            if (boardButton.Image == null && boardButton.BackColor != Color.White)
                return true;
            else
                return false;
        }

        public bool isFieldOccupiedByOpponent(BoardButton boardButton, Team currentTeam)
        {
            if (!isFieldEmpty(boardButton) && boardButton.Image != currentTeam.figureImage)
                return true;
            else
                return false;
        }

        public void EnableButton(BoardButton boardButton)
        {
            boardButton.IsEnabled = true;
        }

        public void EnableAllTeamButtons(Team currentTeam)
        {
            foreach (BoardButton button in checkerboard)
            {
                if (button.Image == currentTeam.figureImage)
                    button.IsEnabled = true;
            }
        }

        public void DisableAllButtons()
        {
            foreach (BoardButton button in checkerboard)
                button.IsEnabled = false;
        }

        public void DisableButton(BoardButton boardButton)
        {
            boardButton.IsEnabled = false;
        }

        public void MarkButton(BoardButton boardButton)
        {
            boardButton.FlatStyle = FlatStyle.Flat;
            boardButton.FlatAppearance.BorderSize = 5;
            boardButton.FlatAppearance.BorderColor = Color.Green;

        }

        public void UnmarkButton(BoardButton boardButton)
        {
            boardButton.FlatStyle = FlatStyle.Standard;

        }

        public void UnmarkAllButtons()
        {
            foreach (BoardButton button in checkerboard) button.FlatStyle = FlatStyle.Standard;
        }

        public void NewMove(Team currentTeam)
        {
            EnableAllTeamButtons(currentTeam);
        }

        public void MakeMove(BoardButton oldPosition, BoardButton newPosition, Team currentTeam)
        {
            if (newPosition.IsEnabled)
            {
                newPosition.Image = currentTeam.figureImage;
                oldPosition.Image = null;
                UnmarkAllButtons();
                DisableAllButtons();
                if (Execution(newPosition))
                {
                    currentTeam.AddPoint();
                    newPosition.IsEnabled = true;
                    newPosition.IsChosen = true;
                }
                    
                

            }
        }

        public bool Execution(BoardButton newPosition)
        {
            for (int i = 0; i < markedPositionsList.Count; i++)
            {
                if (newPosition == markedPositionsList[i])
                {
                    ExecuteOpponentsFigure(markedFiguresToExecuteList[i]);
                    return true;
                }
            }
            return false;
        }

        public void ExecuteOpponentsFigure(BoardButton figureToExecute)
        {
            figureToExecute.Image = null;
            ClearMarkingLists();
        }

        public void ClearMarkingLists()
        {
            markedPositionsList.Clear();
            markedFiguresToExecuteList.Clear();
        }

        public int TeamIndicatorChecker(Team currentTeam)
        {
            if (currentTeam.Name == "Red")
                return 1;
            else
                return -1;
        }

        public bool PossibleFights(BoardButton currentPosition, Team currentTeam)
        {
            int teamIndicator = TeamIndicatorChecker(currentTeam);
            bool isSomeoneToExecute = false;
            if (currentPosition.IsChosen)
            {
                for (int row = (currentPosition.Row - 1); row <= (currentPosition.Row + 1); row++)
                {
                    for (int column = (currentPosition.Column - 1); column <= (currentPosition.Column + 1); column++)
                    {
                        if (currentPosition.Column != column && currentPosition.Row != row)
                        {
                            try
                            {
                                if (isFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && isFieldEmpty(checkerboard[column + (column - currentPosition.Column), row + (row - currentPosition.Row)]))
                                {
                                    EnableButton(checkerboard[column + (column - currentPosition.Column), row + (row - currentPosition.Row)]);
                                    MarkButton(checkerboard[column + (column - currentPosition.Column), row + (row - currentPosition.Row)]);
                                    markedPositionsList.Add(checkerboard[column + (column - currentPosition.Column), row + (row - currentPosition.Row)]);
                                    markedFiguresToExecuteList.Add(checkerboard[column, row]);
                                    isSomeoneToExecute = true;
                                }
                            }
                            catch { }

                        }
                    }
                }
            }

            return isSomeoneToExecute;
        }
    }
}
