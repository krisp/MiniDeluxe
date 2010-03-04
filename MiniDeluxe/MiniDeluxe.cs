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
        private const String Serialport = "COM20";
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
                        default:
                            _mode = value;
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
                        case "160":
                            _band = "160m";
                            break;
                        case "080":
                            _band = "80m";
                            break;
                        case "060":
                            _band = "60m";
                            break;
                        case "040":
                            _band = "40m";
                            break;
                        case "030":
                            _band = "30m";
                            break;
                        case "020":
                            _band = "20m";
                            break;
                        case "017":
                            _band = "17m";
                            break;
                        case "015":
                            _band = "15m";
                            break;
                        case "012":
                            _band = "12m";
                            break;
                        case "010":
                            _band = "10m";
                            break;
                        case "006":
                            _band = "6m";
                            break;
                        case "002":
                            _band = "2m";
                            break;
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
            _data = new RadioData {vfoa = "0", vfob = "0", Mode = "", mox = false};
            _cat = new CATConnector(new SerialPort(Serialport));
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
            else if( s.Contains("GET DROPDOWN-LIST"))
            {
                bw.Write(HRDMessage.HRDMessageToByteArray(GetDropdownList(s)));
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
            MatchCollection mc = Regex.Matches(s, "{([A-Z~]+)}");            

            if(mc.Count == 0) return String.Empty; 

            foreach (Match m in mc)
            {                
                switch (m.Groups[1].Value)
                {
                    case "MODE":
                        output.Append("Mode: " + _data.Mode + "\u0009");
                        break;
                    case "BAND":
                        output.Append("Band: " + _data.Band + "\u0009");
                        break;
                    case "AGC":
                        output.Append("AGC: Med" + "\u0009");
                        break;
                    case "DISPLAY":
                        output.Append("Display: Off" + "\u0009");
                        break;
                    case "PREAMP":
                        output.Append("Preamp: High" + "\u0009");
                        break;
                    case "DSP~FLTR":
                        output.Append("DSP Fltr: 2.3kHz" + "\u0009");
                        break;
                }
            }            
            return output.ToString();
        }

        private String GetDropdownList(String s)
        {
            String q = s.Substring(s.IndexOf("{") + 1, (s.IndexOf("}") - s.IndexOf("{") - 1));
            String output;

            switch(q)
            {
                case "MODE":
                    output = "LSB,USB,DSB,CWL,CWU,FMN,AM,DIGU,SPEC,DIGL,SAM,DRM";
                    break;
                case "AGC":
                    output = "Fixed,Long,Slow,Med,Fast";
                    break;
                case "BAND":
                    output = "160m,80m,60m,40m,30m,20m,17m,15m,12m,10m,6m,2m,GEN,WWV";
                    break;
                case "DISPLAY":
                    output = "Spectrum,Panadapter,Scope,Phase,Phase2,Waterfall,Histogram,Off";
                    break;
                case "DSP FLTR":
                    output = "6.0kHz,4.0kHz,2.6kHz,2.1kHz,1.0kHz,500Hz,250Hz,100Hz,50Hz,25Hz,VAR1,VAR2";
                    break;
                case "PREAMP":
                    output = "Off,Low,Medium,High";
                    break;
                default:
                    output = String.Empty;
                    break;
            }

            return output;
        }
    }  
}
