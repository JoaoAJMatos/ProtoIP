using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using ProtoIP;

class ComplexClient : ProtoClient
{
      // We can override the OnSend() method to describe our Client logic
      // In this example, we are printing out a message when the server responds
      public override void OnReceive() 
      {
            byte[] data = _protoStream.GetDataAs<byte[]>();
            Packet receivedPacket = Packet.Deserialize(data);

            // We can check the type of the packet and act accordingly
            if (receivedPacket._GetType() == (int)Packet.Type.PONG)
            {
                  Console.WriteLine("CLIENT: Received PONG packet!");
            }
      }
}

class ComplexServer : ProtoServer
{
      // We can override the OnRequest() method to describe our Server logic
      // Once the server receives a request, it will call this method
      public override void OnRequest(int userID)
      {
            byte[] data = _clients[userID].GetDataAs<byte[]>();
            Packet receivedPacket = Packet.Deserialize(data);

            // We can check the type of the packet and act accordingly
            if (receivedPacket._GetType() == (int)Packet.Type.PING)
            {
                  Console.WriteLine("SERVER: Received PING packet, sending PONG!");
                  Packet packet = new Packet((int)Packet.Type.PONG);
                  Send(Packet.Serialize(packet), userID);
            }
      }
}

class Program
{
      const int PORT = 1234;

      static void Main()
      {
            // Create the server and start it
            ComplexServer server = new ComplexServer();
            Thread serverThread = new Thread(() => server.Start(PORT));
            serverThread.Start();

            // Create a new ComplexClient object and connect to the server
            ComplexClient client = new ComplexClient();
            client.Connect("127.0.0.1", PORT);

            // Serialize the packet and send it
            Packet packet = new Packet((int)Packet.Type.PING);
            client.Send(Packet.Serialize(packet));

            // Receive the response from the server
            client.Receive();

            // Disconnect from the server
            client.Disconnect();

            // Stop the server
            server.Stop();
      }
}