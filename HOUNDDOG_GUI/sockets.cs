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
        List<string> cbAdapters = new List<string>(); // List of available capture points on device
        int SelectedIndex = 1;
        Vita49 pack = new Vita49(); // Instance of the parser
        DataTable table = new DataTable(); // Populated with parsed packet data for display in GUI
        string fileL = System.IO.Directory.GetCurrentDirectory() + @"\data.txt"; // Save Location
        List<double> dataPayNormalized = new List<double>(); // Normalized data payload data, currently just Reals
        bool[] dataPayloadType = new bool[3]; // 0 - Real 1 - Complex, Cartesian 2 - Complex, Polar
        //string fileL = @"C:\Users\truearrow\Documents\Visual Studio 2017\Projects\HOUNDDOG\HOUNDDOG\bin\Debug\data.txt";

        //List<Packet> packets = new List<Packet>(); // Other option instead of using the table
        //public class Packet
        //{
        //    int CapLength { get; set; }
        //    int Length { get; set; }
        //    DateTime TimeStamp { get; set; }
        //    string Source { get; set; }
        //    string Destination { get; set; }

        //    public Packet(int cap, int len, DateTime time, string src, string dest)
        //    {
        //        CapLength = cap;
        //        Length = len;
        //        TimeStamp = time;
        //        Source = src;
        //        Destination = dest;
        //    }
        //}

        public List<double> NormalizedPayload
        {
            get { return dataPayNormalized; }
            set { dataPayNormalized = value; }
        }

        public Vita49 VPacket
        {
            get { return pack; }
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
                    cbAdapters.Add(((Device)devlist[i]).Name + ":" + ((Device)devlist[i]).Description); // Get all capture points on device
                }
            }
        }

        //public List<Packet> myPackets
        //{
        //    get { return packets; }
        //}

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
            if (!wpcap.Open(((Device)devlist[SelectedIndex]).Name, 65536, 1, 0)) // Open a socket on the selected capture point on this port
            {
                System.Diagnostics.Debug.WriteLine(wpcap.LastError);
                Console.WriteLine(wpcap.LastError + "\n");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("opened successfully: " + ((Device)devlist[0]).Name);
                Console.WriteLine("Opened successfully: " + ((Device)devlist[SelectedIndex]).Name + "\n");
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

        public void ReceivePacket(object sender, PacketHeader p, byte[] s) // Main processing function 
        {
            try
            {
                this.pack_count++;
                StringBuilder rtb = new StringBuilder();
                string binary = "";
                if (s.Length > 50)
                {
                    binary = formatHeader(s);
                }
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
                //rtb.Focus
                string streamID = "";
                if (s.Length > 50)
                {
                    streamID = formatStreamID(s);
                }

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
                //rtb.Append("    Class ID: " + classID + "\n"); // Mandatory in Vita-49

                binary.Replace(" ", string.Empty);
                string report = "";
                if (s.Length > 50)
                {
                    report = pack.parseHeader(binary); // Parse out the VRT Header from the packet
                }

                string report2 = string.Empty;
                if (pack.Trailer == true)
                {
                    string binary2 = formatTrailer(s);
                    report2 = pack.parseTrailer(binary2); // Parse VRT trailer in data packets, Mandatory for V49A
                    rtb.Append("    VRT Trailer: 0x" + binary2 + "\n");
                }

                rtb.Append("    Packet Type: " + (pack.PackType ? "Data" : "Context") + "\n");

                byte[] contextData;
                string contextStr = "";
                string report3 = string.Empty;
                if (pack.PackType == false) // If the packet is a context packet, parse out the context data payload
                {
                    int length = s.Length - 62;
                    if (length > 0)
                    {
                        contextData = s.SubArray(62, length);
                        contextStr = formatContextData(contextData, true);
                        string conBin = formatContextData(contextData, false);
                        //System.Diagnostics.Debug.WriteLine("Entire Context Data " + contextData.Length);
                        string restOfCon = formatBytestoBin(contextData.SubArray(4, contextData.Length - 4));
                        report3 = pack.processContextData(conBin, restOfCon);
                    }
                }

                int payloadInd = 50; // Start after Stream ID
                byte[] dataPayload = new byte[1];
                List<int> myData = new List<int>();
                if (frm.getSpectralDisplayEnableValue() == true && pack.PackType == true) // If it is a data packet, and the spectal display is enabled
                {
                    payloadInd += pack.classPres ? 8 : 0; // Class ID
                    payloadInd += 4; // Integer Timestamp
                    payloadInd += 8; // Fractional Timestamp

                    if (pack.Trailer == true)
                    {
                        int length = s.Length - payloadInd - 4;
                        if (length > 0)
                            dataPayload = s.SubArray(payloadInd, length);
                    }
                    else
                    {
                        int length = s.Length - payloadInd;
                        if (length > 0)
                            dataPayload = s.SubArray(payloadInd, length);
                    }

                    if (dataPayload.Length > 1)
                    {
                        myData = conversionToReals(dataPayload); // Literal Real Values

                        double min = 1.0 * myData.Min();
                        double max = 1.0 * myData.Max();
                        dataPayNormalized = myData.Select(x => (x - min) / (max - min)).ToList<double>(); // Real Values Normalized for plotting
                    }
                    //frm.updatePayloadChart(newList);
                }

                //packets.Add(new Packet(p.Caplength, p.Length, p.TimeStamp, src, dest));
                if (streamID.Length == 0 || binary.Length == 0) // Not valid V49A if it doesn't contain a StreamID or VRT Header
                    pack.valVita = false;

                // Adding all of the parsed and gathered data to the table for display
                table.Rows.Add(table.Rows.Count, p.Caplength, p.Length, p.TimeStamp, src, dest, pack.PackType ? "Data" : "Context", srcPort, destPort, streamID, pack.valVita);
                //Console.WriteLine(table.Rows.Count);

                // Where all the generated reports from the V49 parser class are written to
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(fileL, true)) // For Valid Packets
                {
                    file.WriteLine("-----------------------------------------");
                    file.WriteLine(rtb.ToString());

                    if (frm.getVerboseValue() == true)
                        file.WriteLine(report); // VRT Header

                    if (pack.Trailer == true)
                    {
                        if (frm.getVerboseValue() == true)
                            file.WriteLine(report2); // VRT Trailer
                        pack.Trailer = false;
                    }

                    if (pack.PackType == false)
                    {
                        if (frm.getVerboseValue() == true)
                        {
                            file.WriteLine("Context Packet Data:\n0x" + contextStr + "h\n");
                            file.WriteLine(report3); // Context Packet Data 
                        }
                    }
                    file.WriteLine("-----------------------------------------");
                }

                if (pack.valVita == false) // For Invalid Packets
                {
                    string badPacks = System.IO.Directory.GetCurrentDirectory() + @"\invalidPackets.txt";
                    using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(badPacks, true))
                    {
                        file.WriteLine("-----------------------------------------");
                        file.WriteLine(rtb.ToString());

                        if (frm.getVerboseValue() == true)
                            file.WriteLine(report); // VRT Header

                        if (pack.Trailer == true)
                        {
                            if (frm.getVerboseValue() == true)
                                file.WriteLine(report2); // VRT Trailer
                            pack.Trailer = false;
                        }

                        if (pack.PackType == false)
                        {
                            if (frm.getVerboseValue() == true)
                            {
                                file.WriteLine("Context Packet Data:\n0x" + contextStr + "h\n");
                                file.WriteLine(report3); // Context Packet Data
                            }
                        }

                        file.WriteLine("-----------------------------------------");
                    }
                }

                frm.updatePacketNum("# of Packets: " + pack_count, pack_count);
                if (pack.PackType == true && pack.valVita == true)
                    frm.DataPack++;
                else if (pack.PackType == false && pack.valVita == true)
                    frm.ContextPack++;
                else
                    frm.OtherPacks++;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public List<int> conversionToReals(byte[] data)
        {
            List<int> realData = new List<int>();
            int val = 0;
            string b1 = "";
            string b2 = "";
            string temp = "";
            bool neg = false;

            for (int i = 0; i < data.Length; i += 2)
            {
                if (i == data.Length - 1)
                {
                    break;
                }

                b1 = Convert.ToString(data[i], 2);
                b2 = Convert.ToString(data[i], 2);
                temp = "";
                for (int j = 8 - b1.Length; j > 0; j--)
                {
                    temp += "0";
                }
                temp += b1;
                b1 = temp;

                temp = "";
                for (int j = 8 - b2.Length; j > 0; j--)
                {
                    temp += "0";
                }
                temp += b2;
                b2 = temp;
                b1 += b2;

                b1 = b1.Replace(" ", string.Empty);
                if (b1[0].Equals('1') == true)
                {
                    string myNum = pack.twosComplement(b1);
                    int num = Convert.ToInt32(myNum, 2);
                    num *= -1;
                    val = num;
                }
                else
                {
                    val = Convert.ToInt32(b1, 2);
                }
                realData.Add(val);
            }

            return realData;
        }

        public string formatBytestoBin(byte[] bin)
        {
            //System.Diagnostics.Debug.WriteLine("Bytes to Bin " + bin.Length);
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
            string b2 = s[47].ToString("X");
            string b3 = s[48].ToString("X");
            string b4 = s[49].ToString("X");

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