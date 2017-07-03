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
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.refreshButton = new System.Windows.Forms.Button();
            this.saveLocationButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.saveLoc = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.verboseCheck = new System.Windows.Forms.CheckBox();
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.cartesianChart2 = new LiveCharts.WinForms.CartesianChart();
            this.updateChart = new System.Windows.Forms.Button();
            this.specDisplayEnable = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(14, 54);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(85, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(14, 83);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(85, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 384);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "# of Packets: 0";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(109, 54);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(546, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(661, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Interface";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(15, 139);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1147, 229);
            this.dataGridView1.TabIndex = 5;
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(14, 110);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(85, 23);
            this.refreshButton.TabIndex = 6;
            this.refreshButton.Text = "Clear";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // saveLocationButton
            // 
            this.saveLocationButton.Location = new System.Drawing.Point(15, 12);
            this.saveLocationButton.Name = "saveLocationButton";
            this.saveLocationButton.Size = new System.Drawing.Size(85, 36);
            this.saveLocationButton.TabIndex = 7;
            this.saveLocationButton.Text = "Change Save Location";
            this.saveLocationButton.UseVisualStyleBackColor = true;
            this.saveLocationButton.Click += new System.EventHandler(this.saveLocationButton_Click);
            // 
            // saveLoc
            // 
            this.saveLoc.AutoSize = true;
            this.saveLoc.Location = new System.Drawing.Point(106, 24);
            this.saveLoc.Name = "saveLoc";
            this.saveLoc.Size = new System.Drawing.Size(22, 13);
            this.saveLoc.TabIndex = 8;
            this.saveLoc.Text = "C:\\";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(109, 110);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1053, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(112, 85);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(135, 20);
            this.textBox1.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(253, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "# of Packets to Capture";
            // 
            // verboseCheck
            // 
            this.verboseCheck.AutoSize = true;
            this.verboseCheck.Location = new System.Drawing.Point(716, 59);
            this.verboseCheck.Name = "verboseCheck";
            this.verboseCheck.Size = new System.Drawing.Size(65, 17);
            this.verboseCheck.TabIndex = 12;
            this.verboseCheck.Text = "Verbose";
            this.verboseCheck.UseVisualStyleBackColor = true;
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(787, 6);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(375, 100);
            this.cartesianChart1.TabIndex = 13;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // cartesianChart2
            // 
            this.cartesianChart2.Location = new System.Drawing.Point(14, 411);
            this.cartesianChart2.Name = "cartesianChart2";
            this.cartesianChart2.Size = new System.Drawing.Size(1148, 240);
            this.cartesianChart2.TabIndex = 14;
            this.cartesianChart2.Text = "cartesianChart2";
            // 
            // updateChart
            // 
            this.updateChart.Location = new System.Drawing.Point(298, 378);
            this.updateChart.Name = "updateChart";
            this.updateChart.Size = new System.Drawing.Size(75, 23);
            this.updateChart.TabIndex = 15;
            this.updateChart.Text = "Refresh";
            this.updateChart.UseVisualStyleBackColor = true;
            this.updateChart.Click += new System.EventHandler(this.updateChart_Click);
            // 
            // specDisplayEnable
            // 
            this.specDisplayEnable.AutoSize = true;
            this.specDisplayEnable.Location = new System.Drawing.Point(154, 384);
            this.specDisplayEnable.Name = "specDisplayEnable";
            this.specDisplayEnable.Size = new System.Drawing.Size(138, 17);
            this.specDisplayEnable.TabIndex = 16;
            this.specDisplayEnable.Text = "Enable Spectral Display";
            this.specDisplayEnable.UseVisualStyleBackColor = true;
            this.specDisplayEnable.CheckedChanged += new System.EventHandler(this.specDisplayEnable_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 675);
            this.Controls.Add(this.specDisplayEnable);
            this.Controls.Add(this.updateChart);
            this.Controls.Add(this.cartesianChart2);
            this.Controls.Add(this.cartesianChart1);
            this.Controls.Add(this.verboseCheck);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.saveLoc);
            this.Controls.Add(this.saveLocationButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Name = "Form1";
            this.Text = "Vita-49 Parser";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button saveLocationButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label saveLoc;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox verboseCheck;
        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private LiveCharts.WinForms.CartesianChart cartesianChart2;
        private System.Windows.Forms.Button updateChart;
        private System.Windows.Forms.CheckBox specDisplayEnable;
    }
}

