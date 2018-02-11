using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper
{
    internal class Square
    {
        /* Properties */
        private int _value;
        private DisplayInfo _displayInfo;

        internal int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        internal DisplayInfo displayInfo
        {
            get { return _displayInfo; }
            set { _displayInfo = value; }
        }

        /* Constructor */
        internal Square()
        {
            _displayInfo = DisplayInfo.SquareUnclicked;
        }
    }

    internal enum DisplayInfo
    {
        SquareUnclicked = 0,
        BombFlag = 1,
        QuestionFlag = 2,
        BombExplode = 3,
        SquareClicked = 4,
        IncorrectBombFlag = 5
    }
}
