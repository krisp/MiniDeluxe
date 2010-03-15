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
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace MiniDeluxe
{
    class HRDTCPServer
    {
        public bool IsListening { get; private set; }

        public event HRDTCPEventHandler HRDTCPEvent;

        private bool _stopListening;
        private bool _stopClients;
        private int _connectionCount;
        private readonly MiniDeluxe _parent;

        private readonly TcpListener _listener;

        public HRDTCPServer(MiniDeluxe parent)
        {
            _parent = parent;
            _connectionCount = 0;
            _listener = new TcpListener(Properties.Settings.Default.LocalOnly ? IPAddress.Loopback : IPAddress.Any,
                                            Properties.Settings.Default.Port);
        }

        public void Start()
        {
            _listener.Start();
            IsListening = true;
            Thread listenerThread = new Thread(ListenerThread);
            listenerThread.Start();
        }

        private void ListenerThread()
        {
            while (!_stopListening)
            {
                try
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    Thread clientThread = new Thread(ClientThread);
                    clientThread.Start(client);                    
                }
                catch
                {
                }
                _parent.SetNotifyIconText("MiniDeluxe - Running (" + ++_connectionCount + " connections)");
            }
        }

        private void ClientThread(object o)
        {            
            TcpClient client = (TcpClient)o;            
            BinaryReader br = new BinaryReader(client.GetStream());            

            while (!_stopClients)
            {
                try
                {
                    HRDMessageBlock msg = HRDMessage.BytesToHRDMessage(br);

                    if (msg.nSize == 0)
                        break;

                    HRDTCPEventArgs e = new HRDTCPEventArgs(client, msg);

                    if (HRDTCPEvent != null)
                        HRDTCPEvent(this, e);
                }
                catch
                {
                }
            }
            _parent.SetNotifyIconText("MiniDeluxe - Running (" + --_connectionCount + " connections)");
        }

        public void Close()
        {
            _stopListening = true;
            _stopClients = true;
            IsListening = false;
            _listener.Stop();
        }        
    }
    
    public struct HRDMessageBlock
    {
        public uint nSize;
        public uint nSanity1;
        public uint nSanity2;
        public uint nChecksum;        
        public byte[] szText;
    }
    
    public static class HRDMessage
    {        
        public static byte[] HRDMessageToByteArray(String szText)
        {            
            // create HRD message
            HRDMessageBlock msg = new HRDMessageBlock
              {
                  nChecksum = 0,
                  nSanity1 = 0x1234ABCD,
                  nSanity2 = 0xABCD1234,
                  szText = Encoding.Unicode.GetBytes(szText + "\0"),
                  nSize = (uint)Encoding.Unicode.GetByteCount(szText + "\0") + (sizeof(uint) * 4)                      
              };

#if DEBUG
            MiniDeluxe.Debug(String.Format("TX: {0}", szText));
#endif
            // Serialize it
            int len = (int)msg.nSize;
            byte[] buf = new byte[len];
            Array.Copy(BitConverter.GetBytes(msg.nSize), 0, buf, 0, 4);
            Array.Copy(BitConverter.GetBytes(msg.nSanity1), 0, buf, 4, 4);
            Array.Copy(BitConverter.GetBytes(msg.nSanity2), 0, buf, 8, 4);
            Array.Copy(BitConverter.GetBytes(msg.nChecksum), 0, buf, 12, 4);
            Array.Copy(msg.szText, 0, buf, 16, msg.szText.Length);
            return buf;
        }

        public static HRDMessageBlock BytesToHRDMessage(BinaryReader br)
        {
            HRDMessageBlock msg = new HRDMessageBlock();

            try
            {
                msg.nSize = br.ReadUInt32();
                msg.nSanity1 = br.ReadUInt32();
                msg.nSanity2 = br.ReadUInt32();
                msg.nChecksum = br.ReadUInt32();
                msg.szText = br.ReadBytes((int)msg.nSize - (sizeof(UInt32) * 4));
                return msg;
            }
            catch
            {
                msg.nSize = 0;
                return msg;
            }
        }
    }

    public delegate void HRDTCPEventHandler(object sender, HRDTCPEventArgs e);
    public class HRDTCPEventArgs : EventArgs
    {
        public TcpClient Client { get; private set; }
        public HRDMessageBlock Message { get; private set; }

        public HRDTCPEventArgs(TcpClient client, HRDMessageBlock msg)
        {
            Client = client;
            Message = msg;
        }

        public override String ToString()
        {
            return new String(Encoding.Unicode.GetChars(Message.szText));
        }
    }
}
