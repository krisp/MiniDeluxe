using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace MiniDeluxe
{
    class MiniDeluxe
    {
        public MiniDeluxe()
        {
            CATConnector c = new CATConnector(new SerialPort("COM21"));
            c.CATEvent += new CATEventHandler(c_CATEvent);
        }

        void c_CATEvent(object sender, CATEventArgs e)
        {
            Console.WriteLine("Command: " + e.Command + " Data: " + e.Data);
        }
    }  
}
