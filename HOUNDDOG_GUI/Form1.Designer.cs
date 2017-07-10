namespace HOUNDDOG_GUI
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.cartesianChart2 = new LiveCharts.WinForms.CartesianChart();
            this.saveLocationButton = new MetroFramework.Controls.MetroButton();
            this.startButton = new MetroFramework.Controls.MetroButton();
            this.stopButton = new MetroFramework.Controls.MetroButton();
            this.clearButton = new MetroFramework.Controls.MetroButton();
            this.comboBox1 = new MetroFramework.Controls.MetroComboBox();
            this.label2 = new MetroFramework.Controls.MetroLabel();
            this.saveLoc = new MetroFramework.Controls.MetroLabel();
            this.textBox1 = new MetroFramework.Controls.MetroTextBox();
            this.label3 = new MetroFramework.Controls.MetroLabel();
            this.verboseCheck = new MetroFramework.Controls.MetroCheckBox();
            this.label1 = new MetroFramework.Controls.MetroLabel();
            this.specDisplayEnable = new MetroFramework.Controls.MetroCheckBox();
            this.refreshRateSlider = new MetroFramework.Controls.MetroTrackBar();
            this.refreshRate = new MetroFramework.Controls.MetroLabel();
            this.dataGridView1 = new MetroFramework.Controls.MetroGrid();
            this.dataFormat = new MetroFramework.Controls.MetroLabel();
            this.fromFile = new MetroFramework.Controls.MetroCheckBox();
            this.fileLoadChoose = new MetroFramework.Controls.MetroButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.loadLoc = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(799, 50);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(375, 100);
            this.cartesianChart1.TabIndex = 13;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // cartesianChart2
            // 
            this.cartesianChart2.Location = new System.Drawing.Point(27, 427);
            this.cartesianChart2.Name = "cartesianChart2";
            this.cartesianChart2.Size = new System.Drawing.Size(1148, 240);
            this.cartesianChart2.TabIndex = 14;
            this.cartesianChart2.Text = "cartesianChart2";
            // 
            // saveLocationButton
            // 
            this.saveLocationButton.Location = new System.Drawing.Point(165, 27);
            this.saveLocationButton.Name = "saveLocationButton";
            this.saveLocationButton.Size = new System.Drawing.Size(128, 23);
            this.saveLocationButton.TabIndex = 19;
            this.saveLocationButton.Text = "Change Save Location";
            this.saveLocationButton.UseSelectable = true;
            this.saveLocationButton.Click += new System.EventHandler(this.saveLocationButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(25, 63);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 25);
            this.startButton.TabIndex = 20;
            this.startButton.Text = "Start";
            this.startButton.UseSelectable = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(25, 94);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 25);
            this.stopButton.TabIndex = 21;
            this.stopButton.Text = "Stop";
            this.stopButton.UseSelectable = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(25, 125);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 25);
            this.clearButton.TabIndex = 22;
            this.clearButton.Text = "Clear";
            this.clearButton.UseSelectable = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.ItemHeight = 23;
            this.comboBox1.Location = new System.Drawing.Point(106, 59);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(543, 29);
            this.comboBox1.TabIndex = 23;
            this.comboBox1.UseSelectable = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(656, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 19);
            this.label2.TabIndex = 24;
            this.label2.Text = "Interface";
            // 
            // saveLoc
            // 
            this.saveLoc.AutoSize = true;
            this.saveLoc.Location = new System.Drawing.Point(299, 31);
            this.saveLoc.Name = "saveLoc";
            this.saveLoc.Size = new System.Drawing.Size(26, 19);
            this.saveLoc.TabIndex = 25;
            this.saveLoc.Text = "C:\\";
            // 
            // textBox1
            // 
            // 
            // 
            // 
            this.textBox1.CustomButton.Image = null;
            this.textBox1.CustomButton.Location = new System.Drawing.Point(123, 1);
            this.textBox1.CustomButton.Name = "";
            this.textBox1.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.textBox1.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.textBox1.CustomButton.TabIndex = 1;
            this.textBox1.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textBox1.CustomButton.UseSelectable = true;
            this.textBox1.CustomButton.Visible = false;
            this.textBox1.Lines = new string[0];
            this.textBox1.Location = new System.Drawing.Point(106, 97);
            this.textBox1.MaxLength = 32767;
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '\0';
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox1.SelectedText = "";
            this.textBox1.SelectionLength = 0;
            this.textBox1.SelectionStart = 0;
            this.textBox1.ShortcutsEnabled = true;
            this.textBox1.Size = new System.Drawing.Size(145, 23);
            this.textBox1.TabIndex = 26;
            this.textBox1.UseSelectable = true;
            this.textBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.textBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(257, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 19);
            this.label3.TabIndex = 27;
            this.label3.Text = "# of Packets to Capture";
            // 
            // verboseCheck
            // 
            this.verboseCheck.AutoSize = true;
            this.verboseCheck.Checked = true;
            this.verboseCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.verboseCheck.Location = new System.Drawing.Point(445, 104);
            this.verboseCheck.Name = "verboseCheck";
            this.verboseCheck.Size = new System.Drawing.Size(64, 15);
            this.verboseCheck.TabIndex = 28;
            this.verboseCheck.Text = "Verbose";
            this.verboseCheck.UseSelectable = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 398);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 19);
            this.label1.TabIndex = 29;
            this.label1.Text = "# of Packets: 0";
            // 
            // specDisplayEnable
            // 
            this.specDisplayEnable.AutoSize = true;
            this.specDisplayEnable.Location = new System.Drawing.Point(222, 401);
            this.specDisplayEnable.Name = "specDisplayEnable";
            this.specDisplayEnable.Size = new System.Drawing.Size(144, 15);
            this.specDisplayEnable.TabIndex = 30;
            this.specDisplayEnable.Text = "Enable Spectral Display";
            this.specDisplayEnable.UseSelectable = true;
            this.specDisplayEnable.CheckedChanged += new System.EventHandler(this.specDisplayEnable_CheckedChanged);
            // 
            // refreshRateSlider
            // 
            this.refreshRateSlider.BackColor = System.Drawing.Color.Transparent;
            this.refreshRateSlider.Location = new System.Drawing.Point(372, 398);
            this.refreshRateSlider.Maximum = 5000;
            this.refreshRateSlider.Minimum = 1;
            this.refreshRateSlider.Name = "refreshRateSlider";
            this.refreshRateSlider.Size = new System.Drawing.Size(255, 23);
            this.refreshRateSlider.TabIndex = 31;
            this.refreshRateSlider.Text = "Refresh Rate";
            this.refreshRateSlider.Value = 150;
            this.refreshRateSlider.Scroll += new System.Windows.Forms.ScrollEventHandler(this.refreshRateSlider_Scroll);
            // 
            // refreshRate
            // 
            this.refreshRate.AutoSize = true;
            this.refreshRate.Location = new System.Drawing.Point(633, 401);
            this.refreshRate.Name = "refreshRate";
            this.refreshRate.Size = new System.Drawing.Size(129, 19);
            this.refreshRate.TabIndex = 32;
            this.refreshRate.Text = "Refresh Rate: 150 ms";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dataGridView1.Location = new System.Drawing.Point(25, 156);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1149, 236);
            this.dataGridView1.TabIndex = 33;
            // 
            // dataFormat
            // 
            this.dataFormat.AutoSize = true;
            this.dataFormat.Location = new System.Drawing.Point(864, 401);
            this.dataFormat.Name = "dataFormat";
            this.dataFormat.Size = new System.Drawing.Size(86, 19);
            this.dataFormat.TabIndex = 34;
            this.dataFormat.Text = "Data Format:";
            // 
            // fromFile
            // 
            this.fromFile.AutoSize = true;
            this.fromFile.Location = new System.Drawing.Point(106, 135);
            this.fromFile.Name = "fromFile";
            this.fromFile.Size = new System.Drawing.Size(103, 15);
            this.fromFile.TabIndex = 35;
            this.fromFile.Text = "Parse From File";
            this.fromFile.UseSelectable = true;
            this.fromFile.CheckedChanged += new System.EventHandler(this.fromFile_CheckedChanged);
            // 
            // fileLoadChoose
            // 
            this.fileLoadChoose.Location = new System.Drawing.Point(215, 126);
            this.fileLoadChoose.Name = "fileLoadChoose";
            this.fileLoadChoose.Size = new System.Drawing.Size(108, 24);
            this.fileLoadChoose.TabIndex = 36;
            this.fileLoadChoose.Text = "Choose Pcap File";
            this.fileLoadChoose.UseSelectable = true;
            this.fileLoadChoose.Visible = false;
            this.fileLoadChoose.Click += new System.EventHandler(this.fileLoadChoose_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // loadLoc
            // 
            this.loadLoc.AutoSize = true;
            this.loadLoc.Location = new System.Drawing.Point(330, 131);
            this.loadLoc.Name = "loadLoc";
            this.loadLoc.Size = new System.Drawing.Size(26, 19);
            this.loadLoc.TabIndex = 37;
            this.loadLoc.Text = "C:\\";
            this.loadLoc.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1197, 676);
            this.Controls.Add(this.loadLoc);
            this.Controls.Add(this.fileLoadChoose);
            this.Controls.Add(this.fromFile);
            this.Controls.Add(this.dataFormat);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.refreshRate);
            this.Controls.Add(this.refreshRateSlider);
            this.Controls.Add(this.specDisplayEnable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.verboseCheck);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.saveLoc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.saveLocationButton);
            this.Controls.Add(this.cartesianChart2);
            this.Controls.Add(this.cartesianChart1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Resizable = false;
            this.Text = "HOUNDDOG";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private LiveCharts.WinForms.CartesianChart cartesianChart2;
        private MetroFramework.Controls.MetroButton saveLocationButton;
        private MetroFramework.Controls.MetroButton startButton;
        private MetroFramework.Controls.MetroButton stopButton;
        private MetroFramework.Controls.MetroButton clearButton;
        private MetroFramework.Controls.MetroComboBox comboBox1;
        private MetroFramework.Controls.MetroLabel label2;
        private MetroFramework.Controls.MetroLabel saveLoc;
        private MetroFramework.Controls.MetroTextBox textBox1;
        private MetroFramework.Controls.MetroLabel label3;
        private MetroFramework.Controls.MetroCheckBox verboseCheck;
        private MetroFramework.Controls.MetroLabel label1;
        private MetroFramework.Controls.MetroCheckBox specDisplayEnable;
        private MetroFramework.Controls.MetroTrackBar refreshRateSlider;
        private MetroFramework.Controls.MetroLabel refreshRate;
        private MetroFramework.Controls.MetroGrid dataGridView1;
        private MetroFramework.Controls.MetroLabel dataFormat;
        private MetroFramework.Controls.MetroCheckBox fromFile;
        private MetroFramework.Controls.MetroButton fileLoadChoose;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private MetroFramework.Controls.MetroLabel loadLoc;
    }
}

