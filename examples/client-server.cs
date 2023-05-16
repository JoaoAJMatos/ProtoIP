// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

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
            Packet receivedPacket = AssembleReceivedDataIntoPacket();

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
            Packet receivedPacket = AssembleReceivedDataIntoPacket(userID);

            // We can check the type of the packet and act accordingly
            if (receivedPacket._GetType() == (int)Packet.Type.PING)
            {
                  Console.WriteLine("SERVER: Received PING packet, sending PONG!");
                  Packet packet = new Packet(Packet.Type.PONG);
                  Send(Packet.Serialize(packet), userID);
            }
      }
}

class Program
{
      static void Main()
      {
            int PORT = ProtoIP.Common.Network.GetRandomUnusedPort();

            // Create the server and start it
            ComplexServer server = new ComplexServer();
            Thread serverThread = new Thread(() => server.Start(PORT));
            serverThread.Start();

            // Create a new ComplexClient object and connect to the server
            ComplexClient client = new ComplexClient();
            client.Connect("127.0.0.1", PORT);

            // Serialize the packet and send it
            Packet packet = new Packet(Packet.Type.PING);
            client.Send(Packet.Serialize(packet));

            // Receive the response from the server
            client.Receive();

            // Disconnect from the server
            client.Disconnect();

            // Stop the server
            server.Stop();
      }
}

// MIT License
// 
// Copyright (c) 2023 João Matos
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
