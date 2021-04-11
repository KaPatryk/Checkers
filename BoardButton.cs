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
    public class BoardButton : Button
    {
        public bool IsChosen { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsKing { get; set; }
        public int Column { get; private set; }
        public int Row { get; private set; }
        public int[] Position { get; set; }

        public BoardButton()
        {
            IsKing = false;
        }

        public BoardButton(int column, int row)
        {
            SetPosition(column, row);
            IsKing = false;
        }

        public void SetPosition(int column, int row)
        {
            Column = column;
            Row = row;
            Position = new int[] { column, row };
        }
    }
}
