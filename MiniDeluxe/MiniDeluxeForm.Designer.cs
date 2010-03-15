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
            this.cbSerialport = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbLow = new System.Windows.Forms.TextBox();
            this.tbHigh = new System.Windows.Forms.TextBox();
            this.cbLocalOnly = new System.Windows.Forms.CheckBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnCheckForUpdate = new System.Windows.Forms.Button();
#if DEBUG
            this.btnDebug = new System.Windows.Forms.Button();
#endif
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbSerialport
            // 
            this.cbSerialport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSerialport.FormattingEnabled = true;
            this.cbSerialport.Location = new System.Drawing.Point(123, 13);
            this.cbSerialport.Name = "cbSerialport";
            this.cbSerialport.Size = new System.Drawing.Size(100, 21);
            this.cbSerialport.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "PowerSDR Serial Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "HRD Server Port:";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(123, 43);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(100, 20);
            this.tbPort.TabIndex = 3;
            this.tbPort.Text = "7809";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "High Priority:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Low Priority:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbLow);
            this.groupBox1.Controls.Add(this.tbHigh);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(6, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 81);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Update Intervals (msec)";
            // 
            // tbLow
            // 
            this.tbLow.Location = new System.Drawing.Point(166, 49);
            this.tbLow.Name = "tbLow";
            this.tbLow.Size = new System.Drawing.Size(51, 20);
            this.tbLow.TabIndex = 10;
            this.tbLow.Text = "5000";
            // 
            // tbHigh
            // 
            this.tbHigh.Location = new System.Drawing.Point(166, 22);
            this.tbHigh.Name = "tbHigh";
            this.tbHigh.Size = new System.Drawing.Size(51, 20);
            this.tbHigh.TabIndex = 9;
            this.tbHigh.Text = "1000";
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
#if DEBUG
            // 
            // btnDebug
            // 
            this.btnDebug.Location = new System.Drawing.Point(172, 160);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(64, 23);
            this.btnDebug.TabIndex = 16;
            this.btnDebug.Text = "Debug";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
#endif
            // 
            // MiniDeluxeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 247);
#if DEBUG
            this.Controls.Add(this.btnDebug);
#endif
            this.Controls.Add(this.btnCheckForUpdate);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.cbLocalOnly);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbSerialport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MiniDeluxeForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MiniDeluxe Options";
            this.Load += new System.EventHandler(this.MiniDeluxeForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbSerialport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbLow;
        private System.Windows.Forms.TextBox tbHigh;
        private System.Windows.Forms.CheckBox cbLocalOnly;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnCheckForUpdate;
#if DEBUG
        private System.Windows.Forms.Button btnDebug;
#endif
    }
}

