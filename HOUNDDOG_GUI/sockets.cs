using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using System.Linq.Expressions;
using System.Reflection;

namespace HOUNDDOG_GUI
{
    static class ArrExt
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }

    class sockets
    {
        dotnetWinpCap wpcap = null;
        ArrayList devlist = null;
        private dotnetWinpCap.ReceivePacket rcvPack = null;
        int pack_count = 0;
        private System.ComponentModel.Container components = null;
        List<string> cbAdapters = new List<string>();
        int SelectedIndex = 1;
        string labPacketCnt = "";
        Vita49 pack = new Vita49();
        List<Packet> packets = new List<Packet>();
        DataTable table = new DataTable();
        string fileL = System.IO.Directory.GetCurrentDirectory() + @"\data.txt";
        //string fileL = @"C:\Users\truearrow\Documents\Visual Studio 2017\Projects\HOUNDDOG\HOUNDDOG\bin\Debug\data.txt";


        public class Packet
        {
            int CapLength { get; set; }
            int Length { get; set; }
            DateTime TimeStamp { get; set; }
            string Source { get; set; }
            string Destination { get; set; }

            public Packet(int cap, int len, DateTime time, string src, string dest)
            {
                CapLength = cap;
                Length = len;
                TimeStamp = time;
                Source = src;
                Destination = dest;
            }
        }

        public int DeviceInd
        {
            set { SelectedIndex = value; }
        }

        public int packCount
        {
            get { return pack_count; }
        }
        
        public List<string> adapt
        {
            get { return cbAdapters; }
        }

        public string FileLoc
        {
            get { return fileL; }
            set { fileL = value; }
        }

        public DataTable getTable()
        {
            return table;
        }

        public void clearTable()
        {
            pack_count = 0;
            table.Clear();
        }

        Form1 frm = null;
        public sockets(Form1 frm_)
        {
            frm = frm_;
            table.Columns.Add("Packet #", typeof(int));
            table.Columns.Add("CapLength", typeof(int));
            table.Columns.Add("Length", typeof(int));
            table.Columns.Add("TimeStamp", typeof(DateTime));
            table.Columns.Add("Source", typeof(string));
            table.Columns.Add("Destination", typeof(string));
            table.Columns.Add("Packet Type", typeof(string));
            table.Columns.Add("Source Port", typeof(int));
            table.Columns.Add("Destination Port", typeof(int));
            table.Columns.Add("Stream ID", typeof(string));
            table.Columns.Add("Valid Vita-49a", typeof(bool));
            if (wpcap == null)
            {
                devlist = dotnetWinpCap.FindAllDevs();
                for (int i = 0; i <= devlist.Count - 1; i++)
                {
                    cbAdapters.Add(((Device)devlist[i]).Name + ":" + ((Device)devlist[i]).Description);
                }
            }
        }

        public List<Packet> myPackets
        {
            get { return packets; }
        }

        public void CloseConnection()
        {
            if (wpcap != null)
            {
                //wpcap.StopDump();
                wpcap.Close();
            }
        }

        public void ListAdapters()
        {
            //Console.Clear();
            Console.WriteLine("Adapters: ");
            for (int i = 0; i < cbAdapters.Count; i++)
                Console.WriteLine(i + ": " + cbAdapters[i]);
            Console.WriteLine("Make your choice: ");
            SelectedIndex = Convert.ToInt32(Console.ReadLine());
        }

        public void Start()
        {
            if (wpcap == null)
            {
                wpcap = new dotnetWinpCap();
            }
            if (wpcap.IsOpen)
            {
                wpcap.Close();
            }
            if (!wpcap.Open(((Device)devlist[SelectedIndex]).Name, 65536, 1, 0)) //cbAdapters.SelectedIndex
            {
                System.Diagnostics.Debug.WriteLine(wpcap.LastError);
                //rtb.AppendText(wpcap.LastError + "\n");
                Console.WriteLine(wpcap.LastError + "\n");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("opened successfully: " + ((Device)devlist[0]).Name);
                //rtb.AppendText("Opened successfully: " + ((Device)devlist[cbAdapters.SelectedIndex]).Name + "\n");
                Console.WriteLine("Opened successfully: " + ((Device)devlist[SelectedIndex]).Name + "\n"); //cbAdapters.SelectedIndex
            }


            wpcap.SetMinToCopy(100);

            if (rcvPack == null)
            {
                rcvPack = new dotnetWinpCap.ReceivePacket(this.ReceivePacket);
                wpcap.OnReceivePacket += rcvPack;
            }


            wpcap.StartListen();
            //wpcap.StartDump("data.txt");
            //rtb.AppendText("Capturing. . .\n");
            Console.WriteLine("Capturing. . .\n");
        }

        delegate void ReceivePacketDel(object sender, PacketHeader p, byte[] s);
        event ReceivePacketDel OnReceivePacket;

        public void ReceivePacket(object sender, PacketHeader p, byte[] s)
        {
            try
            {
                this.pack_count++;
                StringBuilder rtb = new StringBuilder();
                string binary = formatHeader(s);
                // 26-29 Source
                string src = s[26] + "." + s[27] + "." + s[28] + "." + s[29];
                // 30-33 Destination
                string dest = s[30] + "." + s[31] + "." + s[32] + "." + s[33];
                // 34-35 Source Port
                string srcPort = Convert.ToString(s[34], 2) + Convert.ToString(s[35], 2);
                srcPort = Convert.ToInt32(srcPort, 2).ToString();
                // 36-37 Destination Port
                string destPort = Convert.ToString(s[35], 2) + Convert.ToString(s[36], 2);
                destPort = Convert.ToInt32(destPort, 2).ToString();
                //rtb.Focus();
                string streamID = formatStreamID(s);

                rtb.Append("Content of packet : \n");
                rtb.Append("    Packet #: " + table.Rows.Count + "\n");
                rtb.Append("	Caplength: " + p.Caplength + "\n");
                rtb.Append("	Length   : " + p.Length + "\n");
                rtb.Append("	Timestamp: " + p.TimeStamp.ToLongDateString() + " " + p.TimeStamp.ToLongTimeString() + "\n");
                rtb.Append("    Source: " + src + "\n");
                rtb.Append("    Destination: " + dest + "\n");
                rtb.Append("    Source Port: " + srcPort + "\n");
                rtb.Append("    Destination Port: " + destPort + "\n");
                rtb.Append("    VRT Header: 0x" + binary + "\n");
                rtb.Append("    Stream ID: " + streamID + "\n"); // Mandatory in Vita-49

                binary.Replace(" ", string.Empty);
                string report = pack.parseHeader(binary);

                string report2 = string.Empty;
                if (pack.Trailer == true)
                {
                    string binary2 = formatTrailer(s);
                    report2 = pack.parseTrailer(binary2);
                    rtb.Append("    VRT Trailer: 0x" + binary2 + "\n");
                }

                rtb.Append("    Packet Type: " + (pack.PackType ? "Data" : "Context") + "\n");

                byte[] contextData;
                string contextStr = "";
                string report3 = string.Empty;
                if (pack.PackType == false)
                {
                    contextData = s.SubArray(62, s.Length - 63);
                    contextStr = formatContextData(contextData, true);
                    string conBin = formatContextData(contextData, false);
                    report3 = pack.processContextData(conBin, formatBytestoBin(contextData.SubArray(4, contextData.Length - 5)));

                }

                //packets.Add(new Packet(p.Caplength, p.Length, p.TimeStamp, src, dest));
                table.Rows.Add(table.Rows.Count, p.Caplength, p.Length, p.TimeStamp, src, dest, pack.PackType ? "Data" : "Context", srcPort, destPort, streamID, pack.valVita);
                //Console.WriteLine(table.Rows.Count);

                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(fileL, true))
                {
                    file.WriteLine("-----------------------------------------");
                    file.WriteLine(rtb.ToString());

                    if (frm.getVerboseValue() == true)
                        file.WriteLine(report);

                    if (pack.Trailer == true)
                    {
                        if (frm.getVerboseValue() == true)
                            file.WriteLine(report2);
                        pack.Trailer = false;
                    }

                    if (pack.PackType == false)
                    {
                        if (frm.getVerboseValue() == true)
                        {
                            file.WriteLine("Context Packet Data:\n0x" + contextStr + "h\n");
                            file.WriteLine(report3);
                        }
                    }
                    file.WriteLine("-----------------------------------------");
                }
                if (pack.valVita == false)
                {
                    string badPacks = System.IO.Directory.GetCurrentDirectory() + @"\invalidPackets.txt";
                    using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(badPacks, true))
                    {
                        file.WriteLine("-----------------------------------------");
                        file.WriteLine(rtb.ToString());

                        if (frm.getVerboseValue() == true)
                            file.WriteLine(report);

                        if (pack.Trailer == true)
                        {
                            if (frm.getVerboseValue() == true)
                                file.WriteLine(report2);
                            pack.Trailer = false;
                        }

                        if (pack.PackType == false)
                        {
                            if (frm.getVerboseValue() == true)
                            {
                                file.WriteLine("Context Packet Data:\n0x" + contextStr + "h\n");
                                file.WriteLine(report3);
                            }
                        }

                        file.WriteLine("-----------------------------------------");
                    }
                }
                //Console.WriteLine(rtb.ToString());
                //Console.Clear();
                //Console.WriteLine("# of Packets: " + labPacketCnt);
                frm.updatePacketNum("# of Packets: " + pack_count, pack_count);
                frm.updateProgress();
            }
            catch
            {

            }
            //labPacketCnt = Convert.ToString(this.pack_count);
        }

        public string formatBytestoBin(byte[] bin)
        {
            string val = "";
            string b1 = "";
            string temp = "";
            for (int j = 0; j < bin.Length; j++)
            {
                b1 = Convert.ToString(bin[j], 2);
                temp = "";
                for (int i = 8 - b1.Length; i > 0; i--)
                {
                    temp += "0";
                }
                temp += b1;
                b1 = temp;
                val += b1;
            }
            return val;
        }

        public string formatContextData(byte[] con, bool byteData)
        {
            if (byteData == true)
            {
                string val = "";
                string temp = "";
                for (int i = 0; i < con.Length; i++)
                {
                    temp = con[i].ToString("X");
                    temp = temp.Length == 1 ? "0" + temp : temp;
                    val += temp + " ";
                    if ((i + 1) % 4 == 0)
                        val += "\n";
                    temp = "";
                }
                return val;
            }
            else
            {
                string b1 = Convert.ToString(con[0], 2);
                string b2 = Convert.ToString(con[1], 2);
                string b3 = Convert.ToString(con[2], 2);
                string b4 = Convert.ToString(con[3], 2);
                string temp = "";
                for (int i = 8 - b1.Length; i > 0; i--)
                {
                    temp += "0";
                }
                temp += b1;
                b1 = temp;
                temp = "";
                for (int i = 8 - b2.Length; i > 0; i--)
                {
                    temp += "0";
                }
                temp += b2;
                b2 = temp;
                temp = "";
                for (int i = 8 - b3.Length; i > 0; i--)
                {
                    temp += "0";
                }
                temp += b3;
                b3 = temp;
                temp = "";
                for (int i = 8 - b4.Length; i > 0; i--)
                {
                    temp += "0";
                }
                temp += b4;
                b4 = temp;
                return b1 + " " + b2 + " " + b3 + " " + b4;
            }
        }

        public string formatStreamID(byte[] s)
        {
            string b1 = s[46].ToString("X");
            string b2 = s[46].ToString("X");
            string b3 = s[46].ToString("X");
            string b4 = s[46].ToString("X");

            b1 = b1.Length == 1 ? "0" + b1 : b1;
            b2 = b2.Length == 1 ? "0" + b2 : b2;
            b3 = b3.Length == 1 ? "0" + b3 : b3;
            b4 = b4.Length == 1 ? "0" + b4 : b4;

            return "0x" + b1 + b2 + b3 + b4 + "h";
        }

        public string formatHeader(byte[] s)
        {
            string b1 = Convert.ToString(s[42], 2);
            string b2 = Convert.ToString(s[43], 2);
            string b3 = Convert.ToString(s[44], 2);
            string b4 = Convert.ToString(s[45], 2);
            string temp = "";
            for (int i = 8 - b1.Length; i > 0; i--)
            {
                temp += "0";
            }
            temp += b1;
            b1 = temp;
            temp = "";
            for (int i = 8 - b2.Length; i > 0; i--)
            {
                temp += "0";
            }
            temp += b2;
            b2 = temp;
            temp = "";
            for (int i = 8 - b3.Length; i > 0; i--)
            {
                temp += "0";
            }
            temp += b3;
            b3 = temp;
            temp = "";
            for (int i = 8 - b4.Length; i > 0; i--)
            {
                temp += "0";
            }
            temp += b4;
            b4 = temp;
            return b1 + " " + b2 + " " + b3 + " " + b4;
        }

        public string formatTrailer(byte[] s)
        {
            string b1 = Convert.ToString(s[s.Length - 4], 2);
            string b2 = Convert.ToString(s[s.Length - 3], 2);
            string b3 = Convert.ToString(s[s.Length - 2], 2);
            string b4 = Convert.ToString(s[s.Length - 1], 2);
            string temp = "";
            for (int i = 8 - b1.Length; i > 0; i--)
            {
                temp += "0";
            }
            temp += b1;
            b1 = temp;
            temp = "";
            for (int i = 8 - b2.Length; i > 0; i--)
            {
                temp += "0";
            }
            temp += b2;
            b2 = temp;
            temp = "";
            for (int i = 8 - b3.Length; i > 0; i--)
            {
                temp += "0";
            }
            temp += b3;
            b3 = temp;
            temp = "";
            for (int i = 8 - b4.Length; i > 0; i--)
            {
                temp += "0";
            }
            temp += b4;
            b4 = temp;
            return b1 + " " + b2 + " " + b3 + " " + b4;
        }
    }
}