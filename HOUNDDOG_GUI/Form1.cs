using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Windows.Threading;
using LiveCharts.Geared;
using MetroFramework.Forms;
using MetroFramework.Controls;
using MetroFramework;

namespace HOUNDDOG_GUI
{
    public partial class Form1 : MetroForm
    {
        sockets sock;
        bool limitSet = false;
        int packetLimit;
        Random rand = new Random();
        int prevPackCount = 0;
        System.Timers.Timer timer = new System.Timers.Timer();
        System.Timers.Timer timer2 = new System.Timers.Timer();
        int dataPacks = 0;
        int contPacks = 0;
        int otherOrInvalidPacks = 0;
        string fileLoadLocation = @"";
        PcapReader pRead;
        Point pt = new Point();

        public class MeasureModel
        {
            public System.DateTime DateTime { get; set; }
            public int Value { get; set; }
        }

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            sock = new sockets(this);
            saveLoc.Text = sock.FileLoc;
            loadLoc.Text = fileLoadLocation;
            verboseCheck.Checked = true;
            updateComboBox();
            BindGrid();

            setupPacketCountChart();
            setupDataPayloadChart();
            
            cartesianChart2.Visible = false;
            cartesianChart2.Enabled = false;
            refreshRate.Visible = false;
            refreshRateSlider.Visible = false;
            dataFormat.Visible = false;

            Height -= 250;
        }

        public int DataPack
        {
            get { return dataPacks; }
            set { dataPacks = value; }
        }

        public int ContextPack
        {
            get { return contPacks; }
            set { contPacks = value; }
        }

        public int OtherPacks
        {
            get { return otherOrInvalidPacks; }
            set { otherOrInvalidPacks = value; }
        }

        public sockets getSock()
        {
            return sock;
        }

        public bool getFileorLive()
        {
            return fromFile.Checked;
        }

        public string getFileLoadLoc()
        {
            return openFileDialog1.FileName;
        }

        public bool getVerboseValue()
        {
            return verboseCheck.Checked;
        }

        public bool getSpectralDisplayEnableValue()
        {
            return specDisplayEnable.Checked;
        }

        public void updateDataFormat(string format)
        {
            dataFormat.Text = "Data Format: " + format;
        }

        public void updatePacketNum(string s, long num)
        {
            label1.Text = s;
            if (limitSet == true && num >= packetLimit)
            {
                dataGridView1.Refresh();
                sock.CloseConnection();
            }
        }

        private void updateComboBox()
        {
            foreach (string s in sock.adapt)
                comboBox1.Items.Add(s);
            comboBox1.SelectedIndex = 0;
        }

        private void BindGrid()
        {
            DataTable temp = sock.getTable();
            BindingSource sBind = new BindingSource();
            sBind.DataSource = temp;
            dataGridView1.DataSource = sBind;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (fromFile.Checked != true)
            {
                bool badInp = false;
                packetLimit = 0;
                if (textBox1.Text.Length > 0)
                {
                    bool result = Int32.TryParse(textBox1.Text, out packetLimit);
                    if (result != true)
                    {
                        MetroMessageBox.Show(this, "Please enter a valid integer. Up to 1M packets.");
                        badInp = true;
                    }
                    else
                    {
                        if (packetLimit > 1000000)
                        {
                            MetroMessageBox.Show(this, "Please enter a valid integer. Up to 1M packets.");
                            badInp = true;
                        }
                        else
                        {
                            limitSet = true;
                            badInp = false;
                        }
                    }
                }
                if (badInp == false)
                {
                    sock.DeviceInd = comboBox1.SelectedIndex;
                    sock.Start();
                    startButton.Enabled = false;

                    if (specDisplayEnable.Checked == true)
                    {
                        if (sock.NormalizedPayload.Count > 0)
                        {
                            updatePayloadChart(sock.NormalizedPayload);
                        }
                        timerStart();
                    }
                }
            }
            else
            {
                if (fileLoadLocation.Length == 0)
                {
                    MetroMessageBox.Show(this, "Please choose a Pcap file to load.");
                }
                else
                {
                    fromFile.Enabled = false;
                    fileLoadChoose.Enabled = false;
                    startButton.Enabled = false;
                    BackgroundWorker backgroundWorker1 = new BackgroundWorker();
                    backgroundWorker1.RunWorkerAsync();
                    backgroundWorker1.DoWork += backgroundWorker1_DoWork;
                    backgroundWorker1.WorkerReportsProgress = true;
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            pRead = new PcapReader(fileLoadLocation, sock, this);
            fromFile.Enabled = true;
            fileLoadChoose.Enabled = true;
            startButton.Enabled = true;
            timer2.Stop();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            BindGrid();
            dataGridView1.Refresh();
            sock.CloseConnection();
            timer.Stop();
            startButton.Enabled = true;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            //BindGrid();
            //dataGridView1.Refresh();
            //updateProgress();
            sock.clearTable();
        }

        private void saveLocationButton_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                sock.FileLoc = saveFileDialog1.FileName;
                saveLoc.Text = saveFileDialog1.FileName;
                sock.setBadPack();
            }
        }

        public GearedValues<MeasureModel> ChartValues { get; set; }
        //public ChartValues<MeasureModel> ChartValues { get; set; }
        public System.Windows.Forms.Timer Timer { get; set; }
        public Random R { get; set; }

        private void SetAxisLimits(System.DateTime now)
        {
            cartesianChart1.AxisX[0].MaxValue = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 100ms ahead
            cartesianChart1.AxisX[0].MinValue = now.Ticks - TimeSpan.FromSeconds(8).Ticks; //we only care about the last 8 seconds
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            var now = System.DateTime.Now;

            ChartValues.Add(new MeasureModel
            {
                DateTime = now,
                Value = sock.packCount - prevPackCount //R.Next(0, 10)
            });
            prevPackCount = sock.packCount;
            SetAxisLimits(now);

            //lets only use the last 30 values
            if (ChartValues.Count > 30) ChartValues.RemoveAt(0);
        }

        public void setupPacketCountChart()
        {
            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            //the ChartValues property will store our values array
            ChartValues = new GearedValues<MeasureModel>();
            ChartValues.WithQuality(Quality.Low);
            //ChartValues = new ChartValues<MeasureModel>();
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = ChartValues,
                    PointGeometrySize = 0,
                    StrokeThickness = 4
                }
            };
            cartesianChart1.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => new System.DateTime((long)value).ToString("mm:ss"),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(1).Ticks
                }
            });

            SetAxisLimits(System.DateTime.Now);

            //The next code simulates data changes every 500 ms
            Timer = new System.Windows.Forms.Timer
            {
                Interval = 500
            };
            Timer.Tick += TimerOnTick;
            R = new Random();
            Timer.Start();
        }

        public void setupDataPayloadChart()
        {
            //cartesianChart2.Series = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Title = "Series 1",
            //        Values = new ChartValues<double> {4, 6, 5, 2, 7}
            //    }
            //};

            cartesianChart2.AxisX.Add(new Axis
            {
                //Title = "Time"
            });

            cartesianChart2.AxisY.Add(new Axis
            {
                //Title = "Voltage"
            });

            SetAxisLimitsPayload();

            cartesianChart2.DisableAnimations = true;
            //cartesianChart2.LegendLocation = LegendLocation.Right;

            //modifying the series collection will animate and update the chart
            //cartesianChart2.Series.Add(new LineSeries
            //{
            //    Values = new ChartValues<double> { 5, 3, 2, 4, 5 },
            //    LineSmoothness = 0, //straight lines, 1 really smooth lines
            //    PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 50,
            //    PointForeground = System.Windows.Media.Brushes.Gray
            //});

            //modifying any series values will also animate and update the chart
            //cartesianChart2.Series[2].Values.Add(5d);
        }

        public void updatePayloadChart(List<double> myData)
        {
            if (myData.Count > 0)
            {
                //cartesianChart2.AxisX[0].MaxValue = myData.Count * sock.VPacket.SampleRate;
                //cartesianChart2.AxisX[0].MinValue = sock.VPacket.SampleRate;

                if (cartesianChart2.Series.Count > 0)
                {
                    cartesianChart2.Series.Clear();
                }

                GearedValues<double> temp = new GearedValues<double>();
                temp.WithQuality(Quality.Low);
                //ChartValues<double> temp = new ChartValues<double>();

                foreach (double d in myData)
                {
                    temp.Add(d);
                }

                cartesianChart2.Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Data",
                        Values = temp,
                        LineSmoothness = 1, //straight lines, 1 really smooth lines
                        PointGeometrySize = 5
                    }
                };
            }
        }

        private void SetAxisLimitsPayload()
        {
            cartesianChart2.AxisY[0].MaxValue = 1;
            cartesianChart2.AxisY[0].MinValue = 0;
        }

        private void InvokeUI(Action a)
        {
            try
            {
                this.BeginInvoke(new MethodInvoker(a));
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        private void timerStart()
        {
            timer.Elapsed += new System.Timers.ElapsedEventHandler(function);
            timer.Interval = refreshRateSlider.Value;
            timer.Enabled = true;
            timer.Start();
            refreshRate.Text = "Refresh Rate: " + refreshRateSlider.Value + " ms";
        }

        private void function(object sender, System.Timers.ElapsedEventArgs e)
        {
            InvokeUI(() =>
            {
                if (sock.VPacket.PayloadType.Contains(true)) // Check that Payload Type has been set
                {
                    if (sock.VPacket.PayloadType[0] == true || sock.VPacket.PayloadType[1] == true) // Check that the Data Payload is set to Reals or Complex, Cartesian
                        updatePayloadChart(sock.NormalizedPayload);
                    else
                        timer.Stop();
                }
            });
        }

        private void specDisplayEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (specDisplayEnable.Checked == true)
            {
                //updateChart.Visible = true;
                cartesianChart2.Visible = true;
                //updateChart.Enabled = true;
                cartesianChart2.Enabled = true;
                refreshRate.Visible = true;
                refreshRateSlider.Visible = true;
                dataFormat.Visible = true;

                timerStart();

                Height += 250;
            }
            else
            {
                //updateChart.Visible = false;
                cartesianChart2.Visible = false;
                //updateChart.Enabled = false;
                cartesianChart2.Enabled = false;
                refreshRate.Visible = false;
                refreshRateSlider.Visible = false;
                dataFormat.Visible = false;

                timer.Stop();

                Height -= 250;
            }
        }

        private void refreshRateSlider_Scroll(object sender, EventArgs e)
        {
            timer.Interval = refreshRateSlider.Value;
            refreshRate.Text = "Refresh Rate: " + refreshRateSlider.Value + " ms";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sock.CloseConnection();
        }

        private void fromFile_CheckedChanged(object sender, EventArgs e)
        {
            if (fromFile.Checked == true)
            {
                pt = new Point(verboseCheck.Location.X, verboseCheck.Location.Y);
                verboseCheck.Location = new Point(startButton.Location.X + startButton.Width + 25, startButton.Location.Y);
                fileLoadChoose.Visible = true;
                loadLoc.Visible = true;

                //cartesianChart1.Visible = false;
                //cartesianChart1.Enabled = false;
                stopButton.Visible = false;
                //clearButton.Visible = false;
                comboBox1.Visible = false;
                label3.Visible = false;
                label2.Visible = false;
                textBox1.Visible = false;
                //specDisplayEnable.Visible = false;
            }
            else
            {
                verboseCheck.Location = pt;
                fileLoadChoose.Visible = false;
                loadLoc.Visible = false;

                //cartesianChart1.Visible = true;
                //cartesianChart1.Enabled = true;
                stopButton.Visible = true;
                //clearButton.Visible = true;
                comboBox1.Visible = true;
                label3.Visible = true;
                label2.Visible = true;
                textBox1.Visible = true;
                //specDisplayEnable.Visible = true;
            }
        }

        private void fileLoadChoose_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                fileLoadLocation = openFileDialog1.FileName;
                loadLoc.Text = openFileDialog1.FileName;
            }
        }

        private void discoOpt_CheckedChanged(object sender, EventArgs e)
        {
            if (discoOpt.Checked == true)
            {
                startDisco();
            }
            else if (discoOpt.Checked == false)
            {
                timer2.Stop();
                this.Style = MetroColorStyle.Blue;
                this.dataGridView1.Style = MetroColorStyle.Blue;
                this.Update();
                this.Refresh();
            }
        }

        private void startDisco()
        {
            timer2.Elapsed += new System.Timers.ElapsedEventHandler(discoFunc);
            timer2.Interval = 200;
            timer2.Enabled = true;
            timer2.Start();
        }
        bool opc = false;
        private void discoFunc(object sender, System.Timers.ElapsedEventArgs e)
        {
            InvokeUI(() =>
            {
                this.Style = (MetroColorStyle)rand.Next(0, 15);
                this.dataGridView1.Style = (MetroColorStyle)rand.Next(0, 15);
                this.Update();
                this.Refresh();
            });
        }
        int count = 0;
        private void label2_Click(object sender, EventArgs e)
        {
            count++;
            if (count == 10)
            {
                discoOpt.Visible = true;
            }
        }
    }
}
