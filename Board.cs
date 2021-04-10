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
        private List<BoardButton> validPositionsList = new List<BoardButton>();
        private List<BoardButton> figuresToExecuteList = new List<BoardButton>();
        private List<BoardButton> validExecutionMovesList = new List<BoardButton>();
        private List<BoardButton> validFiguresToMoveList = new List<BoardButton>();


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

        public void DisableValidExecutionMovesButtons()
        {
            foreach (BoardButton button in validExecutionMovesList)
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

        private void MakeTheKing(BoardButton currentButton, Team currentTeam)
        {
            if(currentTeam.GetTeamDirection() == 1 && currentButton.Row == 7)
                currentButton.IsKing = true;
            else if (currentTeam.GetTeamDirection() == -1 && currentButton.Row == 0)
                currentButton.IsKing = true;
        }

        public void MakeMove(BoardButton oldPosition, BoardButton newPosition, Team currentTeam)
        {
            if (newPosition.IsEnabled)
            {
                newPosition.Image = currentTeam.figureImage;
                newPosition.IsKing = oldPosition.IsKing;
                MakeTheKing(newPosition, currentTeam);
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
            for (int i = 0; i < validExecutionMovesList.Count; i++)
            {
                if (newPosition == validExecutionMovesList[i])
                {
                    ExecuteOpponentsFigure(figuresToExecuteList[i]);
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
                    ValidMenMoves(button, currentTeam); 
                }
            }

            if (figuresToExecuteList.Count != 0)
            {
                fightIndicator = true;

                DisableAllButtons();
                EnableFiguresToMove();
                validPositionsList.Clear();

                foreach (BoardButton button in validExecutionMovesList)
                {
                    EnableButton(button);
                    MarkButton(button);
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
            if (validExecutionMovesList.Count > 0)
                return true;
            else
                return false;
        }


        //tutaj zaczyna się część eksperymentalna nad ruchem damki
        public void ValidKingsMoves(BoardButton currentButton, Team currentTeam) //Dopracować. Zmienić możliwe ruchy i dodać zbijanie.
        {
            /*
            foreach (BoardButton button in checkerboard)
            {
                if ((button.Row - button.Column) == (currentButton.Row - currentButton.Column) || (button.Row + button.Column) == (currentButton.Row + currentButton.Column))
                {
                    if (IsFieldEmpty(button)) validPositionsList.Add(button);
                    else if (IsFieldOccupiedByOpponent(button, currentTeam))
                    {
                        int rowDirection = button.Row - currentButton.Row;
                        rowDirection /= Math.Abs(rowDirection);
                        int columnDirection = button.Column - currentButton.Column;
                        columnDirection /= Math.Abs(columnDirection);
                        try
                        {
                            if (IsFieldEmpty(checkerboard[button.Column + columnDirection, button.Row + rowDirection]) && IsFieldEmpty(checkerboard[button.Column - columnDirection, button.Row - rowDirection]))
                            {
                                figuresToExecuteList.Add(checkerboard[button.Column, button.Row]);
                                validExecutionMovesList.Add(checkerboard[button.Column + columnDirection, button.Row + rowDirection]);
                                validPositionsList.Add(checkerboard[button.Column + columnDirection, button.Row + rowDirection]);
                            }
                        }
                        catch { }
                    }
                }
            }
            */
            Path(currentButton, currentTeam, 1, -1, 0);
            Path(currentButton, currentTeam, -1, 1, 0);
            Path(currentButton, currentTeam, -1, -1, 0);
            Path(currentButton, currentTeam, 1, 1, 0);
        }

        public void Path (BoardButton currentButton, Team currentTeam, int columnFactor, int rowFactor, int status)
        {
            int row = currentButton.Row+rowFactor;
            int column = currentButton.Column+columnFactor;
            try
            {
                while (IsFieldEmpty(checkerboard[column, row]))
                {
                    validPositionsList.Add(checkerboard[column, row]);

                    if(status == 1)
                    {
                        validExecutionMovesList.Add(checkerboard[column, row]);
                    }

                    column += columnFactor;
                    row += rowFactor;
                }

                if(IsFieldOccupiedByOpponent(checkerboard[column,row], currentTeam) && status == 0)
                {

                    figuresToExecuteList.Add(checkerboard[column, row]);
                    Path(checkerboard[column, row], currentTeam, columnFactor, rowFactor, 1);
                }

            }
            catch { }

        }

        

    }
}
