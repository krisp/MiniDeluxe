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
            lblStatus.Text = "";            
            foreach (String port in SerialPort.GetPortNames())
                cbSerialport.Items.Add(port);

            cbSerialport.SelectedItem = Properties.Settings.Default.SerialPortIdx;
            tbPort.Text = Properties.Settings.Default.Port.ToString();
            tbHigh.Text = Properties.Settings.Default.HighInterval.ToString();
            tbLow.Text = Properties.Settings.Default.LowInterval.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.SerialPortIdx = (int)cbSerialport.SelectedIndex;
                Properties.Settings.Default.SerialPort = cbSerialport.Text;
                Properties.Settings.Default.Port = int.Parse(tbPort.Text);
                Properties.Settings.Default.HighInterval = double.Parse(tbHigh.Text);
                Properties.Settings.Default.LowInterval = double.Parse(tbLow.Text);
                Properties.Settings.Default.FirstRun = false;
                Properties.Settings.Default.Save();
                _parent.Restart();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save: " + ex.Message);
                Close();
            }
        }
    }
}
