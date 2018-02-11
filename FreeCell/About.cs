using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace FreeCell
{
    internal partial class About : Form
    {
        /* Constructor */
        internal About()
        {
            InitializeComponent();

            string appName = Assembly.GetAssembly(this.GetType()).Location;
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(appName);
            lblVersion.Text = "v" + assemblyName.Version.ToString(3);
        }
    }
}