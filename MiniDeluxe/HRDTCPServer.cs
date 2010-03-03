using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections;
using System.IO;

namespace MiniDeluxe
{
    class HRDTCPServer
    {
        private bool stopListening = false;
        private bool stopClients = false;

        private ArrayList clients;
        private TcpListener listener;

        public HRDTCPServer()
        {
            clients = new ArrayList();
            listener = new TcpListener(IPAddress.Any, 7810);
            listener.Start();
            Thread listenerThread = new Thread(new ThreadStart(ListenerThread));
            listenerThread.Start();
        }

        private void ListenerThread()
        {
            while (!stopListening)
            {
                TcpClient client = this.listener.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(ClientThread));
                clients.Add(client);
                clientThread.Start(client);
            }
        }

        private void ClientThread(object o)
        {
            TcpClient client = (TcpClient)o;            
            BinaryReader br = new BinaryReader(client.GetStream());                       
            BinaryWriter bw = new BinaryWriter(client.GetStream());

            while (!stopClients)
            {
                HRDMessageBlock msg = HRDMessage.BytesToHRDMessage(br);
                
                if (msg.nSize == 0)
                    break;

                String s = new String(Encoding.Unicode.GetChars(msg.szText)).ToUpper();                                               

                if (s.Contains("GET ID"))
                {                                       
                    bw.Write(HRDMessage.HRDMessageToByteArray("MiniDeluxe"));
                }
                else if(s.Contains("GET VERSION"))
                {
                    bw.Write(HRDMessage.HRDMessageToByteArray("v0.1"));
                }
                else if(s.Contains("GET FREQUENCY"))
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
            
            clients.Remove(client);
        }
    }
    
    struct HRDMessageBlock
    {
        public uint nSize;
        public uint nSanity1;
        public uint nSanity2;
        public uint nChecksum;        
        public byte[] szText;
    }
    
    static class HRDMessage
    {        
        public static byte[] HRDMessageToByteArray(String szText)
        {            
            // create HRD message
            HRDMessageBlock msg = new HRDMessageBlock();
            msg.nChecksum = 0;
            msg.nSanity1 = 0x1234ABCD;
            msg.nSanity2 = 0xABCD1234;
            msg.szText = Encoding.Unicode.GetBytes(szText + "\0"); 
            msg.nSize = (uint)Encoding.Unicode.GetByteCount(szText + "\0") + (sizeof(uint) * 4);

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
}
