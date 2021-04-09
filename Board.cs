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

            boardButton.Location = new Point(boardButton.Size.Width * column, 35 + (boardButton.Size.Height * row));
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
            int startingPosition = team.GetTeamDirection();

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

            else if (boardButton.IsChosen && boardButton.IsQueen)
            {
                ValidKingsMoves(boardButton, currentTeam);
                MarkPossibleButtons();
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
            if(TeamIndicatorChecker(currentTeam) == 1 && currentButton.Row == 7)
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
                ValidMansMoves(currentPosition, currentTeam);
                MarkPossibleButtons();
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

        

        public void ValidKingsMoves(BoardButton currentButton, Team currentTeam)
        {
            foreach(BoardButton button in checkerboard)
            {
                if((button.Row - button.Column) == (currentButton.Row - currentButton.Column) || (button.Row + button.Column) == (currentButton.Row + currentButton.Column))
                {
                    if (IsFieldEmpty(button)) markedPositionsList.Add(button);
                    else if(IsFieldOccupiedByOpponent(button, currentTeam)) markedFiguresToExecuteList.Add(button);
                }
            }
        }

        public void ValidMansMoves(BoardButton currentButton, Team currentTeam)
        {
            int factor = currentTeam.GetTeamDirection();

            foreach (BoardButton button in checkerboard)
            {
                if (((button.Row - button.Column) == (currentButton.Row - currentButton.Column) || (button.Row + button.Column) == (currentButton.Row + currentButton.Column)) && Math.Abs(button.Row - currentButton.Row) == 1) //&& (button.Row - currentButton.Row) == factor) //buton.Row - currentButton.Row <= factor - współczynnik w zależności czy damka czy zwykly ma wartość 7 lub 1; Wymyślić jak to rozegrać dla kierunku poruszania się
                {
                    if (IsFieldEmpty(button) && (button.Row - currentButton.Row) == factor) markedPositionsList.Add(button);
                    else if (IsFieldOccupiedByOpponent(button, currentTeam))
                    {
                        int rowDirection = button.Row - currentButton.Row;
                        int columnDirection = button.Column - currentButton.Column;
                        rowDirection += button.Row;
                        columnDirection += button.Column;
                        
                        if(IsFieldEmpty(checkerboard[columnDirection, rowDirection]))
                            markedPositionsList.Add(checkerboard[columnDirection,rowDirection]);
                    }
                }
            }
        }

        public void MarkPossibleButtons()
        {
            foreach(BoardButton button in markedPositionsList)
            {
                EnableButton(button);
                MarkButton(button);
            }
        }

    }
}
