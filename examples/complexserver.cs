using System;
using System.Net.Sockets;
using System.Text;

// Include the ProtoIP namespace
using ProtoIP;

class ComplexServer : ProtoServer
{
      // Once the user makes a request, we can build the packet from the protoStream
      // and respond accordingly
      public override void OnRequest(int userID)
      {
            // Get the data from the ProtoStream and deserialize the packet
            byte[] data = _protoStreamArrayClients[userID].GetDataAs<byte[]>();
            Packet receivedPacket = Packet.Deserialize(data);

            // Respond to PING packets
            if (receivedPacket._GetType() == Packet.Type.PING)
            {
                  Packet packet = new Packet(Packet.Type.PONG);
                  Send(packet.Serialize(), userID);
            }
      }
}

class Program 
{
      const int PORT = 1234;

      static void Main()
      {
            // Create the server and start it
            ComplexServer server = new ComplexServer(PORT);
            server.Start();
      }
}