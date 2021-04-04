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
        public int Column { get; set; }
        public int Row { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsQueen { get; set; }
    }
}
