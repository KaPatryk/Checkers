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
        public int Column { get; private set; }
        public int Row { get; private set; }
        public bool IsEnabled { get; set; }
        public bool IsQueen { get; set; }

        public BoardButton()
        {
            IsQueen = false;
        }

        public void SetPosition(int column, int row)
        {
            Column = column;
            Row = row;
        }
    }

    
}
