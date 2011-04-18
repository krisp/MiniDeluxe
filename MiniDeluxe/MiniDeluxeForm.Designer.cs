namespace MiniDeluxe
{
    partial class MiniDeluxeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiniDeluxeForm));
            this.btnSave = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbLocalOnly = new System.Windows.Forms.CheckBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnCheckForUpdate = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbLow = new System.Windows.Forms.TextBox();
            this.tbHigh = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSerialport = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnRIOXTest = new System.Windows.Forms.Button();
            this.txtRIOXport = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRIOXIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(172, 189);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(3, 194);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 13);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "lblStatus";
            // 
            // cbLocalOnly
            // 
            this.cbLocalOnly.AutoSize = true;
            this.cbLocalOnly.Checked = true;
            this.cbLocalOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLocalOnly.Location = new System.Drawing.Point(6, 158);
            this.cbLocalOnly.Name = "cbLocalOnly";
            this.cbLocalOnly.Size = new System.Drawing.Size(135, 17);
            this.cbLocalOnly.TabIndex = 13;
            this.cbLocalOnly.Text = "Local connections only";
            this.cbLocalOnly.UseVisualStyleBackColor = true;
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(123, 189);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(44, 23);
            this.btnAbout.TabIndex = 14;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnCheckForUpdate
            // 
            this.btnCheckForUpdate.Location = new System.Drawing.Point(123, 218);
            this.btnCheckForUpdate.Name = "btnCheckForUpdate";
            this.btnCheckForUpdate.Size = new System.Drawing.Size(113, 23);
            this.btnCheckForUpdate.TabIndex = 15;
            this.btnCheckForUpdate.Text = "Check for Update";
            this.btnCheckForUpdate.UseVisualStyleBackColor = true;
            this.btnCheckForUpdate.Click += new System.EventHandler(this.btnCheckForUpdate_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(241, 151);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.tbPort);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.cbSerialport);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(233, 125);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "PowerSDR";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbLow);
            this.groupBox1.Controls.Add(this.tbHigh);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(6, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 63);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Update Intervals (msec)";
            // 
            // tbLow
            // 
            this.tbLow.Location = new System.Drawing.Point(100, 38);
            this.tbLow.Name = "tbLow";
            this.tbLow.Size = new System.Drawing.Size(51, 20);
            this.tbLow.TabIndex = 10;
            this.tbLow.Text = "5000";
            // 
            // tbHigh
            // 
            this.tbHigh.Location = new System.Drawing.Point(100, 15);
            this.tbHigh.Name = "tbHigh";
            this.tbHigh.Size = new System.Drawing.Size(51, 20);
            this.tbHigh.TabIndex = 9;
            this.tbHigh.Text = "1000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "High Priority:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Low Priority:";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(117, 30);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(100, 20);
            this.tbPort.TabIndex = 15;
            this.tbPort.Text = "7809";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "HRD Server Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "PowerSDR Serial Port:";
            // 
            // cbSerialport
            // 
            this.cbSerialport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSerialport.FormattingEnabled = true;
            this.cbSerialport.Items.AddRange(new object[] {
            "RIOX (TCP/IP)"});
            this.cbSerialport.Location = new System.Drawing.Point(117, 6);
            this.cbSerialport.Name = "cbSerialport";
            this.cbSerialport.Size = new System.Drawing.Size(100, 21);
            this.cbSerialport.TabIndex = 12;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnRIOXTest);
            this.tabPage2.Controls.Add(this.txtRIOXport);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtRIOXIP);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(233, 125);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "RIOX/DDUtil";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnRIOXTest
            // 
            this.btnRIOXTest.Location = new System.Drawing.Point(122, 69);
            this.btnRIOXTest.Name = "btnRIOXTest";
            this.btnRIOXTest.Size = new System.Drawing.Size(75, 23);
            this.btnRIOXTest.TabIndex = 4;
            this.btnRIOXTest.Text = "Test";
            this.btnRIOXTest.UseVisualStyleBackColor = true;
            this.btnRIOXTest.Click += new System.EventHandler(this.btnRIOXTest_Click);
            // 
            // txtRIOXport
            // 
            this.txtRIOXport.Location = new System.Drawing.Point(97, 34);
            this.txtRIOXport.Name = "txtRIOXport";
            this.txtRIOXport.Size = new System.Drawing.Size(100, 20);
            this.txtRIOXport.TabIndex = 3;
            this.txtRIOXport.Text = "1234";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(60, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Port:";
            // 
            // txtRIOXIP
            // 
            this.txtRIOXIP.Location = new System.Drawing.Point(97, 7);
            this.txtRIOXIP.Name = "txtRIOXIP";
            this.txtRIOXIP.Size = new System.Drawing.Size(100, 20);
            this.txtRIOXIP.TabIndex = 1;
            this.txtRIOXIP.Text = "localhost";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "RIOX Server IP:";
            // 
            // MiniDeluxeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 247);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCheckForUpdate);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.cbLocalOnly);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MiniDeluxeForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MiniDeluxe Options";
            this.Load += new System.EventHandler(this.MiniDeluxeForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox cbLocalOnly;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnCheckForUpdate;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbLow;
        private System.Windows.Forms.TextBox tbHigh;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSerialport;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnRIOXTest;
        private System.Windows.Forms.TextBox txtRIOXport;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRIOXIP;
        private System.Windows.Forms.Label label5;
    }
}

