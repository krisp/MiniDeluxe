using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Timers;
using System.Net.Sockets;
using System.IO;

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
            server.HRDTCPEvent += new HRDTCPEventHandler(server_HRDTCPEvent);
        }

        void server_HRDTCPEvent(object sender, HRDTCPEventArgs e)
        {
            String s = e.ToString().ToUpper();
            BinaryWriter bw = new BinaryWriter(e.Client.GetStream());
            
            if (s.Contains("GET ID"))
            {
                bw.Write(HRDMessage.HRDMessageToByteArray("MiniDeluxe"));
            }
            else if (s.Contains("GET VERSION"))
            {
                bw.Write(HRDMessage.HRDMessageToByteArray("v0.1"));
            }
            else if (s.Contains("GET FREQUENCY"))
            {
                bw.Write(HRDMessage.HRDMessageToByteArray("144450000"));
            }
            else if (s.Contains("GET RADIO"))
            {
                bw.Write(HRDMessage.HRDMessageToByteArray("PowerSDR"));
            }
            else if (s.Contains("GET CONTEXT"))
            {
                bw.Write(HRDMessage.HRDMessageToByteArray("1"));
            }
            else if (s.Contains("GET FREQUENCIES"))
            {
                bw.Write(HRDMessage.HRDMessageToByteArray("144450000-433000000"));
            }
            else
            {
                bw.Write(HRDMessage.HRDMessageToByteArray(""));
            }
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
