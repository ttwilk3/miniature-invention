using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.LibPcap;
using PacketDotNet;

namespace HOUNDDOG_GUI
{
    class PcapReader
    {
        ICaptureDevice device;
        sockets sock;

        public PcapReader(string capFile, sockets sock_, Form1 frm)
        {
            sock = sock_;
            try
            {
                // Get an offline device
                device = new CaptureFileReaderDevice(capFile);

                // Open the device
                device.Open();
            }
            catch (Exception e)
            {
                MetroFramework.MetroMessageBox.Show(frm, "Caught exception when opening file" + e.ToString());
                //Console.WriteLine("Caught exception when opening file" + e.ToString());
                //Console.ReadKey();
                return;
            }

            // Register our handler function to the 'packet arrival' event
            device.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival);

            //Console.WriteLine();
            //Console.WriteLine
            //    ("-- Capturing from '{0}', hit 'Ctrl-C' to exit...",
            //    capFile);

            // Start capture 'INFINTE' number of packets
            // This method will return when EOF reached.
            device.Capture();

            // Close the pcap device
            device.Close();

            return;
            //Console.WriteLine("-- End of file reached.");
            //Console.Write("Hit 'Enter' to exit...");
            //Console.ReadLine();
        }
        
        /// <summary>
        /// Prints the source and dest MAC addresses of each received Ethernet frame
        /// </summary>
        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            if (e.Packet.LinkLayerType == PacketDotNet.LinkLayers.Ethernet)
            {
                var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
                byte[] s = packet.Bytes;

                PacketHeader p = new PacketHeader(new dotnetWinpCap.timeval(), s.Length, s.Length);

                sock.ReceivePacket(null, p, s);
            }
        }
    }
}
