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
    public class Team
    {
        public string Name { get; private set; }
        public string StartingPosition { get; private set; }
        public Image figureImage { get; private set; }
        public int Points { get; private set; }

        public Team(string name, Image figureImage, string startingPosition)
        {
            Name = name;
            this.figureImage = figureImage;
            StartingPosition = startingPosition;
            Points = 0;
        }

        public void AddPoint()
        {
            Points += 1;
        }

        public int GetTeamDirection()
        {
            if (this.StartingPosition == "Up") return 1;
            else if (this.StartingPosition == "Down") return -1;
            else
            {
                MessageBox.Show("The starting position is wrong.");
                return 0;
            }

        }
    }
}
