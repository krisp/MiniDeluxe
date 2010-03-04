using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Threading;

namespace MiniDeluxe
{   
    class CATConnector
    {
        public event CATEventHandler CATEvent;
        
        private readonly SerialPort _port;
        private readonly Thread _readThread;
        private StringBuilder _buffer;
        private bool _stopThread;        

        public CATConnector(SerialPort port)
        {
            _buffer = new StringBuilder();

            _port = port;            
            port.Open();

            _readThread = new Thread(ReadThread);
            _readThread.Start();
        }

        private void ReadThread()
        {
            while (!_stopThread)
            {
                try
                {
                    char b = (char)_port.ReadChar();
                    _buffer.Append(b);
                    ProcessData();
                }
                catch
                { }
            }
        }

        public void WriteCommand(String command)
        {
            try
            {
                _port.Write(command);
            }
            catch
            {
            }
        }

        private void ProcessData()
        {
            if (_buffer.ToString().EndsWith(";"))
            {
                ParseCommand(_buffer.ToString());
                _buffer = new StringBuilder();
            }
        }

        private void ParseCommand(String command)
        {
            Match m = Regex.Match(command, "([A-Z]{2,4})(.*);", RegexOptions.Compiled);
            if (!m.Success) return;
            
            CATEventArgs cea = new CATEventArgs(m.Groups[1].ToString(), m.Groups[2].ToString());
            if (CATEvent != null)
                CATEvent(this, cea);
            return;
        }

        public void Close()
        {
            _stopThread = true;
            _port.Close();
        }
    }
   
    public delegate void CATEventHandler(object sender, CATEventArgs e);
    public class CATEventArgs : EventArgs
    {
        public String Data { get; set; }
        public String Command { get; set; }

        public CATEventArgs(String command, String data)
        {
            Command = command;
            Data = data;
        }

        public CATEventArgs() { }
    }
}
