using System;
using System.Net.Sockets;
using System.Text;

// Include the ProtoIP namespace
using ProtoIP;

class Program {
      static void Main(string[] args) {
            // Connect to the server
            TcpClient client = new TcpClient("1.1.1.1", 1234);
            NetworkStream stream = client.GetStream();

            // Create a new Stream object
            ProtoIP.Stream _stream = new Stream(stream);

            // Transmit a string over the network
            _stream.Transmit("Hello World!");
      }
}