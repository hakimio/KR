using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Dialog_Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            String file = "";
            if (args.Length > 0)
            {
                file = Convert.ToString(args[0]);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GUI(file));
        }
    }
}
