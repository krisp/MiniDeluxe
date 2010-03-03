using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Timers;

namespace MiniDeluxe
{
    class MiniDeluxe
    {
        Timer t;
        CATConnector c;

        public MiniDeluxe()
        {
            /* test code for serial port 
            c = new CATConnector(new SerialPort("COM21"));
            c.CATEvent += new CATEventHandler(c_CATEvent);

            t = new Timer(1000);
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Start();
            */

            // test code for tcpip

            HRDTCPServer server = new HRDTCPServer();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            c.WriteCommand("ZZIF;");
        }

        void c_CATEvent(object sender, CATEventArgs e)
        {
            Console.WriteLine("Command: " + e.Command + " Data: " + e.Data);
        }
    }  
}
