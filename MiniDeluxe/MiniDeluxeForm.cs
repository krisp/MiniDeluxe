using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace MiniDeluxe
{
    public partial class MiniDeluxeForm : Form
    {
        private readonly MiniDeluxe _parent;

        public MiniDeluxeForm(MiniDeluxe parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        private void MiniDeluxeForm_Load(object sender, EventArgs e)
        {
            lblStatus.Text = _parent.HRDTCPServer_IsListening() ? "Server enabled." : "Server disabled.";
            btnStartStop.Text = _parent.HRDTCPServer_IsListening() ? "Stop" : "Start";

            foreach (String port in SerialPort.GetPortNames())
                cbSerialport.Items.Add(port);

            cbSerialport.SelectedIndex = Properties.Settings.Default.SerialPortIdx;
            tbPort.Text = Properties.Settings.Default.Port.ToString();
            tbHigh.Text = Properties.Settings.Default.HighInterval.ToString();
            tbLow.Text = Properties.Settings.Default.LowInterval.ToString();
            cbLocalOnly.Checked = Properties.Settings.Default.LocalOnly;
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
                MessageBox.Show("Unable to save: " + ex.Message);
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
                Properties.Settings.Default.LocalOnly = true;
                Properties.Settings.Default.Save();
            }
            catch (Exception e)
            {                
                throw new Exception("Save failed: " + e.Message);
            }
        }
        
        private void btnStartStop_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSettings();
                switch (btnStartStop.Text)
                {
                    case "Start":
                        _parent.Start();
                        btnStartStop.Text = "Stop";
                        lblStatus.Text = "Server enabled.";
                        break;
                    case "Stop":
                        _parent.Stop();
                        btnStartStop.Text = "Start";
                        lblStatus.Text = "Server disabled.";
                        break;
                }
            }
            catch (Exception)
            {                
                MessageBox.Show("Unable to save: " + ex.Message);
            }            
        }
    }
}
