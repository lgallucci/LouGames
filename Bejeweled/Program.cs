using System;
using System.Collections.Generic;

namespace Bejeweled
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Engine game = new Engine();
            game.Run();
        }
    }
}
