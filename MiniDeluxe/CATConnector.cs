using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Threading;

namespace MiniDeluxe
{   
    class CATConnector
    {
        public event CATEventHandler CATEvent;
        
        private SerialPort port;
        private Thread readThread;
        private StringBuilder buffer;
        private bool stopThread = false;        

        public CATConnector(SerialPort port)
        {
            buffer = new StringBuilder();

            this.port = port;            
            port.Open();

            readThread = new Thread(new ThreadStart(ReadThread));
            readThread.Start();
        }

        private void ReadThread()
        {
            while (!stopThread)
            {
                try
                {
                    char b = (char)port.ReadChar();
                    buffer.Append(b);
                    ProcessData();
                }
                catch { }
            }
        }

        public void WriteCommand(String command)
        {
            try
            {
                port.Write(command);
            }
            catch { }
        }

        private void ProcessData()
        {
            if (buffer.ToString().EndsWith(";"))
            {
                ParseCommand(buffer.ToString());
                buffer = new StringBuilder();
            }
        }

        private void ParseCommand(String command)
        {           
            Match m = Regex.Match(command, "([A-Z]{2,4})(.*);");
            if (m.Success)
            {
                CATEventArgs cea = new CATEventArgs(m.Groups[1].ToString(), m.Groups[2].ToString());
                if (CATEvent != null)
                    CATEvent(this, cea);               
                return;
            }
        }

        public void Close()
        {
            stopThread = true;
            port.Close();
        }
    }
   
    public delegate void CATEventHandler(object sender, CATEventArgs e);
    public class CATEventArgs : EventArgs
    {
        private String _command;
        private String _data;

        public String Data { get { return _data; } set { _data = value; } }
        public String Command { get { return _command; } set { _command = value; } }

        public CATEventArgs(String command, String data)
        {
            _command = command;
            _data = data;
        }

        public CATEventArgs() { }
    }
}
