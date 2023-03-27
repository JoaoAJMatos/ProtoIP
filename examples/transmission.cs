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
            ProtoIP.ProtoStream _protoStream = new Stream(stream);

            // Transmit a string over the network
            _protoStream.Transmit("Hello World!");
      }
}