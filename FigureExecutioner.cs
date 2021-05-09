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
    static class FigureExecutioner
    {
        public static List<BoardButton> figuresToExecuteList { get; private set; } = new List<BoardButton>();

        public static bool Execution(BoardButton oldPosition, BoardButton newPosition)
        {
            int directionColumnFactor = newPosition.Column - oldPosition.Column;
            int directionRowFactor = newPosition.Row - oldPosition.Row;

            if(directionColumnFactor != 0 && directionRowFactor != 0)
            {
                directionColumnFactor /= Math.Abs(directionColumnFactor);
                directionRowFactor /= Math.Abs(directionRowFactor);
            }

            foreach (BoardButton button in figuresToExecuteList)
            {
                int directionExecutionColumnFactor = button.Column - oldPosition.Column;
                directionExecutionColumnFactor /= Math.Abs(directionExecutionColumnFactor);

                int directionExecutionRowFactor = button.Row - oldPosition.Row;
                directionExecutionRowFactor /= Math.Abs(directionExecutionRowFactor);

                int rowsBetween = Math.Abs(button.Row - oldPosition.Row);
                int columnsBetween = Math.Abs(button.Column - oldPosition.Column);

                if (directionExecutionColumnFactor == directionColumnFactor && directionExecutionRowFactor == directionRowFactor && columnsBetween == rowsBetween)
                {
                    ExecuteOpponentsFigure(button);
                    return true;
                }
            }
            return false;
        }

        private static void ExecuteOpponentsFigure(BoardButton figureToExecute)
        {
            figureToExecute.Image = null;
            ClearExecutionList();
        }

        public static bool IsFigureToExecute()
        {
            return figuresToExecuteList.Count > 0;
        }

        public static void AddFigureToExecutionList(BoardButton figureToExecute)
        {
            figuresToExecuteList.Add(figureToExecute);
        }

        private static void ClearExecutionList()
        {
            figuresToExecuteList.Clear();
        }
    }
}
