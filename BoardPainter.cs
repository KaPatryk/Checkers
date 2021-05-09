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
    static class BoardPainter
    {
        private const int DEFAULT_MARKED_BUTTON_BORDER = 3;
        private const int DEFAULT_BUTTON_BORDER = 1;

        public static void SetButtonBackColor(BoardButton boardButton, int column, int row)
        {
            if (NumberCheck.IsEven(row) && NumberCheck.IsEven(column))
            {
                boardButton.BackColor = Color.White;
            }
            else if (!NumberCheck.IsEven(row) && NumberCheck.IsEven(column))
            {
                boardButton.BackColor = Color.Black;
            }
            else if (!NumberCheck.IsEven(row) && !NumberCheck.IsEven(column))
            {
                boardButton.BackColor = Color.White;
            }
            else if (NumberCheck.IsEven(row) && !NumberCheck.IsEven(column))
            {
                boardButton.BackColor = Color.Black;
            }
        }

        public static void MarkButton(BoardButton boardButton)
        {
            boardButton.FlatStyle = FlatStyle.Flat;
            boardButton.FlatAppearance.BorderSize = DEFAULT_MARKED_BUTTON_BORDER;
            boardButton.FlatAppearance.BorderColor = Color.Green;
        }

        public static void MarkButton(BoardButton boardButton, Color color)
        {
            boardButton.FlatStyle = FlatStyle.Flat;
            boardButton.FlatAppearance.BorderSize = DEFAULT_MARKED_BUTTON_BORDER;
            boardButton.FlatAppearance.BorderColor = color;
        }

        public static void UnmarkAllButtons(BoardButton [,] checkerboard)
        {
            foreach (BoardButton button in checkerboard)
            {
                button.FlatAppearance.BorderColor = Color.Black;
                button.FlatAppearance.BorderSize = DEFAULT_BUTTON_BORDER;
            }
        }

        public static void UnmarkAllGreenButtons(BoardButton[,] checkerboard)
        {
            foreach (BoardButton button in checkerboard)
            {
                if (button.FlatAppearance.BorderColor == Color.Green)
                {
                    button.FlatAppearance.BorderColor = Color.Black;
                    button.FlatAppearance.BorderSize = DEFAULT_BUTTON_BORDER;
                }
            }
        }
    }
}
