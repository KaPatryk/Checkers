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
        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public int ButtonSize { get; private set; }

        private BoardButton[,] checkerboard;
        private List<BoardButton> markedPositionsList = new List<BoardButton>();
        private List<BoardButton> markedFiguresToExecuteList = new List<BoardButton>();


        public Board(Form1 form, int boardSize, int buttonSize, Team firstTeam, Team secondTeam)
        {
            ButtonSize = buttonSize;
            Columns = boardSize;
            Rows = boardSize;
            checkerboard = new BoardButton[Columns, Rows];

            BuildBoard(Columns, Rows, ButtonSize, form);
            ResetPlayerPositions(checkerboard, firstTeam);
            ResetPlayerPositions(checkerboard, secondTeam);
        }

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

        public BoardButton[,] GetCheckerboard()
        {
            return checkerboard;
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

        private void AddButtonClickTo_EventHandler(BoardButton boardButton, Form1 form)
        {
            boardButton.Click += new System.EventHandler(form.MoveFigure);
        }

        private BoardButton CreateNewButton(int buttonSize)
        {
            BoardButton boardButton = new BoardButton();
            boardButton.Size = new Size(buttonSize, buttonSize);

            return boardButton;
        }

        private void PlaceButton(BoardButton boardButton, int column, int row)
        {

            boardButton.Location = new Point(boardButton.Size.Width * column, boardButton.Size.Height * row);
            boardButton.SetPosition(column, row);

        }

        private void SetButtonBackColor(BoardButton boardButton, int column, int row)
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

        public void ValidMoves(BoardButton boardButton, Team currentTeam)
        {
            int teamIndicator = TeamIndicatorChecker(currentTeam);

            if (boardButton.IsChosen && !boardButton.IsQueen)
            {
                for (int row = (boardButton.Row - 1); row <= (boardButton.Row + 1); row++)
                {
                    for (int column = (boardButton.Column - 1); column <= (boardButton.Column + 1); column++)
                    {
                        if (boardButton.Column != column && boardButton.Row != row)
                        {
                            try
                            {
                                if (IsFieldEmpty(checkerboard[column, row]) && (boardButton.Row - row) == -teamIndicator)
                                {
                                    EnableButton(checkerboard[column, row]);
                                    MarkButton(checkerboard[column, row]);
                                }
                            }
                            catch { }

                        }
                    }
                }
            }

            else if(boardButton.IsChosen && boardButton.IsQueen)
            {

                for (int column = boardButton.Column, row = boardButton.Row; column >= 0 && row >= 0; column--, row--)
                {
                    try
                    {
                        if (IsFieldEmpty(checkerboard[column, row]))
                        {
                            EnableButton(checkerboard[column, row]);
                            MarkButton(checkerboard[column, row]);
                        }
                        else if (!IsFieldEmpty(checkerboard[column, row]) && !IsFieldEmpty(checkerboard[column - 1, row - 1]))
                            break;
                        else if (!IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && !IsFieldEmpty(checkerboard[column - 1, row - 1]))
                        {
                            break;
                        }
                        else if (!IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam))
                        {
                            break;
                        }
                    }
                    catch { }
                    
                }
                for (int column = boardButton.Column, row = boardButton.Row; column >= 0 && row < 8; column--, row++)
                {
                    try
                    {
                        if (IsFieldEmpty(checkerboard[column, row]))
                        {
                            EnableButton(checkerboard[column, row]);
                            MarkButton(checkerboard[column, row]);
                        }
                        else if (!IsFieldEmpty(checkerboard[column, row]) && !IsFieldEmpty(checkerboard[column - 1, row + 1]))
                        {
                            break; 
                        }
                        else if (!IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && !IsFieldEmpty(checkerboard[column - 1, row + 1]))
                        {
                            break;
                        }
                        else if (!IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam))
                        {
                            break;
                        }
                    }
                    catch { }
                    
                }
                for (int column = boardButton.Column, row = boardButton.Row; column < 8 && row < 8; column++, row++)
                {
                    try
                    {
                        if (IsFieldEmpty(checkerboard[column, row]))
                        {
                            EnableButton(checkerboard[column, row]);
                            MarkButton(checkerboard[column, row]);
                        }
                        else if (!IsFieldEmpty(checkerboard[column, row]) && !IsFieldEmpty(checkerboard[column + 1, row + 1])) break;
                        else if (!IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && !IsFieldEmpty(checkerboard[column + 1, row + 1]))
                        {
                            break;
                        }
                        else if (!IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam))
                        {
                            break;
                        }
                    }
                    catch { }
                    
                }
                for(int column = boardButton.Column, row = boardButton.Row; column < 8 && row >= 0; column++, row--)
                    {
                    try
                    {
                        if (IsFieldEmpty(checkerboard[column, row]))
                        {
                            EnableButton(checkerboard[column, row]);
                            MarkButton(checkerboard[column, row]);
                        }
                        else if (!IsFieldEmpty(checkerboard[column, row]) && !IsFieldEmpty(checkerboard[column + 1, row - 1])) break;
                        else if (!IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && !IsFieldEmpty(checkerboard[column + 1, row - 1]))
                        {
                            break;
                        }
                        else if (!IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam))
                        {
                            break;
                        }
                    }
                    catch { }
                }
            }

        }


        public bool IsFieldEmpty(BoardButton boardButton)
        {
            if (boardButton.Image == null && boardButton.BackColor != Color.White)
                return true;
            else
                return false;
        }

        public bool IsFieldOccupiedByOpponent(BoardButton boardButton, Team currentTeam)
        {
            if (!IsFieldEmpty(boardButton) && boardButton.Image != currentTeam.figureImage)
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

        private void MakeTheQueen(BoardButton currentButton, Team currentTeam)
        {
            if(TeamIndicatorChecker(currentTeam)==1 && currentButton.Row == 7)
                currentButton.IsQueen = true;
            else if (TeamIndicatorChecker(currentTeam) == -1 && currentButton.Row == 0)
                currentButton.IsQueen = true;
        }

        public void MakeMove(BoardButton oldPosition, BoardButton newPosition, Team currentTeam)
        {
            if (newPosition.IsEnabled)
            {
                newPosition.Image = currentTeam.figureImage;
                newPosition.IsQueen = oldPosition.IsQueen;
                MakeTheQueen(newPosition, currentTeam);
                oldPosition.Image = null;
                UnmarkAllButtons();
                DisableAllButtons();
                if (Execution(newPosition))
                {
                    currentTeam.AddPoint();
                    newPosition.IsEnabled = true;
                    newPosition.IsChosen = true;
                }
                ClearMarkingLists();
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

            PickUpButton(currentPosition, currentTeam);

            if (currentPosition.IsChosen && currentPosition.IsQueen)
            {
                for (int column = currentPosition.Column, row = currentPosition.Row; column >= 0 && row >= 0; column--, row--)
                {
                    try
                    {
                        if (IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && IsFieldEmpty(checkerboard[column - 1, row - 1]))
                        {
                            EnableButton(checkerboard[column - 1, row - 1]);
                            MarkButton(checkerboard[column - 1, row - 1]);
                            markedPositionsList.Add(checkerboard[column - 1, row - 1]);
                            markedFiguresToExecuteList.Add(checkerboard[column, row]);
                            isSomeoneToExecute = true;

                            break;
                        }
                        else if (IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && !IsFieldEmpty(checkerboard[column - 1, row - 1]))
                        {
                            break;
                        }
                    }
                    catch { }

                }
                for (int column = currentPosition.Column, row = currentPosition.Row; column >= 0 && row < 8; column--, row++)
                {
                    try
                    {
                        if (IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && IsFieldEmpty(checkerboard[column - 1, row + 1]))
                        {
                            EnableButton(checkerboard[column - 1, row + 1]);
                            MarkButton(checkerboard[column - 1, row + 1]);
                            markedPositionsList.Add(checkerboard[column - 1, row + 1]);
                            markedFiguresToExecuteList.Add(checkerboard[column, row]);
                            isSomeoneToExecute = true;

                            break;
                        }
                        else if (IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && !IsFieldEmpty(checkerboard[column - 1, row + 1]))
                        {
                            break;
                        }
                    }
                    catch { }

                }
                for (int column = currentPosition.Column, row = currentPosition.Row; column < 8 && row < 8; column++, row++)
                {
                    try
                    {
                        if (IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && IsFieldEmpty(checkerboard[column + 1, row + 1]))
                        {
                            EnableButton(checkerboard[column + 1, row + 1]);
                            MarkButton(checkerboard[column + 1, row + 1]);
                            markedPositionsList.Add(checkerboard[column + 1, row + 1]);
                            markedFiguresToExecuteList.Add(checkerboard[column, row]);
                            isSomeoneToExecute = true;
                            break;
                        }
                        else if (IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && !IsFieldEmpty(checkerboard[column + 1, row + 1]))
                        {
                            break;
                        }
                    }
                    catch { }

                }
                for (int column = currentPosition.Column, row = currentPosition.Row; column < 8 && row >= 0; column++, row--)
                {
                    try
                    {
                        if (IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && IsFieldEmpty(checkerboard[column + 1, row - 1]))
                        {
                            EnableButton(checkerboard[column + 1, row - 1]);
                            MarkButton(checkerboard[column + 1, row - 1]);
                            markedPositionsList.Add(checkerboard[column + 1, row - 1]);
                            markedFiguresToExecuteList.Add(checkerboard[column, row]);
                            isSomeoneToExecute = true;
                            break;
                        }
                        else if (IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && !IsFieldEmpty(checkerboard[column + 1, row - 1]))
                        {
                            break;
                        }
                    }
                    catch { }
                }
            }

            else if (currentPosition.IsChosen && !currentPosition.IsQueen)
            {
                for (int row = (currentPosition.Row - 1); row <= (currentPosition.Row + 1); row++)
                {
                    for (int column = (currentPosition.Column - 1); column <= (currentPosition.Column + 1); column++)
                    {
                        if (currentPosition.Column != column && currentPosition.Row != row)
                        {
                            try
                            {
                                if (IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && IsFieldEmpty(checkerboard[column + (column - currentPosition.Column), row + (row - currentPosition.Row)]))
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
