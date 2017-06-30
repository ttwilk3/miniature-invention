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
using System.Data;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace HOUNDDOG_GUI
{
    public partial class Form1 : Form
    {
        sockets sock;
        bool running = false;
        bool limitSet = false;
        bool reset = false;
        int packetLimit;
        Random rand = new Random();
        int prevPackCount = 0;

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
            verboseCheck.Checked = true;
            updateComboBox();
            BindGrid();

            setupPacketCountChart();
            setupDataPayloadChart();
        }

        public bool getVerboseValue()
        {
            return verboseCheck.Checked;
        }

        public void updatePacketNum(string s, long num)
        {
            label1.Text = s;
            if (limitSet == true && num >= packetLimit)
            {
                dataGridView1.Refresh();
                running = false;
                reset = true;
                progressBar1.Value = 100;
                sock.CloseConnection();
            }
        }

        public void updateProgress()
        {
            if (progressBar1.Value >= 100)
            {
                //Thread.Sleep(500);
                progressBar1.Value = 0;
            }

            if (rand.Next(0, 101) < 30)
                progressBar1.Value += 1;

        }

        private void updateComboBox()
        {
            foreach (string s in sock.adapt)
                comboBox1.Items.Add(s);
            comboBox1.SelectedIndex = 1;
        }

        private void BindGrid()
        {
            DataTable temp = sock.getTable();
            BindingSource sBind = new BindingSource();
            sBind.DataSource = temp;
            dataGridView1.DataSource = sBind;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool badInp = false;
            packetLimit = 0;
            if (textBox1.Text.Length > 0)
            {
                bool result = Int32.TryParse(textBox1.Text, out packetLimit);
                if (result != true)
                {
                    MessageBox.Show("Please enter a valid integer. Up to 1M packets.");
                    badInp = true;
                }
                else
                {
                    if (packetLimit > 1000000)
                    {
                        MessageBox.Show("Please enter a valid integer. Up to 1M packets.");
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
                running = true;
                sock.DeviceInd = comboBox1.SelectedIndex;
                sock.Start();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            BindGrid();
            dataGridView1.Refresh();
            running = false;
            sock.CloseConnection();
            progressBar1.Value = 100;
        }

        private void refreshButton_Click(object sender, EventArgs e)
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
            }
        }

        public ChartValues<MeasureModel> ChartValues { get; set; }
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
            ChartValues = new ChartValues<MeasureModel>();
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = ChartValues,
                    PointGeometrySize = 8,
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
            cartesianChart2.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> {4, 6, 5, 2, 7}
                }
            };

            cartesianChart2.AxisX.Add(new Axis
            {
                Title = "Time"
            });

            cartesianChart2.AxisY.Add(new Axis
            {
                Title = "Values"
            });

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
    }
}
