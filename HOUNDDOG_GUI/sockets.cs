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

    public class sockets
    {
        dotnetWinpCap wpcap = null;
        ArrayList devlist = null;
        private dotnetWinpCap.ReceivePacket rcvPack = null;
        int pack_count = 0;
        private System.ComponentModel.Container components = null;
        List<string> cbAdapters = new List<string>(); // List of available capture points on device
        int SelectedIndex = 0;
        Vita49 pack = new Vita49(); // Instance of the parser
        DataTable table = new DataTable(); // Populated with parsed packet data for display in GUI
        string fileL = System.IO.Directory.GetCurrentDirectory() + @"\data.txt"; // Save Location
        List<double> dataPayNormalized = new List<double>(); // Normalized data payload data, currently just Reals
        DateTime dt = new DateTime();
        string badPacks = @"\invalidPackets.txt";
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

        public string badPackFileLoc
        {
            get { return badPacks; }
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
            dt = DateTime.Now;
            frm = frm_;
            table.Columns.Add("Packet #", typeof(int));
            //table.Columns.Add("CapLength", typeof(int));
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
            setBadPack();
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
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(fileL, true)) // For Valid Packets
                {
                    if (frm.getFileorLive() == true)
                    {
                        file.WriteLine("*****THESE PACKETS ARE PARSED FROM: " + frm.getFileLoadLoc() + "*****\n");
                    }
                    else
                    {
                        file.WriteLine("*****THESE PACKETS ARE PARSED FROM A LIVE CAPTURE*****\n");
                    }
                }
                //string badPacks = System.IO.Directory.GetCurrentDirectory() + @"\invalidPackets.txt";
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(badPacks, true)) // For Valid Packets
                {
                    if (frm.getFileorLive() == true)
                    {
                        file.WriteLine("*****THESE PACKETS ARE PARSED FROM: " + frm.getFileLoadLoc() + "*****\n");
                    }
                    else
                    {
                        file.WriteLine("*****THESE PACKETS ARE PARSED FROM A LIVE CAPTURE*****\n");
                    }
                }
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
                

                rtb.Append("Content of packet : \n");
                rtb.Append("    Packet #: " + table.Rows.Count + "\n");
                //rtb.Append("	Caplength: " + p.Caplength + "\n");
                rtb.Append("	Length   : " + p.Length + "\n");
                rtb.Append("	Timestamp: " + p.TimeStamp.ToLongDateString() + " " + p.TimeStamp.ToLongTimeString() + "\n");
                rtb.Append("    Source: " + src + "\n");
                rtb.Append("    Destination: " + dest + "\n");
                rtb.Append("    Source Port: " + srcPort + "\n");
                rtb.Append("    Destination Port: " + destPort + "\n");
                rtb.Append("    VRT Header: 0x" + binary + "\n");
                
                //rtb.Append("    Class ID: " + classID + "\n"); // Mandatory in Vita-49

                binary.Replace(" ", string.Empty);
                string report = "";
                if (s.Length > 50)
                {
                    report = pack.parseHeader(binary); // Parse out the VRT Header from the packet
                }

                int payloadInd = pack.StreamID == true ? 50 : 46; // Start after Stream ID if true, if it isn't present start at 46

                string streamID = "";
                if (pack.StreamID == true &&  s.Length > 50)
                {
                    streamID = formatStreamID(s);
                    rtb.Append("    Stream ID: " + streamID + "\n"); // Mandatory in Vita-49
                }

                string report2 = string.Empty;
                if (pack.Trailer == true)
                {
                    string binary2 = formatTrailer(s);
                    report2 = pack.parseTrailer(binary2); // Parse VRT trailer in data packets, Mandatory for V49A
                    rtb.Append("    VRT Trailer: 0x" + binary2 + "\n");
                }

                string classID = "";
                string report4 = "";
                if (pack.classPres == true && s.Length > 57)
                {
                    byte[] classIDByte = s.SubArray(payloadInd, 8);
                    classID = formatBytestoBin(classIDByte);
                    report4 = pack.processClassID(classID);
                }

                rtb.Append("    Packet Type: " + (pack.PackType ? "Data" : "Context") + "\n");

                byte[] contextData;
                string contextStr = "";
                string report3 = string.Empty;
                if (pack.PackType == false) // If the packet is a context packet, parse out the context data payload
                {
                    int ind = 50; // Start Right after Stream ID
                    ind += pack.classPres ? 8 : 0; // Class ID
                    ind += pack.IntegerTimestamp ? 4 : 0; // Integer Timestamp
                    ind += pack.FractionalTimestamp ? 8 : 0; // Fractional Timestamp

                    int length = s.Length - ind;
                    if (length > 0)
                    {
                        contextData = s.SubArray(ind, length);
                        contextStr = formatContextData(contextData, true);
                        string conBin = formatContextData(contextData, false);
                        //System.Diagnostics.Debug.WriteLine("Entire Context Data " + contextData.Length);
                        string restOfCon = formatBytestoBin(contextData.SubArray(4, contextData.Length - 4));
                        report3 = pack.processContextData(conBin, restOfCon);
                    }
                }

                byte[] dataPayload = new byte[1];
                List<double> myData = new List<double>();
                // If it is a data packet, and the spectal display is enabled
                // Temporarily just checking that the Payload Type has been set to Real, this only processes Real Data Payloads currently
                // TODO -- Add IQ Complex Data Payload parsing and graphing
                if (pack.PayloadType.Contains(true))
                {
                    if (pack.PayloadType[0])
                        frm.updateDataFormat("Real");
                    else if (pack.PayloadType[1])
                        frm.updateDataFormat("Complex, Cartesian");
                    else if (pack.PayloadType[2])
                        frm.updateDataFormat("Complex, Polar");
                }
                if (frm.getSpectralDisplayEnableValue() == true && pack.PackType == true && pack.PayloadType.Contains(true))
                {
                    payloadInd += pack.classPres ? 8 : 0; // Class ID
                    payloadInd += pack.IntegerTimestamp ? 4 : 0; // Integer Timestamp
                    payloadInd += pack.FractionalTimestamp ? 8 : 0; // Fractional Timestamp

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

                    if (pack.PayloadType[0] == true && dataPayload.Length > 1) // For Reals
                    {
                        myData = conversionToReals(dataPayload); // Literal Real Values

                        double min = 1.0 * myData.Min();
                        double max = 1.0 * myData.Max();
                        //dataPayNormalized = myData.Select(x => (x - min) / (max - min)).ToList<double>(); // Real Values Normalized for plotting
                        //dataPayNormalized = myData.Select(x => Math.Log(x / max)).ToList<double>();
                        dataPayNormalized = myData.Select(x => Math.Exp((x - max)/(max - min))).ToList<double>();
                    }
                    else if (pack.PayloadType[1] == true && dataPayload.Length > 1) // For Complex, Cartesian
                    {
                        myData = complexToReals(dataPayload);

                        double min = 1.0 * myData.Min();
                        double max = 1.0 * myData.Max();
                        //dataPayNormalized = myData.Select(x => (x - min) / (max - min)).ToList<double>(); // Real Values Normalized for plotting
                        //dataPayNormalized = myData.Select(x => Math.Log(x / max)).ToList<double>();
                        dataPayNormalized = myData.Select(x => Math.Exp((x - max) / (max - min))).ToList<double>();
                    }
                    
                }

                //packets.Add(new Packet(p.Caplength, p.Length, p.TimeStamp, src, dest));
                if (streamID.Length == 0 || binary.Length == 0) // Not valid V49A if it doesn't contain a StreamID or VRT Header
                    pack.valVita = false;

                if (pack.VRTLen > p.Caplength)
                    pack.valVita = false;

                // Adding all of the parsed and gathered data to the table for display
                table.Rows.Add(table.Rows.Count, p.Length, p.TimeStamp, src, dest, pack.PackType ? "Data" : "Context", srcPort, destPort, streamID, pack.valVita);
                //Console.WriteLine(table.Rows.Count);

                // Where all the generated reports from the V49 parser class are written to
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(fileL, true)) // For Valid Packets
                {
                    file.WriteLine("-----------------------------------------");
                    if (pack.valVita == false)
                        file.WriteLine("Not Vita-49 A compliant.\n");

                    file.WriteLine(rtb.ToString());

                    if (frm.getVerboseValue() == true)
                        file.WriteLine(report); // VRT Header

                    if (frm.getVerboseValue() == true && pack.classPres == true)
                        file.WriteLine(report4); // Class ID

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
                    //string badPacks = System.IO.Directory.GetCurrentDirectory() + @"\invalidPackets.txt";
                    using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(badPacks, true))
                    {
                        file.WriteLine("-----------------------------------------");
                        if (pack.valVita == false)
                            file.WriteLine("Not Vita-49 A compliant.\n");

                        file.WriteLine(rtb.ToString());

                        if (frm.getVerboseValue() == true)
                            file.WriteLine(report); // VRT Header

                        if (frm.getVerboseValue() == true && pack.classPres == true)
                            file.WriteLine(report4); // Class ID

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
                //System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void setBadPack()
        {
            badPacks = @"\invalidPackets.txt";
            int j = 0;
            string tempLoc = fileL;
            for (j = tempLoc.Length - 1; j > 0; j--)
            {
                if (tempLoc[j].Equals('\\'))
                {
                    break;
                }
            }
            tempLoc = tempLoc.Substring(0, j);
            badPacks = tempLoc + badPacks;
        }

        // TODO -- Refactor All of the Functions below this comment
        public List<double> complexToReals(byte[] data)
        {
            List<double> realData = new List<double>();
            double val1 = 0;
            double val2 = 0;
            string b1 = "";
            string b2 = "";
            string b3 = "";
            string b4 = "";
            string temp = "";

            for (int i = 0; i < data.Length; i += 4)
            {
                if (i >= data.Length - 3)
                {
                    break;
                }

                b1 = Convert.ToString(data[i], 2);
                b2 = Convert.ToString(data[i + 1], 2);
                b3 = Convert.ToString(data[i + 2], 2);
                b4 = Convert.ToString(data[i + 3], 2);

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

                temp = "";
                for (int j = 8 - b3.Length; j > 0; j--)
                {
                    temp += "0";
                }
                temp += b3;
                b3 = temp;

                temp = "";
                for (int j = 8 - b4.Length; j > 0; j--)
                {
                    temp += "0";
                }
                temp += b4;
                b4 = temp;

                b1 += b2;
                b3 += b4;
                b1 = b1.Replace(" ", string.Empty);
                b3 = b3.Replace(" ", string.Empty);

                if (b1[0].Equals('1') == true)
                {
                    string myNum = pack.twosComplement(b1);
                    long num = Convert.ToInt64(myNum, 2);
                    num *= -1;
                    val1 = num;
                }
                else
                {
                    val1 = Convert.ToInt32(b1, 2);
                }

                if (b3[0].Equals('1') == true)
                {
                    string myNum = pack.twosComplement(b3);
                    long num = Convert.ToInt64(myNum, 2);
                    num *= -1;
                    val2 = num;
                }
                else
                {
                    val2 = Convert.ToInt64(b3, 2);
                }

                double tempVal = Math.Sqrt((val1 * val1) + (val2 * val2));
                realData.Add(tempVal);
            }

            return realData;
        }

        public List<double> conversionToReals(byte[] data)
        {
            List<double> realData = new List<double>();
            int val = 0;
            string b1 = "";
            string b2 = "";
            string temp = "";

            for (int i = 0; i < data.Length; i += 2)
            {
                if (i >= data.Length - 1)
                {
                    break;
                }

                b1 = Convert.ToString(data[i], 2);
                b2 = Convert.ToString(data[i+1], 2);
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
                //double valSquared = val * val;
                realData.Add(val);
            }

            return realData;
        }

        public string formatClassID(byte[] bin)
        {
            throw new NotImplementedException("TODO -- Implement a Byte to Bin Class ID Func");
            return "";
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
                    if ((i + 1) % 8 == 0)
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

            string check = b1 + b2 + b3 + b4;
            long valStream = Convert.ToInt64(check, 16);
            if (valStream >= 1 && valStream <= int.MaxValue)
            {
                pack.StreamID = true;
            }
            else
                pack.StreamID = false;

            return "0x" + check + "h";
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