using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOUNDDOG_GUI
{
    class Vita49
    {
        Dictionary<string, string> packetType = new Dictionary<string, string>();
        Dictionary<string, string> TSI = new Dictionary<string, string>();
        Dictionary<string, string> TSF = new Dictionary<string, string>();
        Dictionary<string, string> dataFormatCodes = new Dictionary<string, string>();
        bool trail = false;
        bool dataPack = false;
        bool validVita = false;
        bool[] contextPackInd = new bool[24];

        public bool Trailer
        {
            get { return trail; }
            set { trail = value; }
        }

        public bool PackType
        {
            get { return dataPack; }
            set { dataPack = value; }
        }

        public bool valVita
        {
            get { return validVita; }
        }

        public Vita49()
        {
            setupDictionaries();
        }

        public string parseHeader(string bin)
        {
            PackType = false;
            validVita = false;
            StringBuilder report = new StringBuilder();
            report.Append("VRT Header: \n");
            try
            {
                bin = bin.Replace(" ", string.Empty);
                StringBuilder str = new StringBuilder(bin.Substring(0, 4));
                if (str.ToString().Equals("0100") != true && str.ToString().Equals("0101") != true) // Data Packet
                {
                    report.Append("Type: 0x" + str.ToString() + " -- " + packetType[str.ToString()] + "\n");

                    report.Append((bin[4].Equals('1') ? "Class: 0x1 -- Class ID present.\n" : "Class: 0x0 -- Class ID not present.\n"));

                    report.Append((bin[5].Equals('1') ? "Trailer: 0x1 -- Trailer is present.\n" : "Trailer: 0x0 -- TRAILER MUST BE PRESENT IN VITA-49A.\n"));
                    
                    if (bin[5].Equals('1'))
                    {
                        trail = true;
                        validVita = true;
                    }
                    else
                    {
                        validVita = false;
                    }

                    str.Clear();
                    str.Append(bin.Substring(8, 2));
                    report.Append("TSI: 0x" + str.ToString() + " -- " + TSI[str.ToString()] + "\n");

                    str.Clear();
                    str.Append(bin.Substring(10, 2));
                    report.Append("TSF: 0x" + str.ToString() + " -- " + TSF[str.ToString()] + "\n");

                    str.Clear();
                    str.Append(bin.Substring(12, 4));
                    report.Append("Count: 0x" + str.ToString() + " : Decimal -- " + Convert.ToInt32(str.ToString().Replace(" ", string.Empty), 2).ToString() + "\n");

                    str.Clear();
                    str.Append(bin.Substring(16, bin.Length - 17));
                    report.Append("Size: 0x" + str.ToString().Replace(" ", string.Empty) + " : Decimal -- " + Convert.ToInt32(str.ToString().Replace(" ", string.Empty), 2).ToString() + "\n");

                    if (validVita != true)
                    {
                        report.Append("Not Vita-49 compliant.\n");
                    }
                    PackType = true;
                    return report.ToString();
                }
                else if (str.ToString().Equals("0100") == true || str.ToString().Equals("0101") == true) // Context Packet
                {
                    report.Append("Type: 0x" + str.ToString() + " -- " + packetType[str.ToString()] + "\n");

                    report.Append((bin[4].Equals('1') ? "Class: 0x1 -- Class ID present.\n" : "Class: 0x0 -- Class ID not present.\n"));

                    report.Append((bin[7].Equals('1') ? "Timestamp Mode: 0x1 -- General Event Timing.\n" :"Timestamp Mode: 0x0 -- Precise Event Timing\n"));

                    str.Clear();
                    str.Append(bin.Substring(8, 2));
                    report.Append("TSI: 0x" + str.ToString() + " -- " + TSI[str.ToString()] + "\n");

                    str.Clear();
                    str.Append(bin.Substring(10, 2));
                    report.Append("TSF: 0x" + str.ToString() + " -- " + TSF[str.ToString()] + "\n");

                    str.Clear();
                    str.Append(bin.Substring(12, 4));
                    report.Append("Count: 0x" + str.ToString() + " : Decimal -- " + Convert.ToInt32(str.ToString().Replace(" ", string.Empty), 2).ToString() + "\n");

                    str.Clear();
                    str.Append(bin.Substring(16, bin.Length - 17));
                    report.Append("Size: 0x" + str.ToString().Replace(" ", string.Empty) + " : Decimal -- " + Convert.ToInt32(str.ToString().Replace(" ", string.Empty), 2).ToString() + "\n");

                    validVita = true;
                    return report.ToString();
                }
            }
            catch
            {
                validVita = false;
                return report.ToString() + "Not Vita-49 compliant.\n";
            }
            return "";
        }

        public string parseTrailer(string bin)
        {
            //throw new NotImplementedException("Oops");
            bin = bin.Replace(" ", string.Empty);
            StringBuilder report = new StringBuilder();
            report.Append("VRT Trailer: \n");

            report.Append("Calibrated Time Indicator Enable: 0x" + bin[0] + " -- " + (bin[0].Equals('1') ? "True" : "False") + "\n");
            report.Append("Valid Signal Indicator Enable: 0x" + bin[1] + " -- " + (bin[1].Equals('1') ? "True" : "False") + "\n");
            report.Append("Reference Lock Indicator Enable: 0x" + bin[2] + " -- " + (bin[2].Equals('1') ? "True" : "False") + "\n");
            report.Append("AGC/MGC Indicator Enable: 0x" + bin[3] + " -- " + (bin[3].Equals('1') ? "True" : "False") + "\n");

            // Missing bin[4]
            report.Append("Spectral Inversion Indicator Enable: 0x" + bin[5] + " -- " + (bin[5].Equals('1') ? "True" : "False") + "\n");
            report.Append("Overrange Indicator Enable: 0x" + bin[6] + " -- " + (bin[6].Equals('1') ? "True" : "False") + "\n");
            // Missing bin[7]

            // Missing bin[8]
            // Missing bin[9]
            // Missing bin[10]
            // Missing bin[11]

            report.Append("Calibrated Time Indicator: 0x" + bin[12] + " -- " + (bin[12].Equals('1') ? "True" : "False") + "\n");
            report.Append("Valid Signal Indicator: 0x" + bin[13] + " -- " + (bin[13].Equals('1') ? "True" : "False") + "\n");
            report.Append("Reference Lock Indicator: 0x" + bin[14] + " -- " + (bin[14].Equals('1') ? "True" : "False") + "\n");
            report.Append("AGC/MGC Indicator: 0x" + bin[15] + " -- " + (bin[15].Equals('1') ? "True" : "False") + "\n");

            // Missing bin[16]
            report.Append("Spectral Inversion Indicator: 0x" + bin[17] + " -- " + (bin[17].Equals('1') ? "True" : "False") + "\n");
            report.Append("Overrange Indicator: 0x" + bin[18] + " -- " + (bin[18].Equals('1') ? "True" : "False") + "\n");
            // Missing bin[19]

            // Missing bin[20]
            // Missing bin[21]
            // Missing bin[22]
            // Missing bin[23]

            report.Append("Assoicated Context Packet Enabled: 0x" + bin[24] + " -- " + (bin[24].Equals('1') ? "True" : "False") + "\n");
            string sub = bin.Substring(25);
            report.Append("Assoicated Context Packet Count: 0x" + sub + " -- Decimal " + Convert.ToInt32(sub.ToString().Replace(" ", string.Empty), 2).ToString() + "\n");

            validVita = true;
            return report.ToString();
        }

        public string processContextData(string bin, string binWords)
        {
            bin = bin.Replace(" ", string.Empty);
            StringBuilder report = new StringBuilder();
            report.Append("Context Packet Indicators:\n");

            report.Append("Context Field Change Indicator: 0x" + bin[0] + " -- " + (bin[0].Equals('1') ? "True" : "False") + "\n");
            report.Append("Reference Point Indicator: 0x" + bin[1] + " -- " + (bin[1].Equals('1') ? "True" : "False") + "\n");
            report.Append("Bandwidth Indicator: 0x" + bin[2] + " -- " + (bin[2].Equals('1') ? "True" : "False") + "\n");
            report.Append("IF Reference Frequency Indicator: 0x" + bin[3] + " -- " + (bin[3].Equals('1') ? "True" : "False") + "\n");

            report.Append("RF Reference Frequency Indicator: 0x" + bin[4] + " -- " + (bin[4].Equals('1') ? "True" : "False") + "\n");
            report.Append("RF Frequency Offset Indicator: 0x" + bin[5] + " -- " + (bin[5].Equals('1') ? "True" : "False") + "\n");
            report.Append("IF Band Offset Indicator: 0x" + bin[6] + " -- " + (bin[6].Equals('1') ? "True" : "False") + "\n");
            report.Append("Reference Level Indicator: 0x" + bin[7] + " -- " + (bin[7].Equals('1') ? "True" : "False") + "\n");

            report.Append("Gain Indicator: 0x" + bin[8] + " -- " + (bin[8].Equals('1') ? "True" : "False") + "\n");
            report.Append("Over-Range Count Indicator: 0x" + bin[9] + " -- " + (bin[9].Equals('1') ? "True" : "False") + "\n");
            report.Append("Sample Rate Indicator: 0x" + bin[10] + " -- " + (bin[10].Equals('1') ? "True" : "False") + "\n");
            report.Append("Timestamp Adjustment Indicator: 0x" + bin[11] + " -- " + (bin[11].Equals('1') ? "True" : "False") + "\n");

            report.Append("Timestamp Calibration Time Indicator: 0x" + bin[12] + " -- " + (bin[12].Equals('1') ? "True" : "False") + "\n");
            report.Append("Temperature Indicator: 0x" + bin[13] + " -- " + (bin[13].Equals('1') ? "True" : "False") + "\n");
            report.Append("Device Identifier Indicator: 0x" + bin[14] + " -- " + (bin[14].Equals('1') ? "True" : "False") + "\n");
            report.Append("State and Event Indicator: 0x" + bin[15] + " -- " + (bin[15].Equals('1') ? "True" : "False") + "\n");

            report.Append("Data Packet Payload Format Indicator: 0x" + bin[16] + " -- " + (bin[16].Equals('1') ? "True" : "False") + "\n");
            report.Append("Formatted GPS (Global Positioning System) Geolocation Indicator: 0x" + bin[17] + " -- " + (bin[17].Equals('1') ? "True" : "False") + "\n");
            report.Append("Formatted INS (Intertial Navigation System) Geolocation Indicator: 0x" + bin[18] + " -- " + (bin[18].Equals('1') ? "True" : "False") + "\n");
            report.Append("ECEF (Earth-Centered, Earth-Fixed) Ephemeris Indicator: 0x" + bin[19] + " -- " + (bin[19].Equals('1') ? "True" : "False") + "\n");

            report.Append("Relative Ephemeris Indicator: 0x" + bin[20] + " -- " + (bin[20].Equals('1') ? "True" : "False") + "\n");
            report.Append("Ephemeris Reference Identifier Indicator: 0x" + bin[21] + " -- " + (bin[21].Equals('1') ? "True" : "False") + "\n");
            report.Append("GPS ASCII Indicator: 0x" + bin[22] + " -- " + (bin[22].Equals('1') ? "True" : "False") + "\n");
            report.Append("Context Association Lists Indicator: 0x" + bin[23] + " -- " + (bin[23].Equals('1') ? "True" : "False") + "\n\n");

            for (int i = 0; i < contextPackInd.Length; i++)
            {
                contextPackInd[i] = (bin[i].Equals('1') ? true : false);
            }

            if (contextPackInd.Contains(true))
            {
                binWords = binWords.Replace(" ", string.Empty);
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
                        // 8 13 15 21
                        if (i == 1 || i == 9 || i == 12)
                        {
                            string val = conversionToNums(temp);
                            report.Append("Context Field " + i + " Value: " + val + "\n");
                        }
                        else if (i == 7)
                        {
                            string refLevel = temp.Substring(16, 9);
                            string val = conversionToNums(refLevel);
                            report.Append("Reference Level Value: " + val + "\n");
                        }
                        else if (i == 8)
                        {
                            string gainS2 = temp.Substring(0, 9);
                            string gainS1 = temp.Substring(16, 9);

                            string val1 = conversionToNums(gainS2);
                            string val2 = conversionToNums(gainS1);

                            report.Append("Stage 2 Gain Value: " + val1 + "\n");
                            report.Append("Stage 1 Gain Value: " + val2 + "\n");
                        }
                       
                    }
                    else if ((i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 10 || i == 11 || i == 14 || i == 16) && contextPackInd[i] == true)
                    {
                        // Words 2
                        temp = binWords.Substring(ind, 64);
                        ind += 64;
                    }
                    else if ((i == 17 || i == 18) && contextPackInd[i] == true)
                    {
                        // Words 11
                        temp = binWords.Substring(ind, 352);
                        ind += 352;
                    }
                    else if ((i == 19 || i == 20) && contextPackInd[i] == true)
                    {
                        // Words 13
                        temp = binWords.Substring(ind, 416);
                        ind += 416;
                    }
                    else if ((i == 22 || i == 23) && contextPackInd[i] == true)
                    {
                        // Words Variable
                    }
                    temp = "";
                }
            }

            return report.ToString();
        }

        public string conversionToNums(string binStr)
        {
            string temp2 = "-";
            string val = "";
            if (binStr[0].Equals('1') == true)
            {
                string myNum = twosComplement(binStr);
                string num = Convert.ToInt32(myNum, 2).ToString();
                temp2 += num;
                val = temp2;
            }
            else
            {
                val = Convert.ToInt32(binStr, 2).ToString();
            }
            return val;
        }

        public string twosComplement(string binStr)
        {
            int num1 = Convert.ToInt32(binStr, 2);
            int num2 = Convert.ToInt32("0001", 2);

            string twosComp = Convert.ToString(num1 + num2, 2);
            string temp = "";
            for (int i = binStr.Length - twosComp.Length; i > 0; i--)
            {
                temp += "0";
            }
            temp += twosComp;
            return temp;
        }

        public void setupDictionaries()
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
        }
    }
}
