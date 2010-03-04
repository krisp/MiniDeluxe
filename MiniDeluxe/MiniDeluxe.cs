using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Timers;
using System.IO;

namespace MiniDeluxe
{
    class MiniDeluxe
    {
        readonly Timer _timer;
        readonly CATConnector _cat;
        readonly HRDTCPServer _server;
        RadioData _data;

        struct RadioData
        {
            private string _mode;
            private string _band;

            public string vfoa;
            public string vfob;
            public bool mox;

            public string Mode
            {
                get { return _mode; }
                set
                {
                    switch (value)
                    {
                        case "00":
                            _mode = "LSB";
                            break;
                        case "01":
                            _mode = "USB";
                            break;
                        case "02":
                            _mode = "DSB";
                            break;
                        case "03":
                            _mode = "CWL";
                            break;
                        case "04":
                            _mode = "CWU";
                            break;
                        case "05":
                            _mode = "FMN";
                            break;
                        case "06":
                            _mode = "AM";
                            break;
                        case "07":
                            _mode = "DIGU";
                            break;
                        case "08":
                            _mode = "SPEC";
                            break;
                        case "09":
                            _mode = "DIGL";
                            break;
                        case "10":
                            _mode = "SAM";
                            break;
                        case "11":
                            _mode = "DRM";
                            break;
                    }
                }
            }
            public string Band
            {
                get { return _band; }
                set
                {
                    switch (value)
                    {
                        case "888":
                            _band = "GEN";
                            break;
                        case "999":
                            _band = "WWV";
                            break;
                        default:
                            _band = value;
                            break;
                    }
                }
            }
        }

        public MiniDeluxe()
        {
            _data = new RadioData();
            _data.vfoa = "0";
            _data.vfob = "0";
            _data.Mode = "";
            _data.mox = false;

            _cat = new CATConnector(new SerialPort("COM20"));
            _cat.CATEvent += CatcatEvent;

            _cat.WriteCommand("ZZIF;");
            _cat.WriteCommand("ZZFB;");
            _cat.WriteCommand("ZZBS;");

            _timer = new Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();

            _server = new HRDTCPServer();
            _server.HRDTCPEvent += ServerHRDTCPEvent;
        }

        void ServerHRDTCPEvent(object sender, HRDTCPEventArgs e)
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
                bw.Write(HRDMessage.HRDMessageToByteArray(_data.vfoa));
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
                bw.Write(HRDMessage.HRDMessageToByteArray(_data.vfoa + "-" + _data.vfob));
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

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _cat.WriteCommand("ZZIF;");
            _cat.WriteCommand("ZZFB;");
            _cat.WriteCommand("ZZBS;");
        }

        void CatcatEvent(object sender, CATEventArgs e)
        {
            switch(e.Command)
            {
                case "ZZIF":                
                    _data.vfoa = e.Data.Substring(0, 11);
                    _data.Mode = e.Data.Substring(27, 2);
                    _data.mox = (e.Data.Substring(26, 1).Equals(1)) ? true : false;
                    break;
                case "ZZFB":
                    _data.vfob = e.Data;
                    break;
                case "ZZBS":
                    _data.Band = e.Data;
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
                            output.Append("Mode: " + _data.Mode + "\u0009");
                            break;
                        case "BAND":
                            output.Append("Band: " + _data.Band + "\u0009");
                            break;
                    }
                }
            }

            return output.ToString();
        }
    }  
}
