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
            try
            {
                _port.Open();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

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
            while(_port.IsOpen) { }
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
