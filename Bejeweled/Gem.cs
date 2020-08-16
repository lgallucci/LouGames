using System;
using System.Collections.Generic;
using System.Text;

namespace Bejeweled
{

    class GemPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
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

        internal GemPosition Position
        {
            get { return pos; }
            set { pos = value; }
        }
    }
}
