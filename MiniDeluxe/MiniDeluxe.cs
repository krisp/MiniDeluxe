using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Timers;
using System.Net.Sockets;
using System.IO;

namespace MiniDeluxe
{
    class MiniDeluxe
    {
        Timer timer;
        CATConnector cat;
        HRDTCPServer server;
        RadioData data;

        struct RadioData
        {
            private string _Mode;
            private string _Band;

            public string VFOA;
            public string VFOB;
            public bool MOX;

            public string Mode
            {
                get { return _Mode; }
                set
                {
                    switch (value)
                    {
                        case "00":
                            _Mode = "LSB";
                            break;
                        case "01":
                            _Mode = "USB";
                            break;
                        case "02":
                            _Mode = "DSB";
                            break;
                        case "03":
                            _Mode = "CWL";
                            break;
                        case "04":
                            _Mode = "CWU";
                            break;
                        case "05":
                            _Mode = "FMN";
                            break;
                        case "06":
                            _Mode = "AM";
                            break;
                        case "07":
                            _Mode = "DIGU";
                            break;
                        case "08":
                            _Mode = "SPEC";
                            break;
                        case "09":
                            _Mode = "DIGL";
                            break;
                        case "10":
                            _Mode = "SAM";
                            break;
                        case "11":
                            _Mode = "DRM";
                            break;
                    }
                }
            }
            public string Band
            {
                get { return _Band; }
                set
                {
                    switch (value)
                    {
                        case "888":
                            _Band = "GEN";
                            break;
                        case "999":
                            _Band = "WWV";
                            break;
                        default:
                            _Band = value;
                            break;
                    }
                }
            }
        }

        public MiniDeluxe()
        {
            data = new RadioData();
            data.VFOA = "0";
            data.VFOB = "0";
            data.Mode = "";
            data.MOX = false;

            cat = new CATConnector(new SerialPort("COM20"));
            cat.CATEvent += new CATEventHandler(cat_CATEvent);

            cat.WriteCommand("ZZIF;");
            cat.WriteCommand("ZZFB;");
            cat.WriteCommand("ZZBS;");

            timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            server = new HRDTCPServer();
            server.HRDTCPEvent += new HRDTCPEventHandler(server_HRDTCPEvent);
        }

        void server_HRDTCPEvent(object sender, HRDTCPEventArgs e)
        {
            String s = e.ToString().ToUpper();
            BinaryWriter bw = new BinaryWriter(e.Client.GetStream());

            if (s.Contains("GET ID"))
            {
                bw.Write(HRDMessage.HRDMessageToByteArray("Ham Radio Deluxe"));
            }
            else if (s.Contains("GET VERSION"))
            {
                bw.Write(HRDMessage.HRDMessageToByteArray("v0.1"));
            }
            else if (s.Contains("GET FREQUENCY"))
            {
                bw.Write(HRDMessage.HRDMessageToByteArray(data.VFOA));
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
                bw.Write(HRDMessage.HRDMessageToByteArray(data.VFOA + "-" + data.VFOB));
            }
            else if (s.Contains("GET DROPDOWN-TEXT"))
            {
                bw.Write(HRDMessage.HRDMessageToByteArray(GetDropdownText(s)));
            }
            else
            {
                bw.Write(HRDMessage.HRDMessageToByteArray(""));
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            cat.WriteCommand("ZZIF;");
            cat.WriteCommand("ZZFB;");
            cat.WriteCommand("ZZBS;");
        }

        void cat_CATEvent(object sender, CATEventArgs e)
        {
            switch(e.Command)
            {
                case "ZZIF":                
                    data.VFOA = e.Data.Substring(0, 11);
                    data.Mode = e.Data.Substring(27, 2);
                    data.MOX = (e.Data.Substring(26, 1).Equals(1)) ? true : false;
                    break;
                case "ZZFB":
                    data.VFOB = e.Data;
                    break;
                case "ZZBS":
                    data.Band = e.Data;
                    break;
            }
        }

        private String GetDropdownText(String s)
        {
            StringBuilder output = new StringBuilder();
            Match m = Regex.Match(s, "GET DROPDOWN-TEXT {(.*)}");
            if(m.Success)
            {
                foreach(Group g in m.Groups)
                {
                    switch(g.Value)
                    {
                        case "MODE":
                            output.Append("Mode: " + data.Mode + "\u0009");
                            break;
                        case "BAND":
                            output.Append("Band: " + data.Band + "\u0009");
                            break;
                    }
                }
            }

            return output.ToString();
        }
    }  
}
