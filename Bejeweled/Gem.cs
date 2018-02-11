using System;
using System.Collections.Generic;
using System.Text;

namespace Bejeweled
{

    struct GemPosition
    {
        internal int x;
        internal int y;
    }

    class Gem
    {
        private int type;
        private GemPosition pos;

        /* Constructor */
        internal Gem()
        {
            type = -1;
            pos = new GemPosition();
        }

        /* Properties */
        internal int Type
        {
            get { return type; }
            set { type = value; }
        }

        internal int PositionX
        {
            get { return pos.x; }
            set { pos.x = value; }
        }

        internal int PositionY
        {
            get { return pos.y; }
            set { pos.y = value; }
        }
    }
}
