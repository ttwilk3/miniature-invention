# HOUNDDOG

#### Used for sniffing packets from Software Defined Radios and parsing the outputted Vita 49A packets.

##### [Winpcap dependency](https://www.winpcap.org/)

##### You can currently:
* Change the save location of the parsed data.
* Choose different interfaces of your computer to use as a capture point.
* Turn on or off full Verbosity in the report.
* Cap how many packets are captured.
* Look at the live packet count graph to monitor performance.
* Look at the Spectral Display showing the data payloads from the packets.
* Change the refresh rate of the Spectral Display.
* Look at the output file "invalidPackets.txt" in the directory HOUNDDOG is run in for a file of all the invalid Vita 49A packets.

##### TODO:
* Look into Linux Compatibility.
* Work on IQ graph of Complex data payloads. (Current spectral display only works for Real data payloads)
* Add file input (wireshark capture) as an option, so you don't have to live capture the data when running.
* More Verbosity levels to choose from.
* Possibly more live data graphs?
* Add more fields to be parsed from data and context packets.
* Get more documentation for V49a format and complete the missing TODO fields for the packets.
* Eventually add VITA 49.0, 49B, and 49.2 support.


## Example Parsed Packet Data:
[Example Packets Gathered from a MMS MSDD-3000-1](https://www.mms-rf.com/products/msdd-3000-pps)

Ran in SCAN mode, outputting VITA-49.
### Example Valid Data Packet:
Coming soon...

### Example Invalid Data Packet:
-----------------------------------------
	Content of packet : 
	    Packet #: 0
		Caplength: 1038
		Length   : 1038
		Timestamp: Wednesday, July 5, 2017 2:37:08 PM
	    Source: 192.168.0.142
	    Destination: 192.168.0.103
	    Source Port: 1266
	    Destination Port: 4800
	    VRT Header: 0x00110000 10100001 00000000 11111001
	    Stream ID: 0x00000000h
	    Packet Type: Data

	VRT Header: 
	Type: 0x0011 -- Extension Data Packet with Stream Identifier
	Class: 0x0 -- Class ID not present. -- CLASS ID MUST BE PRESENT IN VITA-49A
	Trailer: 0x0 -- Trailer is not present. -- TRAILER MUST BE PRESENT IN VITA-49A.
	TSI: 0x10 -- GPS Time
	TSF: 0x10 -- Real-Time (Picoseconds) Timestamp
	Count: 0x0001 : Decimal -- 1
	Size: 0x000000001111100 : Decimal -- 124
	Not Vita-49 compliant.

-----------------------------------------

### Example Valid Context Packet:

-----------------------------------------
	Content of packet : 
	    Packet #: 1
		Caplength: 114
		Length   : 114
		Timestamp: Wednesday, July 5, 2017 2:37:08 PM
	    Source: 192.168.0.142
	    Destination: 192.168.0.103
	    Source Port: 1266
	    Destination Port: 4800
	    VRT Header: 0x01000001 10100011 00000000 00010010
	    Stream ID: 0x00000000h
	    Packet Type: Context

	VRT Header: 
	Type: 0x0100 -- Context Packet
	Class: 0x0 -- Class ID not present.
	Timestamp Mode: 0x1 -- General Event Timing.
	TSI: 0x10 -- GPS Time
	TSF: 0x10 -- Real-Time (Picoseconds) Timestamp
	Count: 0x0011 : Decimal -- 3
	Size: 0x000000000001001 : Decimal -- 9

	Context Packet Data:
	0x1C E0 80 00 
	00 05 A4 D1 
	0C 00 00 00 
	00 00 1C 9C 
	38 00 00 00 
	00 00 00 00 
	00 00 00 00 
	FC C0 06 00 
	00 00 00 00 
	00 00 17 70 
	00 00 00 00 
	80 00 03 CF 
	00 00 00 00 
	h

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
	Stage 2 Gain Value: -7.64 dB
	Stage 1 Gain Value: 12.0 dB
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
	Content of packet : 
	    Packet #: 0
		Caplength: 47
		Length   : 47
		Timestamp: Wednesday, July 5, 2017 3:44:20 PM
	    Source: 192.168.0.103
	    Destination: 192.168.0.142
	    Source Port: 25573
	    Destination Port: 202
	    VRT Header: 0x
	    Stream ID: 
	    Packet Type: Context


	Context Packet Data:
	0xh


-----------------------------------------

Credit to [beto-rodriguez](https://github.com/beto-rodriguez) for the [Live Chart Libraries](https://github.com/beto-rodriguez/Live-Charts).
