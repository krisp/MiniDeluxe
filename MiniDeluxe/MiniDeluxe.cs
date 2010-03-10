using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Timers;
using System.IO;

namespace MiniDeluxe
{
    public class MiniDeluxe
    {
        #region Declarations        
        private Timer _timerShort;
        private Timer _timerLong;
        private CATConnector _cat;
        private HRDTCPServer _server;
        private readonly NotifyIcon _notifyIcon;

        RadioData _data;
        struct RadioData
        {
            private string _mode;
            private string _band;
            private string _displayMode;
            private string _agc;
            private string _smeter;

            public string vfoa;
            public string vfob;
            public string rawmode;
            public bool mox;
            

            public string Mode
            {
                get { return _mode; }
                set
                {
                    rawmode = value;
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
            public string DisplayMode
            {
                get { return _displayMode; }
                set
                {
                    switch(value)
                    {
                        case "0":
                            _displayMode = "Spectrum";
                            break;
                        case "1":
                            _displayMode = "Panadapter";
                            break;
                        case "2":
                            _displayMode = "Scope";
                            break;
                        case "3":
                            _displayMode = "Phase";
                            break;
                        case "4":
                            _displayMode = "Phase2";
                            break;
                        case "5":
                            _displayMode = "Waterfall";
                            break;
                        case "6":
                            _displayMode = "Histogram";
                            break;
                        case "7":
                            _displayMode = "Off";
                            break;
                    }
                }
            }
            public string AGC
            {
                get { return _agc; }
                set
                {
                    switch (value)
                    {
                        case "0":
                            _agc = "Fixed";
                            break;
                        case "1":
                            _agc = "Long";
                            break;
                        case "2":
                            _agc = "Slow";
                            break;
                        case "3":
                            _agc = "Med";
                            break;
                        case "4":
                            _agc = "Fast";
                            break;
                        case "5":
                            _agc = "Custom";
                            break;
                    }
                }
            }
            public string smeter
            {
                get { return _smeter; }
                set
                {
                    double i = (double.Parse(value) / 0.5) - 121;
                    if (i < -121) _smeter = "0";
                    else if (i < -115) _smeter = "1";
                    else if (i < -109) _smeter = "2";
                    else if (i < -103) _smeter = "3";
                    else if (i < -97) _smeter = "4";
                    else if (i < -91) _smeter = "5";
                    else if (i < -85) _smeter = "6";
                    else if (i < -79) _smeter = "7";
                    else if (i < -73) _smeter = "8";
                    else if (i < -63) _smeter = "9";
                    else if (i < -53) _smeter = "10";
                    else if (i < -43) _smeter = "11";
                    else if (i < -33) _smeter = "12";
                    else if (i < -23) _smeter = "13";
                    else if (i < -13) _smeter = "14";
                }
            }
        }
        #endregion

        #region Constructor
        public MiniDeluxe()
        {
            _notifyIcon = new NotifyIcon(this);

            if (Properties.Settings.Default.FirstRun)
                ShowOptionsForm();
            else
                Start();
        }
        #endregion

        #region Event Handlers
        void ServerHRDTCPEvent(object sender, HRDTCPEventArgs e)
        {
            String s = e.ToString().ToUpper();
            BinaryWriter bw = new BinaryWriter(e.Client.GetStream());

            s = s.Remove(s.IndexOf('\0'));

            if(s.Contains("GET"))            
                ProcessHRDTCPGetCommand(s,bw);                                                      
            else if(s.Contains("SET"))            
                ProcessHRDTCPSetCommand(s,bw);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        void TimerShortElapsed(object sender, ElapsedEventArgs e)
        {
            _cat.WriteCommand("ZZIF;");
            _cat.WriteCommand("ZZFB;");
            _cat.WriteCommand("ZZSM0;");
        }

        void TimerLongElapsed(object sender, ElapsedEventArgs e)
        {
            _cat.WriteCommand("ZZBS;");
            _cat.WriteCommand("ZZDM;");
            _cat.WriteCommand("ZZGT;");
        }

        void CatcatEvent(object sender, CATEventArgs e)
        {
            switch(e.Command)
            {
                // vfoa, mode, xmit status
                case "ZZIF":                
                    _data.vfoa = e.Data.Substring(0, 11);
                    // has the mode changed? if so, ask for new dsp string.
                    if(!e.Data.Substring(27, 2).Equals(_data.rawmode))
                        _cat.WriteCommand("ZZMN" + e.Data.Substring(27, 2) + ";");
                    _data.Mode = e.Data.Substring(27, 2);
                    _data.mox = (e.Data.Substring(26, 1).Equals("1")) ? true : false;
                    break;
                // vfob
                case "ZZFB":
                    _data.vfob = e.Data;
                    break;
                // band
                case "ZZBS":
                    _data.Band = e.Data;
                    break;
                // display mode
                case "ZZDM":
                    _data.DisplayMode = e.Data;
                    break;
                // agc
                case "ZZGT":
                    _data.AGC = e.Data;
                    break;
                // mode dsp filters
                case "ZZMN":
                    ProcessDSPFilters(e.Data);
                    break;
                case "ZZSM":
                    _data.smeter = e.Data.Substring(1);
                    break;
                case "ZZTX":
                    _data.mox = true;
                    break;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion

        #region Processing Functions
        private static void ProcessDSPFilters(String data)
        {
            //implement this crap
            data.Substring(2);
            //MatchCollection mc = Regex.Matches(s, "");
        }

        void ProcessHRDTCPGetCommand(String s, BinaryWriter bw)
        {
            // Console.WriteLine("GET COMMAND: {0}", s);

            if (s.Contains("GET ID"))            
                bw.Write(HRDMessage.HRDMessageToByteArray("Ham Radio Deluxe"));            
            else if (s.Contains("GET VERSION"))            
                bw.Write(HRDMessage.HRDMessageToByteArray("v0.1"));            
            else if (s.Contains("GET FREQUENCY"))            
                bw.Write(HRDMessage.HRDMessageToByteArray(_data.vfoa));            
            else if (s.Contains("GET RADIO"))         
                bw.Write(HRDMessage.HRDMessageToByteArray("PowerSDR"));            
            else if (s.Contains("GET CONTEXT"))            
                bw.Write(HRDMessage.HRDMessageToByteArray("1"));           
            else if (s.Contains("GET FREQUENCIES"))            
                bw.Write(HRDMessage.HRDMessageToByteArray(_data.vfoa + "-" + _data.vfob));            
            else if (s.Contains("GET DROPDOWN-TEXT"))            
                bw.Write(HRDMessage.HRDMessageToByteArray(GetDropdownText(s)));            
            else if (s.Contains("GET DROPDOWN-LIST"))            
                bw.Write(HRDMessage.HRDMessageToByteArray(GetDropdownList(s)));
            else if (s.Contains("GET LOGBOOKUPDATES"))
                bw.Write(HRDMessage.HRDMessageToByteArray("0"));
            else if (s.Contains("GET BUTTONS"))
                bw.Write(HRDMessage.HRDMessageToByteArray(GetButtons(s)));
            else if (s.Contains("GET SMETER-MAIN"))
            {
                Console.WriteLine("S-meter {0}", _data.smeter);
                bw.Write(HRDMessage.HRDMessageToByteArray(String.Format("S,{0},1.5", _data.smeter)));
            }
            else if (s.Contains("GET BUTTON-SELECT TX"))
                bw.Write(HRDMessage.HRDMessageToByteArray(_data.mox ? "1" : "0"));
            else
                bw.Write(HRDMessage.HRDMessageToByteArray("0"));            
        }

        void ProcessHRDTCPSetCommand(String s, BinaryWriter bw)
        {            
            Console.WriteLine("SET COMMAND: {0}", s);

            if (s.Contains("SET DROPDOWN"))
                SetDropdown(s);
            else if (s.Contains("SET FREQUENCIES-HZ"))
            {
                Match m = Regex.Match(s, "FREQUENCIES-HZ (\\d+) (\\d+)");
                if (!m.Success) return;
                _cat.WriteCommand(String.Format("ZZFA{0:00000000000};", long.Parse(m.Groups[1].Value)));
                _cat.WriteCommand(String.Format("ZZFB{0:00000000000};", long.Parse(m.Groups[2].Value)));
            }
            else if (s.Contains("SET BUTTON-SELECT"))
                _cat.WriteCommand(SetButton(s));
            else if (s.Contains("SET FREQUENCY-HZ"))
            {
                Match m = Regex.Match(s, "FREQUENCY-HZ (\\d+)");
                if(!m.Success) return;
                _cat.WriteCommand(String.Format("ZZFA{0:00000000000};", long.Parse(m.Groups[1].Value)));
            }
            // tell the program that the command executed OK, regardless if it did or not.
            bw.Write(HRDMessage.HRDMessageToByteArray("OK"));
        }
        #endregion

        #region Get Functions
        private static String GetButtons(String s)
        {
            return "TX";
        }
        
        private String GetDropdownText(String s)
        {
            StringBuilder output = new StringBuilder();            
            MatchCollection mc = Regex.Matches(s, "{([A-Z~]+)}", RegexOptions.Compiled);            

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
                        output.Append("AGC: " + _data.AGC + "\u0009");
                        break;
                    case "DISPLAY":
                        output.Append("Display: " + _data.DisplayMode + "\u0009");
                        break;
                    case "PREAMP":
                        output.Append("Preamp: High" + "\u0009");
                        break;
                    case "DSP~FLTR":
                        output.Append("DSP Fltr: 500Hz" + "\u0009");
                        break;
                }
            }
            
            // remove trailing \u0009 or else HRD Logbook wont parse it properly
            output.Remove(output.Length - 2, 2);
            return output.ToString();
        }

        private static String GetDropdownList(String s)
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
                    //replace with DSP list for current mode
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
        #endregion

        private void SetDropdown(String s)
        {
            Match m = Regex.Match(s, "SET DROPDOWN (\\w+) (\\w+) (\\d+)", RegexOptions.Compiled);
            if(!m.Success) return;

            switch(m.Groups[1].Value)
            {
                case "MODE":                    
                    _cat.WriteCommand(String.Format("ZZMD{0:00};", int.Parse(m.Groups[3].Value)));
                    break;
                //implement more sets
            }
        }

        private String SetButton(String s)
        {
            Match m = Regex.Match(s, "SET BUTTON-SELECT (\\w+) (\\d)", RegexOptions.Compiled);
            if(!m.Success) return String.Empty;

            switch(m.Groups[1].Value)
            {
                case "TX":                                        
                    String str = String.Format("ZZTX{0};", _data.mox ? "0" : "1" );
                    _data.mox = _data.mox ? false : true;
                    return str;
            }

            return "OK";
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Stop()
        {
            try
            {
                if (_timerShort != null) _timerShort.Stop();
                if (_timerLong != null) _timerLong.Stop();
                if (_cat != null) _cat.Close();
                if (_server != null) _server.Close();

                _cat = null;
                _server = null;
                _timerShort = null;
                _timerLong = null;                
                SetNotifyIconText("MiniDeluxe - Stopped");
            }
            catch
            {                                
            }            
        }

        public void Start()
        {
            try
            {
                _data = new RadioData { vfoa = "0", vfob = "0", Mode = "00", mox = false, DisplayMode = "0", smeter = "0"};
                _cat = new CATConnector(new SerialPort(Properties.Settings.Default.SerialPort));
                _timerShort = new Timer(Properties.Settings.Default.HighInterval);
                _timerLong = new Timer(Properties.Settings.Default.LowInterval);
                _server = new HRDTCPServer(this);

            }
            catch (Exception)
            {
                return;
            }

            // event handlers
            _cat.CATEvent += CatcatEvent;
            _timerShort.Elapsed += TimerShortElapsed;
            _timerLong.Elapsed += TimerLongElapsed;
            _server.HRDTCPEvent += ServerHRDTCPEvent;

            // write initial commands to the radio to fill in initial data
            _cat.WriteCommand("ZZIF;");
            _cat.WriteCommand("ZZFB;");
            _cat.WriteCommand("ZZBS;");
            _cat.WriteCommand("ZZDM;");
            _cat.WriteCommand("ZZGT;");

            _timerShort.Start();
            _timerLong.Start();
            _server.Start();

            SetNotifyIconText("MiniDeluxe - Running (0 connections)");
        }

        public void ShowOptionsForm()
        {
            MiniDeluxeForm form = new MiniDeluxeForm(this);            
            form.Show();
        }

        public void EndProgram()
        {
            Stop();
            _notifyIcon.EndProgram();
        }

        public bool HRDTCPServer_IsListening()
        {
            return _server != null && _server.IsListening;
        }

        public void SetNotifyIconText(String s)
        {
            _notifyIcon.SetNotifyText(s);
        }
    }  
}
