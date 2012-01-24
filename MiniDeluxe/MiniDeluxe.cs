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
using System.Collections;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Timers;
using System.IO;

namespace MiniDeluxe
{
    public class MiniDeluxe
    {
        private Timer _timerShort;
        private Timer _timerLong;
        private CATConnector _cat;
        private RIOX.RIOXClient _riox;
        private HRDTCPServer _server;
        private readonly NotifyIcon _notifyIcon;
        private bool _stopping;
        private bool _listenOnly;
        private bool _usingRIOX;

        RadioData _data;
        struct RadioData
        {
            private string _mode;
            private string _band;
            private string _displayMode;
            private string _agc;
            private string _smeter;
            private string _dspfilter;
            private string _preamp;

            public string vfoa;
            public string vfob;
            public string rawmode;
            public string dspfilters;
            public ArrayList dspfilterarray;
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
                            _mode = "FM";
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
                        case "99":
                            _mode = "OFF";
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
            public string Smeter
            {
                get { return _smeter; }
                set
                {
                    float i = (float.Parse(value) / 2f) - 121f;                    
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
            public string DSPFilter
            {
                get { return _dspfilter;  }
                set
                {
                    try
                    {
                        _dspfilter = (String) dspfilterarray[int.Parse(value)];                        
                    }
                    catch (Exception)
                    {
                        _dspfilter = "UNKN";
                    }
                }
            }
            public string Preamp
            {
                get { return _preamp; }
                set
                {
                    switch(value)
                    {
                        case "0":
                            _preamp = "Off";
                            break;
                        case "1":
                            _preamp = "Low";
                            break;
                        case "2":
                            _preamp = "Med";
                            break;
                        case "3":
                            _preamp = "High";
                            break;
                    }
                }
            }
        }
        
        public MiniDeluxe()
        {
            _notifyIcon = new NotifyIcon(this);

            Timer mmt = new Timer(1000 * 60 * 5);
            mmt.Elapsed += delegate {System.Diagnostics.Process.GetCurrentProcess().MaxWorkingSet =
                System.Diagnostics.Process.GetCurrentProcess().MinWorkingSet;};
            mmt.Start();

            if (Properties.Settings.Default.FirstRun)
                ShowOptionsForm();
            else
                Start();
        }

        void ServerHRDTCPEvent(object sender, HRDTCPEventArgs e)
        {
            String s = e.ToString().ToUpper();
            BinaryWriter bw = new BinaryWriter(e.Client.GetStream());

            s = s.Remove(s.IndexOf('\0'));

#if DEBUG
            //Debug(String.Format("RX: {0}", s));
#endif

            if (s.Contains("GET"))            
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
            _cat.WriteCommand("ZZFI;");
        }

        void TimerLongElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_usingRIOX)
            {
                _cat.WriteCommand("ZZBS;");
                _cat.WriteCommand("ZZDM;");
                _cat.WriteCommand("ZZGT;");
                _cat.WriteCommand("ZZPA;");
                return;
            }

            // RIOX connection check
            if (_riox.IsConnected == false)
            {
                SetNotifyIconText("MiniDeluxe - RIOX Disconnected (" + _server.ConnectionCount + " connections)");
                _riox.Connect();
            }            
        }   

        void CatcatEvent(object sender, CATEventArgs e)
        {
            switch(e.Command)
            {
                // vfoa, mode, xmit status
                case "ZZIF": case "IF":                
                    _data.vfoa = e.Data.Substring(0, 11);
                    // has the mode changed? if so, ask for new dsp string.
                    if (!_data.rawmode.Equals(e.Data.Substring(27, 2)))                    
                        WriteCommand("ZZMN" + e.Data.Substring(27, 2) + "");                    
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
                    _data.Smeter = e.Data.Substring(1);
                    break;
                case "ZZTX":
                    _data.mox = true;
                    break;
                case "ZZFI":
                    _data.DSPFilter = e.Data;
                    break;
                case "ZZPA":
                    _data.Preamp = e.Data;
                    break;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }        
        
        void ProcessDSPFilters(String data)
        {
            StringBuilder s = new StringBuilder();
            String filters = data.Substring(2);
            s.Append(filters.Substring(0, 5) + ",");
            s.Append(filters.Substring(15, 5) + ",");
            s.Append(filters.Substring(30, 5) + ",");
            s.Append(filters.Substring(45, 5) + ",");
            s.Append(filters.Substring(60, 5) + ",");
            s.Append(filters.Substring(75, 5) + ",");
            s.Append(filters.Substring(90, 5) + ",");
            s.Append(filters.Substring(105, 5) + ",");
            s.Append(filters.Substring(120, 5) + ",");
            s.Append(filters.Substring(135, 5) + ",");
            s.Append(filters.Substring(150, 5) + ",");
            s.Append(filters.Substring(165, 5));

            _data.dspfilterarray = new ArrayList
                                       {
                                           filters.Substring(0, 5),
                                           filters.Substring(15, 5),
                                           filters.Substring(30, 5),
                                           filters.Substring(45, 5),
                                           filters.Substring(60, 5),
                                           filters.Substring(75, 5),
                                           filters.Substring(90, 5),
                                           filters.Substring(105, 5),
                                           filters.Substring(120, 5),
                                           filters.Substring(135, 5),
                                           filters.Substring(150, 5),
                                           filters.Substring(165, 5),
                                           "UNKN"
                                       };

            _data.dspfilters = Regex.Replace(s.ToString(), " ", "", RegexOptions.Compiled);   
        }

        void ProcessHRDTCPGetCommand(String s, BinaryWriter bw)
        {            
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
                bw.Write(HRDMessage.HRDMessageToByteArray(GetButtons()));
            else if (s.Contains("GET SMETER-MAIN"))            
                bw.Write(HRDMessage.HRDMessageToByteArray(String.Format("S{0},{0},1.5", 14 - int.Parse(_data.Smeter))));            
            else if (s.Contains("GET BUTTON-SELECT TX"))
                bw.Write(HRDMessage.HRDMessageToByteArray(_data.mox ? "1" : "0"));
            else if (s.Contains("GET DROPDOWNS"))
                bw.Write(HRDMessage.HRDMessageToByteArray(GetDropdowns()));
            else
                bw.Write(HRDMessage.HRDMessageToByteArray("0"));            
        }

        void ProcessHRDTCPSetCommand(String s, BinaryWriter bw)
        {            
            Debug(String.Format("SET COMMAND: {0}", s));

            if (s.Contains("SET DROPDOWN"))
                SetDropdown(s);
            else if (s.Contains("SET FREQUENCIES-HZ"))
            {
                Match m = Regex.Match(s, "FREQUENCIES-HZ (\\d+) (\\d+)");
                if (!m.Success) return;
                String vfoa = String.Format("{0:00000000000}", long.Parse(m.Groups[1].Value));
                String vfob = String.Format("{0:00000000000}", long.Parse(m.Groups[2].Value));
                if (vfoa != "00000000000")
			WriteCommand("ZZFA" + vfoa + ";");
        	if (vfob != "00000000000")
			WriteCommand("ZZFB" + vfob + ";");
                _data.vfoa = vfoa;
                _data.vfob = vfob;
            }
            else if (s.Contains("SET BUTTON-SELECT"))
                SetButton(s);
            else if (s.Contains("SET FREQUENCY-HZ"))
            {
                Match m = Regex.Match(s, "FREQUENCY-HZ (\\d+)");
                if(!m.Success) return;
                String vfoa = String.Format("{0:00000000000}", long.Parse(m.Groups[1].Value));
		if (vfoa != "00000000000")
	                WriteCommand("ZZFA" + vfoa + ";");
                _data.vfoa = vfoa;
            }
            // tell the program that the command executed OK, regardless if it did or not.
            bw.Write(HRDMessage.HRDMessageToByteArray("OK"));
        }
                
        static String GetButtons()
        {
            return "TX";
        }

        static String GetDropdowns()
        {
            return "Mode,Band,AGC,Display,Preamp,DSP Fltr";
        }

        String GetDropdownText(String s)
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
                        output.Append("Preamp: " + _data.Preamp + "\u0009");
                        break;
                    case "DSP~FLTR":
                        output.Append("DSP Fltr:" + _data.DSPFilter + "" + "\u0009");
                        break;
                    default:
                        output.Append(m.Groups[1].Value + ": " + "\u0009");
                        break;
                }
            }
            
            // remove trailing \u0009 or else HRD Logbook wont parse it properly       
            return output.ToString().Remove(output.ToString().LastIndexOf('\u0009'));
        }

        String GetDropdownList(String s)
        {
            String q = s.Substring(s.IndexOf("{") + 1, (s.IndexOf("}") - s.IndexOf("{") - 1));
            String output;

            switch (q)
            {
                case "MODE":
                    output = "LSB,USB,DSB,CWL,CWU,FM,AM,DIGU,SPEC,DIGL,SAM,DRM";
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
                    output = _data.dspfilters;
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

        void SetDropdown(String s)
        {
            Match m = Regex.Match(s, "SET DROPDOWN ([\\w~]+) ([\\w.]+) (\\d+)", RegexOptions.Compiled);
            if(!m.Success) return;

            switch(m.Groups[1].Value)
            {
                case "MODE":
                    String mode = String.Format("{0:00}", int.Parse(m.Groups[3].Value));
                    WriteCommand("ZZMD" + mode + ";");
                    WriteCommand("ZZMN" + mode + ";");
                    _data.Mode = mode;
                    if(!_usingRIOX) WriteCommand("ZZFI;");
                    break;
                case "DSP~FLTR":                    
                    String fltr = String.Format("{0:00}", int.Parse(m.Groups[3].Value));                    
                    WriteCommand("ZZFI" + fltr + ";");
                    _data.DSPFilter = fltr;
                    break;
                case "AGC":
                    WriteCommand("ZZGT" + m.Groups[3].Value + ";");
                    _data.AGC = m.Groups[3].Value;
                    break;
                case "BAND":
                    String band = Regex.Replace(m.Groups[2].Value, "M", "");                    
                    if (band.Equals("GEN"))
                    {
                        WriteCommand("ZZBS888;");
                        _data.Band = "888";
                        break;
                    }
                    if (band.Equals("WWV"))
                    {
                        WriteCommand("ZZBS999;");
                        _data.Band = "999";
                        break;
                    }
                    if(band.Contains("V")) return; // not implementing vhf band switching yet
                    
                    String output = String.Format("{0:000}", int.Parse(band));
                    WriteCommand("ZZBS" + output + ";");
                    _data.Band = output;
                    break;
                case "DISPLAY":
                    WriteCommand("ZZDM" + m.Groups[3].Value + ";");
                    _data.DisplayMode = m.Groups[3].Value;
                    break;
                case "PREAMP":
                    WriteCommand("ZZPA" + m.Groups[3].Value + ";");
                    _data.Preamp = m.Groups[3].Value;
                    break;
            }
        }

        String SetButton(String s)
        {
            Match m = Regex.Match(s, "SET BUTTON-SELECT (\\w+) (\\d)", RegexOptions.Compiled);
            if(!m.Success) return String.Empty;

            switch(m.Groups[1].Value)
            {
                case "TX":                                                     
                    WriteCommand("ZZTX" + (_data.mox ? "0" : "1") + ";");
                    _data.mox = _data.mox ? false : true;
                    break;                    
            }

            return "OK";
        }

        public void ShowOptionsForm()
        {
            try
            {
                MiniDeluxeForm form = new MiniDeluxeForm(this);
                form.Show();
            }
            catch (Exception e)
            {
                Debug(e.Message);
                Debug(e.StackTrace);
            }
        }

        public bool HRDTCPServer_IsListening()
        {
            if(_server != null)
            {
                return _server.IsListening;
            }
            return false;
        }

        public void SetNotifyIconText(String s)
        {
            _notifyIcon.SetNotifyText(s);
        }

        public void Restart()
        {
            Stop();

            while (_stopping)
            {
            }

            Start();
        }

        public void Stop()
        {
            _stopping = true;
            try
            {
                if (_timerShort != null) _timerShort.Stop();
                if (_timerLong != null) _timerLong.Stop();
                if (_cat != null) _cat.Close();
                if (_server != null) _server.Close();
                if (_riox != null) _riox.Close();

                _cat = null;
                _server = null;
                _timerShort = null;
                _timerLong = null;
                SetNotifyIconText("MiniDeluxe - Stopped");
            }
            catch (Exception e)
            {
                _notifyIcon.MessageBox("While stopping: " + e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                _stopping = false;
            }

        }

        public void Start()
        {
            try
            {
                _data = new RadioData
                {
                    vfoa = "0",
                    vfob = "0",
                    Mode = "99",
                    mox = false,
                    DisplayMode = "0",
                    Smeter = "0",
                    dspfilters = "",
                    dspfilterarray = new ArrayList
                                         {
                                         "6.0","4.0","2.6","2.1","1.0",
                                         "500","250","100","50","25","VAR1","VAR2","UNKN"
                                         },
                };

                if (Properties.Settings.Default.SerialPortIdx == 0)
                {
                    // RIOX
                    _riox = new RIOX.RIOXClient(typeof(DDUtilState.RadioData), Properties.Settings.Default.RIOXIP, Properties.Settings.Default.RIOXPort);
                    _riox.ObjectReceivedEvent += new RIOX.RIOXClient.ObjectReceivedEventHandler(_riox_ObjectReceivedEvent);
                    _usingRIOX = true;
                }
                else
                {
                    _cat = new CATConnector(new SerialPort(Properties.Settings.Default.SerialPort));
                    _timerShort = new Timer(Properties.Settings.Default.HighInterval);                    
                    _cat.CATEvent += CatcatEvent;
                    _timerShort.Elapsed += TimerShortElapsed;                    
                }

                _timerLong = new Timer(Properties.Settings.Default.LowInterval);
                _timerLong.Elapsed += TimerLongElapsed;

                //_listenOnly = Properties.Settings.Default.ListenOnly;
                _server = new HRDTCPServer(this);

            }
            catch (Exception e)
            {
                _notifyIcon.MessageBox("While starting: " + e.Message + "\n" +
                                       "Server is disabled. Please check configuration.\nIf using RIOX mode, please make sure DDUtil is running.");
                ShowOptionsForm();
                return;
            }

            _server.HRDTCPEvent += ServerHRDTCPEvent;

            // Start the timers only if serial polling is enabled
            if (!_usingRIOX)
            {
                // write initial commands to the radio to fill in initial data
                WriteCommand("ZZIF;");
                WriteCommand("ZZFB;");
                WriteCommand("ZZBS;");
                WriteCommand("ZZDM;");
                WriteCommand("ZZGT;");
           // WriteCommand("ZZFI;");
                WriteCommand("ZZPA;");
                WriteCommand("ZZFI;");

                _timerShort.Start();
                _timerLong.Start();
            }

            _timerLong.Start();
            _server.Start();

            SetNotifyIconText("MiniDeluxe - Running (0 connections)");
        }

        void _riox_ObjectReceivedEvent(object o, RIOX.RIOXClient.ObjectReceivedEventArgs e)
        {
            DDUtilState.RadioData rd = (DDUtilState.RadioData)e.DataObject;
            _data.AGC = rd.agc;
            _data.Band = rd.bandr1;
            _data.DisplayMode = rd.dispmode;
            _data.DSPFilter = rd.fltr1;
            _data.Mode = rd.moder1;
            _data.Smeter = rd.smtr;
            _data.vfoa = rd.vfoa;
            _data.vfob = rd.vfob;
            _data.mox = rd.mox;
            // _data.Preamp = rd.preamp;
        }
        
        public void EndProgram()
        {
            Stop();
            _notifyIcon.EndProgram();
        }

        public static void Debug(String s)
        {
            System.Diagnostics.Debug.WriteLine(s);
            System.Diagnostics.Debug.Flush();
        }

        private void WriteCommand(String data)
        {
            if (_usingRIOX)
            {
                Debug("Writting RIOX command: " + data);
                try
                {
                    _riox.SendCommand(new RIOX.RIOXCommand("ClientCmd", data));
                }
                catch (Exception)
                {
                    // try reconnecting
                    try
                    {
                        _riox.Close();
                        _riox.Connect();
                    }
                    catch (Exception)
                    {
                        // could not reconnect
                    }
                }
            }
            else
            {
                Debug("Writting CAT command: " + data);
                _cat.WriteCommand(data);
            }
        }
    }  
}
