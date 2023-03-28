using System;
using System.Net.Sockets;
using System.Text;

// Include the ProtoIP namespace
using ProtoIP;

class Program {
      static void Main(string[] args) {
            // Create a new packet
            Packet packet = new Packet(Packet.Type.BYTES);
            packet.SetData("Hello World!");

            // Serialize the packet
            byte[] serializedPacket = packet.Serialize();

            // Connect to the server
            TcpClient client = new TcpClient("1.1.1.1", 1234);
            NetworkStream stream = client.GetStream();

            // Send the packet over the network
            stream.Write(serializedPacket, 0, serializedPacket.Length);
      }
}