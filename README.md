# HOUNDDOG

#### Used for sniffing packets from Software Defined Radios and parsing the outputted Vita 49A packets.

##### [Winpcap dependency](https://www.winpcap.org/)

##### You can currently:
* Run HOUNDDOG on a pcap file.
* Change the save location of the parsed data.
* Choose different interfaces of your computer to use as a capture point.
* Turn on or off full Verbosity in the report.
* Cap how many packets are captured.
* Look at the live packet count graph to monitor performance.
* Look at the Spectral Display showing the data payloads from the packets. Currently supports Real and Complex, Cartesian payloads.
* Change the refresh rate of the Spectral Display.
* Look at the output file "invalidPackets.txt" for a file of all the invalid Vita 49A packets.

##### TODO:
* Refactor all the conversion functions in sockets.cs, many could be consolidated.
* Add Scaling to GUI, currently it is a fixed size.
* Get more documentation for V49a format and complete the missing TODO fields for the packets. Specifically the Ephemris Fields.
* More Verbosity levels to choose from.
* Eventually add VITA 49.0, 49B, and 49.2 support.

##### In Progress:
* ~~Look into Linux Compatibility.~~ [(Try Using Wine)](https://www.winehq.org/)
* ~~Add file input (wireshark capture, pcap file) as an option, so you don't have to live capture the data when running.~~
* ~~Work on IQ graph of Complex data payloads. (Current spectral display only works for Real data payloads)~~
* ~~Add more fields to be parsed from data and context packets.~~
* ~~Possibly more live data graphs? (There already are packet counters for Data, Context, and Other)~~


## Example Parsed Packet Data:
[Example Packets Gathered from a MMS MSDD-3000-1](https://www.mms-rf.com/products/msdd-3000-pps)

Out of the several radios I tested, none of them were outputting valid Vita-49A packets. So I've included an "Almost-Valid" Data and Context packet. These are the two that are closest to what a valid packet should look like when parsed.

Ran in SCAN mode, outputting VITA-49.
### Example Almost-Valid Data Packet:
-----------------------------------------
	Not Vita-49 A compliant.

	Content of packet : 
	    Packet #: 0
	    Length   : 8266
	    Timestamp: Friday, June 30, 2017 6:10:36 PM
	    Source: 192.168.128.103
	    Destination: 255.255.255.255
	    Source Port: 5603
	    Destination Port: 7285
	    VRT Header: 0x00011100 01101011 00001000 00001000
	    Stream ID: 0x731C0029h
	    VRT Trailer: 0x11110100 00000100 00000000 00000000
	    Packet Type: Data

	VRT Header: 
	    Type: 0x0001 -- Signal Data Packet with Stream Identifier
	    Class: 0x1 -- Class ID present.
	    Trailer: 0x1 -- Trailer is present.
	    TSI: 0x01 -- UTC
	    TSF: 0x10 -- Real-Time (Picoseconds) Timestamp
	    Count: 0x1011 : Decimal -- 11
	    Size: 0x000010000000100 : Decimal -- 1028

	VRT Class ID:
	    Pad Bit Count: 0
	    OUI: 0x00000000h
	    Fixed Value: 0
	    Data Type: Invalid
	    Vector Size: 0
	    Information Class Code: 0x00000000h
	    Packet Class Code: 0x00000000h

	VRT Trailer: 
	    Calibrated Time Indicator Enable: 0x1 -- True
	    Valid Signal Indicator Enable: 0x1 -- True
	    Reference Lock Indicator Enable: 0x1 -- True
	    AGC/MGC Indicator Enable: 0x1 -- True
	    Spectral Inversion Indicator Enable: 0x1 -- True
	    Overrange Indicator Enable: 0x0 -- False

	    Calibrated Time Indicator: 0x0 -- False
	    Valid Signal Indicator: 0x1 -- True
	    Reference Lock Indicator: 0x0 -- False
	    AGC/MGC Indicator: 0x0 -- False
	    Spectral Inversion Indicator: 0x0 -- False
	    Overrange Indicator: 0x0 -- False
	    Assoicated Context Packet Enabled: 0x0 -- False
	    Assoicated Context Packet Count: 0x0000000 -- Decimal 0

-----------------------------------------

### Example Invalid Data Packet:
-----------------------------------------
	Not Vita-49 A compliant.

	Content of packet : 
	    Packet #: 2
	    Length   : 1086
	    Timestamp: Thursday, July 6, 2017 2:51:58 PM
	    Source: 192.168.0.142
	    Destination: 192.168.0.103
	    Source Port: 1266
	    Destination Port: 4800
	    VRT Header: 0x00110000 10100010 00000001 00000101
	    Stream ID: 0x00000000h
	    Packet Type: Data

	VRT Header: 
	    Type: 0x0011 -- Extension Data Packet with Stream Identifier
	    Class: 0x0 -- Class ID not present. -- CLASS ID MUST BE PRESENT IN VITA-49A
	    Trailer: 0x0 -- Trailer is not present. -- TRAILER MUST BE PRESENT IN VITA-49A.
	    TSI: 0x10 -- GPS Time
	    TSF: 0x10 -- Real-Time (Picoseconds) Timestamp
	    Count: 0x0010 : Decimal -- 2
	    Size: 0x000000010000010 : Decimal -- 130

-----------------------------------------

### Example Almost-Valid Context Packet:

-----------------------------------------
	Not Vita-49 A compliant.

	Content of packet : 
	    Packet #: 4
	    Length   : 114
	    Timestamp: Thursday, July 6, 2017 2:51:58 PM
	    Source: 192.168.0.142
	    Destination: 192.168.0.103
	    Source Port: 1266
	    Destination Port: 4800
	    VRT Header: 0x01000001 10101100 00000000 00010010
	    Stream ID: 0x00000000h
	    Packet Type: Context

	VRT Header: 
	    Type: 0x0100 -- Context Packet
	    Class: 0x0 -- Class ID not present.
	    Timestamp Mode: 0x1 -- General Event Timing.
	    TSI: 0x10 -- GPS Time
	    TSF: 0x10 -- Real-Time (Picoseconds) Timestamp
	    Count: 0x1100 : Decimal -- 12
	    Size: 0x000000000001001 : Decimal -- 9

	Context Packet Data:
	0x1C E0 80 00 00 05 A4 D1 
	0C 00 00 00 00 00 1C 9C 
	38 00 00 00 00 00 00 00 
	00 00 00 00 FC C0 06 00 
	00 00 00 00 00 00 17 70 
	00 00 00 00 80 00 03 CF 
	00 00 00 00 h

	Context Packet Indicators:
	    Context Field Change Indicator: 0x0 -- False
	    Reference Point Indicator: 0x0 -- False
	    Bandwidth Indicator: 0x0 -- False
	    IF Reference Frequency Indicator: 0x1 -- True
	    RF Reference Frequency Indicator: 0x1 -- True
	    RF Frequency Offset Indicator: 0x1 -- True
	    IF Band Offset Indicator: 0x0 -- False
	    Reference Level Indicator: 0x0 -- False
	    Gain Indicator: 0x1 -- True
	    Over-Range Count Indicator: 0x1 -- True
	    Sample Rate Indicator: 0x1 -- True
	    Timestamp Adjustment Indicator: 0x0 -- False
	    Timestamp Calibration Time Indicator: 0x0 -- False
	    Temperature Indicator: 0x0 -- False
	    Device Identifier Indicator: 0x0 -- False
	    State and Event Indicator: 0x0 -- False
	    Data Packet Payload Format Indicator: 0x1 -- True
	    Formatted GPS (Global Positioning System) Geolocation Indicator: 0x0 -- False
	    Formatted INS (Intertial Navigation System) Geolocation Indicator: 0x0 -- False
	    ECEF (Earth-Centered, Earth-Fixed) Ephemeris Indicator: 0x0 -- False
	    Relative Ephemeris Indicator: 0x0 -- False
	    Ephemeris Reference Identifier Indicator: 0x0 -- False
	    GPS ASCII Indicator: 0x0 -- False
	    Context Association Lists Indicator: 0x0 -- False

	    IF Reference Frequency: 1515000000.0 Hz
	    RF Reference Frequency: 30000000.0 Hz
	    RF Reference Frequency Offset: 0.0 Hz
	    Stage 2 Gain Value: 6.64 dB
	    Stage 1 Gain Value: -12.0 dB
	    Over-Range Count: 0
	    Sample Rate: 24576000.0 Hz

	Data Packet Payload Format:
	    Packing Method: 1
	    Real/Complex: Real
	    Data Item Format: Signed Fixed-Point
	    Sample-Component Repeat Indicator: 0x0 False
	    Event-Tag Size: 0
	    Channel-Tag Size: 0
	    Item Packing Field Size: 15
	    Data Item Size: 15
	    Repeat Count: 0
	    Vector Size: 0

-----------------------------------------

### Example Invalid Context Packet:
-----------------------------------------
	Not Vita-49 A compliant.

	Content of packet : 
	    Packet #: 1
	    Length   : 60
	    Timestamp: Thursday, July 6, 2017 2:51:58 PM
	    Source: 192.168.0.142
	    Destination: 192.168.0.103
	    Source Port: 23
	    Destination Port: 6087
	    VRT Header: 0x01000011 01010000 01001100 00100000
	    Stream ID: 0x34372E30h
	    Packet Type: Context

	VRT Header: 
	    Type: 0x0100 -- Context Packet
	    Class: 0x0 -- Class ID not present.
	    Timestamp Mode: 0x1 -- General Event Timing.
	    TSI: 0x01 -- UTC
	    TSF: 0x01 -- Sample Count Timestamp
	    Count: 0x0000 : Decimal -- 0
	    Size: 0x010011000010000 : Decimal -- 9744

	Context Packet Data:
	0xh


-----------------------------------------

Credit to [beto-rodriguez](https://github.com/beto-rodriguez) for the [Live Chart Libraries](https://github.com/beto-rodriguez/Live-Charts).
