using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MiniDeluxe
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MiniDeluxe deluxe = new MiniDeluxe();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run();
        }
    }
}
