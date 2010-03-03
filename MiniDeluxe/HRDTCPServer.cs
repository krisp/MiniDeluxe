using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace MiniDeluxe
{
    class HRDTCPServer
    {
        private bool stopListening = false;
        private ArrayList clients;

        public HRDTCPServer()
        {
            Thread ListenerThread = new Thread(new ThreadStart(ListenerThread));
            ListenerThread.Start();
        }

        private void ListenerThread()
        {

        }
    }
}
