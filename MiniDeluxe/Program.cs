/* This file is part of MiniDeluxe.
   MiniDeluxe is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   MiniDeluxe is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with MiniDeluxe.  If not, see <http://www.gnu.org/licenses/>.
   
   MiniDeluxe is Copyright (C) 2010 by K1FSY
*/
using System;
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
            System.Diagnostics.Process.GetCurrentProcess().MaxWorkingSet =
                System.Diagnostics.Process.GetCurrentProcess().MinWorkingSet;

#if DEBUG
            System.Diagnostics.Debug.Listeners.Add(new System.Diagnostics.TextWriterTraceListener("debug.txt"));            
#endif
            System.Diagnostics.Debug.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(Console.Out));
            MiniDeluxe.Debug(String.Format("MiniDeluxe version {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version));            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new MiniDeluxe();
            Application.Run();
        }
    }
}
