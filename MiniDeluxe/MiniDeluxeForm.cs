using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections;

namespace MiniDeluxe
{
    public partial class MiniDeluxeForm : Form
    {
        private readonly MiniDeluxe _parent;
        private RIOX.RIOXClient _c;

        public MiniDeluxeForm(MiniDeluxe parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        private void MiniDeluxeForm_Load(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.AutoPopDelay = 10000;
            t.InitialDelay = 1000;
            t.ReshowDelay = 500;

            lblStatus.Text = _parent.HRDTCPServer_IsListening() ? "Server enabled." : "Server disabled.";

            foreach (String port in SerialPort.GetPortNames())
                cbSerialport.Items.Add(port);

            try
            {
                //if(Properties.Settings.Default.SerialPortIdx != 0)
                    cbSerialport.SelectedIndex = Properties.Settings.Default.SerialPortIdx;
            }
            catch (Exception)
            {

            }

            tbPort.Text = Properties.Settings.Default.Port.ToString();
            tbHigh.Text = Properties.Settings.Default.HighInterval.ToString();
            tbLow.Text = Properties.Settings.Default.LowInterval.ToString();
            cbLocalOnly.Checked = Properties.Settings.Default.LocalOnly;
            txtRIOXIP.Text = Properties.Settings.Default.RIOXIP;
            txtRIOXport.Text = Properties.Settings.Default.RIOXPort.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try 
            {
                SaveSettings();
                _parent.Restart();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save: " + ex.Message + "\r\n" + ex.StackTrace);
                Close();
            }
        }

        private void SaveSettings()
        {
            try
            {
                Properties.Settings.Default.SerialPortIdx = cbSerialport.SelectedIndex;
                Properties.Settings.Default.SerialPort = cbSerialport.Text;
                Properties.Settings.Default.Port = int.Parse(tbPort.Text);
                Properties.Settings.Default.HighInterval = double.Parse(tbHigh.Text);
                Properties.Settings.Default.LowInterval = double.Parse(tbLow.Text);
                Properties.Settings.Default.FirstRun = false;
                Properties.Settings.Default.LocalOnly = cbLocalOnly.Checked;
                Properties.Settings.Default.RIOXIP = txtRIOXIP.Text;
                Properties.Settings.Default.RIOXPort = int.Parse(txtRIOXport.Text);
                Properties.Settings.Default.Save();
            }
            catch (Exception e)
            {                
                throw new Exception("Save failed: " + e.Message);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            //throw new Exception("AboutBoxException");
            AboutBox a  = new AboutBox();
            a.Show();
        }

        private void btnCheckForUpdate_Click(object sender, EventArgs e)
        {
            String updateUrl = CheckForUpdate.CheckForXMLUpdate("http://vhfwiki.com/xml/minideluxe.xml");
            if(updateUrl.Equals(String.Empty))
            {            
                MessageBox.Show("No updates available.");
                return;
            }

            const string title = "Update available!";
            const string question = "Open web browser to download new version?";
            if (DialogResult.Yes ==
             MessageBox.Show(this, question, title,
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question))
            {
                System.Diagnostics.Process.Start(updateUrl);
            }            
        }

        private void btnRIOXTest_Click(object sender, EventArgs e)
        {
            try
            {
                //DDUtilState.RadioData r = new DDUtilState.RadioData();
                RIOX.RIOXData r = new RIOX.RIOXData();                
                _c = new RIOX.RIOXClient(r.GetType(), txtRIOXIP.Text, int.Parse(txtRIOXport.Text));
                _c.SendCommand(new RIOX.RIOXCommand("UpDateType", "PSH:500"));
                _c.SendCommand(new RIOX.RIOXCommand("Sub", "ZZFA;"));
                _c.ObjectReceivedEvent += new RIOX.RIOXClient.ObjectReceivedEventHandler(c_ObjectReceivedEvent);                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failure: " + ex.Message);
            }
        }

        void c_ObjectReceivedEvent(object o, RIOX.RIOXClient.ObjectReceivedEventArgs e)
        {
            //DDUtilState.RadioData r = (DDUtilState.RadioData)e.DataObject;
            RIOX.RIOXData r = (RIOX.RIOXData)e.DataObject;
            Console.WriteLine("R: " + r.ToString());
            foreach (DictionaryEntry de in r)
            {
                Console.WriteLine("Key: {0} Value {1}", de.Key, de.Value);
            }
            MessageBox.Show("Success: Received Frequency: " + r["ZZFA"]);
            _c.Close();
        }
     }
}
