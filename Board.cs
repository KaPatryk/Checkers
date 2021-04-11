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
        private int Columns { get; set; }
        private int Rows { get; set; }
        private int ButtonSize { get; set; }

        private const int DEFAULT_COLUMN_COUNT = 8;
        private const int DEFAULT_ROW_COUNT = 8;
        private const int DEFAULT_BUTTON_SIZE_COUNT = 50;

        public BoardButton[,] checkerboard { get; private set; }

        private List<BoardButton> validPositionsList { get; set; } = new List<BoardButton>();
        private List<BoardButton> figuresToExecuteList { get; set; } = new List<BoardButton>();
        private List<BoardButton> validExecutionMovesList { get; set; } = new List<BoardButton>();
        private List<BoardButton> validFiguresToMoveList { get; set; } = new List<BoardButton>();

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
            ButtonSize = DEFAULT_BUTTON_SIZE_COUNT;
            Columns = DEFAULT_COLUMN_COUNT;
            Rows = DEFAULT_ROW_COUNT;
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
            if (IsEven(row) && IsEven(column))
            {
                boardButton.BackColor = Color.White;
            }
            else if (!IsEven(row) && IsEven(column))
            {
                boardButton.BackColor = Color.Black;
            }
            else if (!IsEven(row) && !IsEven(column))
            {
                boardButton.BackColor = Color.White;
            }
            else if (IsEven(row) && !IsEven(column)) 
            { 
                boardButton.BackColor = Color.Black; 
            }
        }

        private bool IsEven(int number)
        {
            return number % 2 == 0;
        }

        public void ResetPlayerPositions(BoardButton[,] checkerboard, Team team)
        {
            int startingPosition = team.GetTeamDirection();

            switch (startingPosition)
            {
                case (int)StartingPosition.Up:

                    for (int row = 0; row < 3; row++)
                    {
                        ResetColumnPositions(row, team);
                    }
                    break;

                case (int)StartingPosition.Down:

                    for (int row = (Rows - 3); row < Rows; row++)
                    {
                        ResetColumnPositions(row, team);
                    }
                    break;
            }
        }

        private void ResetColumnPositions(int row, Team team)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (row % 2 == 0 && column % 2 != 0) checkerboard[column, row].Image = team.figureImage;
                else if (row % 2 != 0 && column % 2 == 0) checkerboard[column, row].Image = team.figureImage;
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

        public bool IsFieldEmpty(BoardButton boardButton)
        {
            return boardButton.Image == null && boardButton.BackColor != Color.White;
        }

        public bool IsFieldOccupiedByOpponent(BoardButton boardButton, Team currentTeam)
        {
            return IsFieldEmpty(boardButton) == false && boardButton.Image != currentTeam.figureImage;
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
                {
                    button.IsEnabled = true;
                }
            }
        }

        public void DisableAllButtons()
        {
            foreach (BoardButton button in checkerboard)
            {
                button.IsEnabled = false;
            }
        }

        public void DisableValidExecutionMovesButtons()
        {
            foreach (BoardButton button in validExecutionMovesList)
            {
                button.IsEnabled = false;
            }
        }

        public void MarkButton(BoardButton boardButton)
        {
            boardButton.FlatStyle = FlatStyle.Flat;
            boardButton.FlatAppearance.BorderSize = 5;
            boardButton.FlatAppearance.BorderColor = Color.Green;
        }

        public void MarkButton(BoardButton boardButton, Color color)
        {
            boardButton.FlatStyle = FlatStyle.Flat;
            boardButton.FlatAppearance.BorderSize = 5;
            boardButton.FlatAppearance.BorderColor = color;
        }

        public void UnmarkButton(BoardButton boardButton)
        {
            boardButton.FlatStyle = FlatStyle.Standard;
        }

        public void UnmarkAllButtons()
        {
            foreach (BoardButton button in checkerboard) button.FlatStyle = FlatStyle.Standard;
        }
        public void UnmarkAllGreenButtons()
        {
            foreach (BoardButton button in checkerboard)
            {
                if (button.FlatAppearance.BorderColor == Color.Green) 
                { 
                    button.FlatStyle = FlatStyle.Standard; 
                }
            }
        }

        private void MakeTheKing(BoardButton currentButton, Team currentTeam)
        {
            if(currentTeam.GetTeamDirection() == 1 && currentButton.Row == 7)
            {
                currentButton.IsKing = true;
                TryToSetKingButtonParameters(currentButton);
            }
            else if (currentTeam.GetTeamDirection() == -1 && currentButton.Row == 0)
            {
                currentButton.IsKing = true;
                TryToSetKingButtonParameters(currentButton);
            }
        }

        private void TryToSetKingButtonParameters(BoardButton button)
        {
            if(button.IsKing == true)
            {
                button.Text = "♚";
                button.Font = new Font(button.Font.FontFamily, 20);
                button.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        private void TryToSetKingButtonParameters(BoardButton oldPosition, BoardButton newPosition)
        {
            oldPosition.Text = null;

            if (oldPosition.IsKing == true)
            {
                newPosition.Text = "♚";
                newPosition.Font = new Font(newPosition.Font.FontFamily, 20);
                newPosition.TextAlign = ContentAlignment.MiddleCenter;
            }
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
                UnmarkAllButtons();
                DisableAllButtons();
                if (Execution(oldPosition, newPosition))
                {
                    currentTeam.AddPoint();
                    newPosition.IsEnabled = true;
                    newPosition.IsChosen = true;
                }
                ClearMarkingLists();
            }
        }

        public bool Execution(BoardButton oldPosition, BoardButton newPosition)
        {
            int directionColumnFactor = newPosition.Column - oldPosition.Column;
            directionColumnFactor /= Math.Abs(directionColumnFactor);

            int directionRowFactor = newPosition.Row - oldPosition.Row;
            directionRowFactor /= Math.Abs(directionRowFactor);

            foreach(BoardButton button in figuresToExecuteList)
            {
                int directionExecutionColumnFactor = button.Column - oldPosition.Column;
                directionExecutionColumnFactor /= Math.Abs(directionExecutionColumnFactor);

                int directionExecutionRowFactor = button.Row - oldPosition.Row;
                directionExecutionRowFactor /= Math.Abs(directionExecutionRowFactor);

                if (directionExecutionColumnFactor == directionColumnFactor && directionExecutionRowFactor == directionRowFactor)
                {
                    ExecuteOpponentsFigure(button);
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
            validPositionsList.Clear();
            validExecutionMovesList.Clear();
            figuresToExecuteList.Clear();
            validFiguresToMoveList.Clear();
        }

        public void ClearValidExecutionMovesList()
        {
            validExecutionMovesList.Clear();
        }

        public void ClearValidPositionsList()
        {
            validPositionsList.Clear();
        }

        public void Clear(List<BoardButton> listToClear)
        {
            listToClear.Clear();
        }

        public bool FightChecker(Team currentTeam)
        {
            bool fightIndicator = false;

            foreach(BoardButton button in checkerboard)
            {
                if(button.IsEnabled)
                {
                    if (!button.IsKing)
                        ValidMenMoves(button, currentTeam);
                    else
                        ValidKingsMoves(button, currentTeam);
                }
            }

            if (figuresToExecuteList.Count != 0)
            {
                fightIndicator = true;

                DisableAllButtons();
                EnableFiguresToMove();
                validPositionsList.Clear();

                foreach (BoardButton button in figuresToExecuteList)
                {
                    EnableButton(button);
                    MarkButton(button, Color.Red);
                }
            }

            return fightIndicator;
        }

        public void EnableFiguresToMove()
        {
            foreach (BoardButton figureToEnable in validFiguresToMoveList)
                EnableButton(figureToEnable);
        }

        public void ValidMenMoves(BoardButton currentButton, Team currentTeam)
        {
            int factor = currentTeam.GetTeamDirection();

            foreach (BoardButton button in checkerboard)
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
                            if (IsFieldEmpty(checkerboard[button.Column + columnDirection, button.Row + rowDirection]))
                            {

                                figuresToExecuteList.Add(checkerboard[button.Column, button.Row]);
                                validExecutionMovesList.Add(checkerboard[button.Column + columnDirection, button.Row + rowDirection]);
                                validFiguresToMoveList.Add(currentButton);
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        public void MarkAllValidButtons()
        {
            if (validPositionsList.Count == 0)
            {
                foreach (BoardButton button in validExecutionMovesList)
                {
                    EnableButton(button);
                    MarkButton(button);
                }
            }
            else
            {
                foreach (BoardButton button in validPositionsList)
                {
                    EnableButton(button);
                    MarkButton(button);
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

        public void Path (BoardButton currentButton, Team currentTeam, int columnFactor, int rowFactor, int status)
        {
            int row = currentButton.Row+rowFactor;
            int column = currentButton.Column+columnFactor;
            try
            {
                if (IsFieldEmpty(checkerboard[column, row]))
                {
                    if (status == 0)
                    {
                        validPositionsList.Add(checkerboard[column, row]);
                    }
                    else if (status == 1)
                    {
                        validExecutionMovesList.Add(checkerboard[column, row]);
                    }
                    
                    Path(checkerboard[column, row], currentTeam, columnFactor, rowFactor, status);
                }
                else if (IsFieldOccupiedByOpponent(checkerboard[column, row], currentTeam) && status == 0 && IsFieldEmpty(checkerboard[column + columnFactor, row + rowFactor]))
                {
                    figuresToExecuteList.Add(checkerboard[column, row]);
                    validPositionsList.Clear();

                    status++;
                    Path(checkerboard[column, row], currentTeam, columnFactor, rowFactor, status);
                }
            }
            catch { }
        }
    }
}
