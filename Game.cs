using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers
{
    class Game
    {
        int whichTurn;

        private void NextTurn()
        {
            if (whichTurn == 1) whichTurn = -1;
            else if (whichTurn == -1) whichTurn = 1;
        }
    }
}
