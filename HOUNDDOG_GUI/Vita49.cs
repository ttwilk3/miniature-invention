using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOUNDDOG_GUI
{
    public class Vita49
    {
        // All the dictionaries for lookup
        Dictionary<string, string> packetType = new Dictionary<string, string>();
        Dictionary<string, string> TSI = new Dictionary<string, string>();
        Dictionary<string, string> TSF = new Dictionary<string, string>();
        Dictionary<string, string> dataFormatCodes = new Dictionary<string, string>();
        Dictionary<string, string> dataSampleType = new Dictionary<string, string>();
        Dictionary<string, string> dataItemFormat = new Dictionary<string, string>();

        bool trail = false; // Trailer
        bool dataPack = false; // If it is a Data packet
        bool validVita = false; // Valid V49A or not

        bool streamIDPres = false; // If there is a Stream ID Present *There must be one in Vita-49A*
        bool classIDPres = false; // If there is a Class ID present
        bool intTimestamp = false; // If there is an Integer-seconds Timestamp present
        bool fracTimestamp = false; // If there is a Fractional-seconds Timestamp present

        // Booleans for Data Packet Payload Format Field in IF Context Packets
        bool dataPacketPayloadFormat = false; // IF Context Packets SHALL include a Data Payload Format Field
        bool procEfficientPack = false; // IF Data packet SHALL use Processing-Efficient Packing
        bool sampleComponentRep = false; // IF Data packet SHALL NOT use Sample-Component Repeating
        bool eventTags = false; // Item Packing Field SHALL NOT include any bits for Event Tags
        bool channelTags = false; // Item Packing Field SHALL NOT include any bits for Channel Tags4

        bool dataFormat = false; // IF Data Packet SHALL use one of the following Data Item Formats 6.1.6-12

        bool[] contextPackInd = new bool[24]; // For the Context Packet Indicator Fields
        bool[] dataPayloadType = new bool[3]; // 0 - Real 1 - Complex, Cartesian 2 - Complex, Polar
        double sampRate = 0.0;
        int packLen = 0;

        public int VRTLen
        {
            get { return packLen; }
        }

        public bool Trailer // If there is a trailer in the packet
        {
            get { return trail; }
            set { trail = value; }
        }

        public bool IntegerTimestamp // If there is an integer timestamp
        {
            get { return intTimestamp; }
        }

        public bool FractionalTimestamp // If there is a fractional timestamp
        {
            get { return fracTimestamp; }
        }

        public double SampleRate 
        {
            get { return sampRate; }
        }

        public bool PackType // If set to true then it is a data packet, if false then it is a context packet
        {
            get { return dataPack; }
            set { dataPack = value; }
        }

        public bool valVita // If the packet is valid or not
        {
            get { return validVita; }
            set { validVita = value; }
        }

        public bool StreamID // If Stream ID is present
        {
            get { return streamIDPres; }
            set { streamIDPres = value; }
        }

        public bool classPres // If class ID is present
        {
            get { return classIDPres; }
            set { classIDPres = value; }
        }

        public bool[] PayloadType // 0 - Real, 1 - Complex, Cartesian, 2 - Complex, Polar
        {
            get { return dataPayloadType; }
        }

        public Vita49()
        {
            setupDictionaries(); // Initialize
            for (int i = 0; i < dataPayloadType.Length; i++)
            {
                dataPayloadType[i] = false;
            }
            
            // For testing purposes
            //dataPayloadType[0] = true;
            //dataPayloadType[1] = true;
            //dataPayloadType[2] = true;
        }

        public string parseHeader(string bin)
        {
            // Reset all the booleans everytime you go to parse
            PackType = false;
            validVita = false;
            streamIDPres = false;
            classIDPres = false;
            intTimestamp = false;
            fracTimestamp = false;
            packLen = 0;
            StringBuilder report = new StringBuilder();
            report.Append("VRT Header: \n");
            try
            {
                bin = bin.Replace(" ", string.Empty);
                StringBuilder str = new StringBuilder(bin.Substring(0, 4));
                if (str.ToString().Equals("0100") != true && str.ToString().Equals("0101") != true) // Data Packet
                {
                    // "Signal Data Packet without Stream ID" and
                    // "Extension Data PAcket without Stream ID"
                    if (str.ToString().Equals("0000") == true || str.ToString().Equals("0010"))
                    {
                        streamIDPres = false;
                        // The Packet Type Code "0000" SHALL NOT be used
                    }
                    else
                        streamIDPres = true;

                    report.Append("    Type: 0x" + str.ToString() + " -- " + packetType[str.ToString()] + "\n");

                    report.Append((bin[4].Equals('1') ? "    Class: 0x1 -- Class ID present.\n" : "    Class: 0x0 -- Class ID not present. -- CLASS ID MUST BE PRESENT IN VITA-49A\n"));
                    classIDPres = bin[4].Equals('1') ? true : false;

                    report.Append((bin[5].Equals('1') ? "    Trailer: 0x1 -- Trailer is present.\n" : "    Trailer: 0x0 -- Trailer is not present. -- TRAILER MUST BE PRESENT IN VITA-49A.\n"));
                    trail = bin[5].Equals('1') ? true : false;

                    validVita = (streamIDPres && classIDPres && trail && (dataPayloadType[0] || dataPayloadType[1])) ? true : false;

                    str.Clear();
                    str.Append(bin.Substring(8, 2));
                    report.Append("    TSI: 0x" + str.ToString() + " -- " + TSI[str.ToString()] + "\n");
                    intTimestamp = setTimestampBools(bin.Substring(8, 2));

                    str.Clear();
                    str.Append(bin.Substring(10, 2));
                    report.Append("    TSF: 0x" + str.ToString() + " -- " + TSF[str.ToString()] + "\n");
                    fracTimestamp = setTimestampBools(bin.Substring(10, 2));

                    str.Clear();
                    str.Append(bin.Substring(12, 4));
                    report.Append("    Count: 0x" + str.ToString() + " : Decimal -- " + Convert.ToInt32(str.ToString().Replace(" ", string.Empty), 2).ToString() + "\n");

                    str.Clear();
                    str.Append(bin.Substring(16, bin.Length - 17));
                    report.Append("    Size: 0x" + str.ToString().Replace(" ", string.Empty) + " : Decimal -- " + Convert.ToInt32(str.ToString().Replace(" ", string.Empty), 2).ToString() + "\n");
                    packLen = 4 * Convert.ToInt32(str.ToString().Replace(" ", string.Empty), 2);

                    //if (validVita != true)
                    //{
                    //    report.Append("Not Vita-49 compliant.\n");
                    //}
                    PackType = true;
                    return report.ToString();
                }
                else if (str.ToString().Equals("0100") == true || str.ToString().Equals("0101") == true) // Context Packet
                {
                    streamIDPres = true;

                    report.Append("    Type: 0x" + str.ToString() + " -- " + packetType[str.ToString()] + "\n");

                    report.Append((bin[4].Equals('1') ? "    Class: 0x1 -- Class ID present.\n" : "    Class: 0x0 -- Class ID not present.\n"));
                    classIDPres = bin[4].Equals('1') ? true : false;

                    report.Append((bin[7].Equals('1') ? "    Timestamp Mode: 0x1 -- General Event Timing.\n" :"    Timestamp Mode: 0x0 -- Precise Event Timing\n"));

                    str.Clear();
                    str.Append(bin.Substring(8, 2));
                    report.Append("    TSI: 0x" + str.ToString() + " -- " + TSI[str.ToString()] + "\n");
                    intTimestamp = setTimestampBools(bin.Substring(8, 2));

                    str.Clear();
                    str.Append(bin.Substring(10, 2));
                    report.Append("    TSF: 0x" + str.ToString() + " -- " + TSF[str.ToString()] + "\n");
                    fracTimestamp = setTimestampBools(bin.Substring(10, 2));

                    str.Clear();
                    str.Append(bin.Substring(12, 4));
                    report.Append("    Count: 0x" + str.ToString() + " : Decimal -- " + Convert.ToInt32(str.ToString().Replace(" ", string.Empty), 2).ToString() + "\n");

                    str.Clear();
                    str.Append(bin.Substring(16, bin.Length - 17));
                    report.Append("    Size: 0x" + str.ToString().Replace(" ", string.Empty) + " : Decimal -- " + Convert.ToInt32(str.ToString().Replace(" ", string.Empty), 2).ToString() + "\n");
                    
                    validVita = classIDPres == true ? true : false;
                    return report.ToString();
                }
            }
            catch
            {
                validVita = false;
                return report.ToString();// + "Not Vita-49 compliant.\n";
            }
            return "";
        }

        public string parseTrailer(string bin)
        {
            bin = bin.Replace(" ", string.Empty);
            StringBuilder report = new StringBuilder();
            report.Append("VRT Trailer: \n");

            report.Append("    Calibrated Time Indicator Enable: 0x" + bin[0] + " -- " + (bin[0].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Valid Signal Indicator Enable: 0x" + bin[1] + " -- " + (bin[1].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Reference Lock Indicator Enable: 0x" + bin[2] + " -- " + (bin[2].Equals('1') ? "True" : "False") + "\n");
            report.Append("    AGC/MGC Indicator Enable: 0x" + bin[3] + " -- " + (bin[3].Equals('1') ? "True" : "False") + "\n");

            // Missing bin[4]
            report.Append("    Spectral Inversion Indicator Enable: 0x" + bin[5] + " -- " + (bin[5].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Overrange Indicator Enable: 0x" + bin[6] + " -- " + (bin[6].Equals('1') ? "True" : "False") + "\n\n");
            // Missing bin[7]

            // Missing bin[8]
            // Missing bin[9]
            // Missing bin[10]
            // Missing bin[11]

            report.Append("    Calibrated Time Indicator: 0x" + bin[12] + " -- " + (bin[12].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Valid Signal Indicator: 0x" + bin[13] + " -- " + (bin[13].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Reference Lock Indicator: 0x" + bin[14] + " -- " + (bin[14].Equals('1') ? "True" : "False") + "\n");
            report.Append("    AGC/MGC Indicator: 0x" + bin[15] + " -- " + (bin[15].Equals('1') ? "True" : "False") + "\n");

            // Missing bin[16]
            report.Append("    Spectral Inversion Indicator: 0x" + bin[17] + " -- " + (bin[17].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Overrange Indicator: 0x" + bin[18] + " -- " + (bin[18].Equals('1') ? "True" : "False") + "\n");
            // Missing bin[19]

            // Missing bin[20]
            // Missing bin[21]
            // Missing bin[22]
            // Missing bin[23]

            report.Append("    Assoicated Context Packet Enabled: 0x" + bin[24] + " -- " + (bin[24].Equals('1') ? "True" : "False") + "\n");
            string sub = bin.Substring(25);
            report.Append("    Assoicated Context Packet Count: 0x" + sub + " -- Decimal " + Convert.ToInt32(sub.ToString().Replace(" ", string.Empty), 2).ToString() + "\n");

            validVita = (streamIDPres && classIDPres && trail && (dataPayloadType[0] || dataPayloadType[1])) ? true : false;
            return report.ToString();
        }

        public string processContextData(string bin, string binWords)
        {
            bin = bin.Replace(" ", string.Empty);
            StringBuilder report = new StringBuilder();
            report.Append("Context Packet Indicators:\n");

            report.Append("    Context Field Change Indicator: 0x" + bin[0] + " -- " + (bin[0].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Reference Point Indicator: 0x" + bin[1] + " -- " + (bin[1].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Bandwidth Indicator: 0x" + bin[2] + " -- " + (bin[2].Equals('1') ? "True" : "False") + "\n");
            report.Append("    IF Reference Frequency Indicator: 0x" + bin[3] + " -- " + (bin[3].Equals('1') ? "True" : "False") + "\n");

            report.Append("    RF Reference Frequency Indicator: 0x" + bin[4] + " -- " + (bin[4].Equals('1') ? "True" : "False") + "\n");
            report.Append("    RF Frequency Offset Indicator: 0x" + bin[5] + " -- " + (bin[5].Equals('1') ? "True" : "False") + "\n");
            report.Append("    IF Band Offset Indicator: 0x" + bin[6] + " -- " + (bin[6].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Reference Level Indicator: 0x" + bin[7] + " -- " + (bin[7].Equals('1') ? "True" : "False") + "\n");

            report.Append("    Gain Indicator: 0x" + bin[8] + " -- " + (bin[8].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Over-Range Count Indicator: 0x" + bin[9] + " -- " + (bin[9].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Sample Rate Indicator: 0x" + bin[10] + " -- " + (bin[10].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Timestamp Adjustment Indicator: 0x" + bin[11] + " -- " + (bin[11].Equals('1') ? "True" : "False") + "\n");

            report.Append("    Timestamp Calibration Time Indicator: 0x" + bin[12] + " -- " + (bin[12].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Temperature Indicator: 0x" + bin[13] + " -- " + (bin[13].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Device Identifier Indicator: 0x" + bin[14] + " -- " + (bin[14].Equals('1') ? "True" : "False") + "\n");
            report.Append("    State and Event Indicator: 0x" + bin[15] + " -- " + (bin[15].Equals('1') ? "True" : "False") + "\n");

            report.Append("    Data Packet Payload Format Indicator: 0x" + bin[16] + " -- " + (bin[16].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Formatted GPS (Global Positioning System) Geolocation Indicator: 0x" + bin[17] + " -- " + (bin[17].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Formatted INS (Intertial Navigation System) Geolocation Indicator: 0x" + bin[18] + " -- " + (bin[18].Equals('1') ? "True" : "False") + "\n");
            report.Append("    ECEF (Earth-Centered, Earth-Fixed) Ephemeris Indicator: 0x" + bin[19] + " -- " + (bin[19].Equals('1') ? "True" : "False") + "\n");

            report.Append("    Relative Ephemeris Indicator: 0x" + bin[20] + " -- " + (bin[20].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Ephemeris Reference Identifier Indicator: 0x" + bin[21] + " -- " + (bin[21].Equals('1') ? "True" : "False") + "\n");
            report.Append("    GPS ASCII Indicator: 0x" + bin[22] + " -- " + (bin[22].Equals('1') ? "True" : "False") + "\n");
            report.Append("    Context Association Lists Indicator: 0x" + bin[23] + " -- " + (bin[23].Equals('1') ? "True" : "False") + "\n\n");

            for (int i = 0; i < contextPackInd.Length; i++)
            {
                contextPackInd[i] = (bin[i].Equals('1') ? true : false);
            }

            if (contextPackInd.Contains(true))
            {
                binWords = binWords.Replace(" ", string.Empty);
                //System.Diagnostics.Debug.WriteLine("Binary Length " + binWords.Length);
                string temp = "";
                int ind = 0;
                for (int i = 0; i < contextPackInd.Length; i++)
                {
                    if (i == 0 && contextPackInd[i] == true)
                    {
                        // Do Nothing
                        // Words 0
                    }
                    else if ((i == 1 || i == 7 || i == 8 || i == 9 || i == 12 || i == 13 || i == 15 || i == 21) && contextPackInd[i] == true)
                    {
                        // Words 1
                        temp = binWords.Substring(ind, 32);
                        ind += 32;
                        if (i == 1 || i == 9 || i == 12)
                        {
                            string val = conversionToNums(temp, true);

                            if (i == 1)
                            {
                                report.Append("    Reference Point Identifier: " + val + "\n");
                            }
                            else if (i == 9)
                            {
                                report.Append("    Over-Range Count: " + val + "\n");
                            }
                            else if (i == 12)
                            {
                                report.Append("    Timestamp Calibration Time: " + val + "\n");
                            }
                        }
                        else if (i == 7)
                        {
                            temp = twosComplement(temp.Substring(16));

                            string refLevel = temp.Substring(16, 9);
                            string refLevelFrac = temp.Substring(25);

                            string val1 = conversionToNums(refLevel, true);
                            string val2 = conversionToNums(refLevelFrac, false);

                            report.Append("     Reference Level Value: " + val1 + "." + val2 + " dBm\n");
                        }
                        else if (i == 8)
                        {
                            string gainTemp1 = twosComplement(temp.Substring(0, 16));
                            string gainTemp2 = twosComplement(temp.Substring(16));
                            string temp2 = gainTemp1 + gainTemp2;

                            string gainS2int = temp2.Substring(0, 9);
                            string gainS2frac = temp2.Substring(9, 7);
                            string gainS1int = temp2.Substring(16, 9);
                            string gainS1frac = temp2.Substring(25);

                            string val1 = conversionToNums(gainS2int, true);
                            string val2 = conversionToNums(gainS1int, true);

                            string val3 = conversionToNums(gainS2frac, false);
                            string val4 = conversionToNums(gainS1frac, false);

                            report.Append("    Stage 2 Gain Value: " + val1 + "." + val3 + " dB\n");
                            report.Append("    Stage 1 Gain Value: " + val2 + "." + val4 + " dB\n");
                        }
                        else if (i == 13)
                        {
                            string tempInt = temp.Substring(16, 9);
                            string tempFrac = temp.Substring(25);

                            string val1 = conversionToNums(tempInt, true);
                            string val2 = conversionToNums(tempFrac, false);

                            report.Append("     Temperature: " + val1 + "." + val2 + " C\n");
                        }
                        else if (i == 15)
                        {
                            report.Append("State and Event Indicators:\n");

                            report.Append("     Calibrated Time Indicator Enabled: " + (temp[0].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Valid Data Indicator Enabled: " + (temp[1].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Referenece Lock Indicator Enabled: " + (temp[2].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     AGC/MGC Indicator Enabled: " + (temp[3].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Detected Signal Indicator Enabled: " + (temp[4].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Spectral Inversion Indicator Enabled: " + (temp[5].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Over-Range Indicator Enabled: " + (temp[6].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Sample Loss Indicator Enabled: " + (temp[7].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");

                            report.Append("     Calibrated Time Indicator: " + (temp[12].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Valid Data Indicator: " + (temp[13].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Referenece Lock Indicator: " + (temp[14].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     AGC/MGC Indicator: " + (temp[15].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Detected Signal Indicator: " + (temp[16].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Spectral Inversion Indicator: " + (temp[17].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Over-Range Indicator: " + (temp[18].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                            report.Append("     Sample Loss Indicator: " + (temp[19].Equals('1') ? " 0x1 True" : " 0x0 False") + "\n");
                        }
                        else if (i == 21)
                        {
                            // TODO -- Need Documentation
                        }

                    }
                    else if ((i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 10 || i == 11 || i == 14 || i == 16) && contextPackInd[i] == true)
                    {
                        // Words 2
                        temp = binWords.Substring(ind, 64);
                        ind += 64;
                        // 11 14
                        if (i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 10)
                        {
                            string freqInt = temp.Substring(0, 44);
                            string freqFrac = temp.Substring(44);

                            string val1 = conversionToNums(freqInt, false);
                            string val2 = conversionToNums(freqFrac, false);

                            string value = val1 + "." + val2 + " Hz\n";
                            if (i == 2)
                            {
                                report.Append("    Bandwidth: " + value);
                            }
                            else if (i == 3)
                            {
                                report.Append("    IF Reference Frequency: " + value);
                            }
                            else if (i == 4)
                            {
                                report.Append("    RF Reference Frequency: " + value);
                            }
                            else if (i == 5)
                            {
                                report.Append("    RF Reference Frequency Offset: " + value);
                            }
                            else if (i == 6)
                            {
                                report.Append("    IF Band Offset: " + value);
                            }
                            else if (i == 10)
                            {
                                sampRate = 1 / Convert.ToDouble(val1 + "." + val2);
                                report.Append("    Sample Rate: " + value);
                            }
                        }
                        else if (i == 11)
                        {
                            // TODO -- Picoseconds?
                        }
                        else if (i == 14)
                        {
                            // TODO -- Fig 5-7 Need Documentation
                        }
                        else if (i == 16)
                        {
                            dataPacketPayloadFormat = true;
                            string temp2 = "";
                            report.Append("\nData Packet Payload Format:\n");

                            report.Append("    Packing Method: " + temp[0] + "\n");
                            procEfficientPack = temp[0].Equals('0') ? true : false;

                            temp2 = temp.Substring(1, 2);
                            report.Append("    Real/Complex: " + dataSampleType[temp2] + "\n");
                            assignDataPayloadType(temp2);

                            temp2 = temp.Substring(3, 5);
                            report.Append("    Data Item Format: " + dataItemFormat[temp2] + "\n");

                            report.Append("    Sample-Component Repeat Indicator: " + (temp[8].Equals('1') ? "0x1 True" : "0x0 False") + "\n");
                            sampleComponentRep = temp[8].Equals('0') ? true : false;

                            temp2 = temp.Substring(9, 3);
                            report.Append("    Event-Tag Size: " + Convert.ToInt32(temp2, 2) + "\n");
                            eventTags = temp2.Equals("000") ? true : false;

                            temp2 = temp.Substring(12, 4);
                            report.Append("    Channel-Tag Size: " + Convert.ToInt32(temp2, 2) + "\n");
                            channelTags = temp2.Equals("0000") ? true : false;

                            temp2 = temp.Substring(20, 6);
                            report.Append("    Item Packing Field Size: " + Convert.ToInt32(temp2, 2) + "\n");

                            temp2 = temp.Substring(26, 6);
                            report.Append("    Data Item Size: " + Convert.ToInt32(temp2, 2) + "\n");

                            temp2 = temp.Substring(32, 16);
                            report.Append("    Repeat Count: " + Convert.ToInt32(temp2, 2) + "\n");

                            temp2 = temp.Substring(48);
                            report.Append("    Vector Size: " + Convert.ToInt32(temp2, 2) + "\n");
                        }
                    }
                    else if ((i == 17 || i == 18) && contextPackInd[i] == true)
                    {
                        // Words 11
                        temp = binWords.Substring(ind, 352);
                        ind += 352;

                        if (i == 17)
                        {
                            // TODO -- Need Documentation
                        }
                        else if (i == 18)
                        {
                            // TODO -- Need Documentation
                        }
                    }
                    else if ((i == 19 || i == 20) && contextPackInd[i] == true)
                    {
                        // Words 13
                        temp = binWords.Substring(ind, 416);
                        ind += 416;
                        
                        if (i == 19)
                        {
                            // TODO -- Need Documentation
                        }
                        else if (i == 20)
                        {
                            // TODO -- Need Documentation
                        }
                    }
                    else if ((i == 22 || i == 23) && contextPackInd[i] == true)
                    {
                        // Words Variable 
                        if (i == 22)
                        {
                            // TODO -- Need Documentation
                        }
                        else if (i == 23)
                        {
                            // TODO -- Need Documentation
                        }
                    }
                    temp = "";
                }
            }
            validVita = (dataPacketPayloadFormat && procEfficientPack && sampleComponentRep && eventTags && channelTags) ? true : false;
            return report.ToString();
        }

        public string processClassID(string classID)
        {
            StringBuilder report = new StringBuilder();
            report.Append("VRT Class ID:\n");
            string temp = "";
            string infoCode = "";
            string pktCode = "";
            //bool OUIeq = false;
            bool res = false;

            // Bits 0-4 Set Per VRT
            temp = classID.Substring(0, 5);
            report.Append("    Pad Bit Count: " + Convert.ToInt64(temp, 2) + "\n");

            // Bits 5-7 Reserved (set to 0 by default)

            // Bits 8-31 OUI
            temp = classID.Substring(8, 24);
            string OUI = Convert.ToInt64(temp).ToString("X");
            string temp2 = "";
            for (int i = 8 - OUI.Length; i > 0; i--)
                temp2 += "0";
            temp2 += OUI;
            report.Append("    OUI: 0x" + temp2 + "h\n");
            //OUIeq = temp2.Equals("FFFFFA") ? true : false;
            
            // Bits 32-39 Fixed Value
            temp = classID.Substring(32, 8);
            report.Append("    Fixed Value: " + Convert.ToInt64(temp, 2) + "\n");

            // Bits 40-41 Reserved (set to 0 by default)
            temp = classID.Substring(40, 2);
            res = temp.Equals("00") ? true : false;

            // Bits 42-43 Real or Complex, Cartesian
            temp = classID.Substring(42, 2);
            //assignDataPayloadType(temp);
            if (dataPayloadType[0])
                report.Append("    R/C: Real Data Payload Type \n");
            else if (dataPayloadType[1])
                report.Append("    R/C: Complex, Cartesian Data Payload Type \n");
            else if (dataPayloadType[2])
                report.Append("    R/C: Complex, Polar Data Payload Type \n");
            
            // Bits 44-47 Data Type
            temp = classID.Substring(44, 4);
            try
            {
                report.Append("    Data Type: " + dataFormatCodes[temp] + "\n");
                dataFormat = true;
            }
            catch
            {
                report.Append("    Data Type: Invalid\n");
                dataFormat = false;
            }

            // Bits 48-63 Vector Size
            temp = classID.Substring(48);
            report.Append("    Vector Size: " + Convert.ToInt64(temp, 2) + "\n");

            // Bits 32 - 47 Information Class Code
            temp = classID.Substring(32, 16);
            infoCode = Convert.ToInt64(temp).ToString("X");
            temp2 = "";
            for (int i = 8 - infoCode.Length; i > 0; i--)
                temp2 += "0";
            temp2 += infoCode;
            infoCode = temp2;
            report.Append("    Information Class Code: 0x" + infoCode + "h\n");

            // Bits 48 - 63 Packet Class Code
            temp = classID.Substring(48);
            pktCode = Convert.ToInt64(temp).ToString("X");
            temp2 = "";
            for (int i = 8 - pktCode.Length; i > 0; i--)
                temp2 += "0";
            temp2 += pktCode;
            pktCode = temp2;
            report.Append("    Packet Class Code: 0x" + pktCode + "h\n");

            validVita = (streamIDPres && classIDPres && trail && (dataPayloadType[0] || dataPayloadType[1]) && dataFormat && res) ? true : false;

            return report.ToString();
        }

        public string conversionToNums(string binStr, bool checkNeg) // Convert Binary to Decimal
        {
            string temp2 = "-";
            string val = "";
            if (binStr[0].Equals('1') == true && checkNeg == true)
            {
                string myNum = twosComplement(binStr);
                string num = Convert.ToInt64(myNum, 2).ToString();
                temp2 += num;
                val = temp2;
            }
            else
            {
                val = Convert.ToInt64(binStr, 2).ToString();
            }
            return val;
        }

        public string twosComplement(string binStr)
        {
            string temp1 = "";
            for(int j = 0; j < binStr.Length; j++)
            {
                if (binStr[j].Equals('1'))
                {
                    temp1 += '0';
                }
                else
                {
                    temp1 += '1';
                }
            }
            binStr = temp1;
            long num1 = Convert.ToInt64(binStr, 2);
            long num2 = Convert.ToInt64("0001", 2);

            string twosComp = Convert.ToString(num1 + num2, 2);
            string temp = "";
            for (int i = binStr.Length - twosComp.Length; i > 0; i--)
            {
                temp += "0";
            }
            temp += twosComp;
            return temp;
        }

        public void assignDataPayloadType(string type)
        {
            for (int i = 0; i < dataPayloadType.Length; i++)
                dataPayloadType[i] = false;

            if (type.Equals("00")) // Real
                dataPayloadType[0] = true;
            else if (type.Equals("01")) // Complex, Cartesian
                dataPayloadType[1] = true;
            else if (type.Equals("10")) // Complex, Polar
                dataPayloadType[2] = true;
        }

        public bool setTimestampBools(string timeField)
        {
            return timeField.Equals("00") ? false : true;
        }

        public void setupDictionaries() // According to V49A Spec Sheet
        {
            packetType.Add("0000", "Signal Data Packet without Stream Identifier");
            packetType.Add("0001", "Signal Data Packet with Stream Identifier");
            packetType.Add("0010", "Extension Data Packet without Stream Identifier");
            packetType.Add("0011", "Extension Data Packet with Stream Identifier");
            packetType.Add("0100", "Context Packet");
            packetType.Add("0101", "Extension Context Packet");
            packetType.Add("0110", "Reserved");
            packetType.Add("0111", "Reserved");
            packetType.Add("1000", "Reserved");
            packetType.Add("1001", "Reserved");
            packetType.Add("1010", "Reserved");
            packetType.Add("1011", "Reserved");

            TSI.Add("00", "No Integer-seconds Timestamp field included");
            TSI.Add("01", "UTC");
            TSI.Add("10", "GPS Time");
            TSI.Add("11", "Other");

            TSF.Add("00", "No Fractional-seconds Timestamp Field Included");
            TSF.Add("01", "Sample Count Timestamp");
            TSF.Add("10", "Real-Time (Picoseconds) Timestamp");
            TSF.Add("11", "Free Running Count Timestamp");

            dataFormatCodes.Add("0001", "4-bit Signed Fixed Point");
            dataFormatCodes.Add("0010", "8-bit Signed Fixed Point");
            dataFormatCodes.Add("0011", "16-bit Signed Fixed Point");
            dataFormatCodes.Add("0100", "32-bit Signed Fixed Point");
            dataFormatCodes.Add("0101", "64-bit Signed Fixed Point");
            dataFormatCodes.Add("0110", "32-bit IEEE 754 Single Precision");
            dataFormatCodes.Add("0111", "64-bit IEEE 754 Single Precision");
            dataFormatCodes.Add("1000", "1-bit Unsigned Fixed Point");
            dataFormatCodes.Add("1001", "4-bit Unsigned Fixed Point");
            dataFormatCodes.Add("1010", "8-bit Unsigned Fixed Point");
            dataFormatCodes.Add("1011", "16-bit Unsigned Fixed Point");
            dataFormatCodes.Add("1100", "32-bit Unsigned Fixed Point");
            dataFormatCodes.Add("1101", "64-bit Unsigned Fixed Point");

            dataSampleType.Add("00", "Real");
            dataSampleType.Add("01", "Complex, Cartesian");
            dataSampleType.Add("10", "Complex, Polar");

            dataItemFormat.Add("00000", "Signed Fixed-Point");
            dataItemFormat.Add("00001", "Signed VRT, 1-bit Exponent");
            dataItemFormat.Add("00010", "Signed VRT, 2-bit Exponent");
            dataItemFormat.Add("00011", "Signed VRT, 3-bit Exponent");
            dataItemFormat.Add("00100", "Signed VRT, 4-bit Exponent");
            dataItemFormat.Add("00101", "Signed VRT, 5-bit Exponent");
            dataItemFormat.Add("00110", "Signed VRT, 6-bit Exponent");
            dataItemFormat.Add("01101", "IEEE-754 Half-Precision Floating-Point");
            dataItemFormat.Add("01110", "IEEE-754 Single-Precision Floating-Point");
            dataItemFormat.Add("01111", "IEEE-754 Double-Precision Floating-Point");
            dataItemFormat.Add("10000", "Unsigned Fixed-Point");
            dataItemFormat.Add("10001", "Unsigned VRT, 1-bit Exponent");
            dataItemFormat.Add("10010", "Unsigned VRT, 2-bit Exponent");
            dataItemFormat.Add("10011", "Unsigned VRT, 3-bit Exponent");
            dataItemFormat.Add("10100", "Unsigned VRT, 4-bit Exponent");
            dataItemFormat.Add("10101", "Unsigned VRT, 5-bit Exponent");
            dataItemFormat.Add("10110", "Unsigned VRT, 6-bit Exponent");
        }
    }
}
