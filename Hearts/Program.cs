using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hearts
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Engine());
        }
    }
}