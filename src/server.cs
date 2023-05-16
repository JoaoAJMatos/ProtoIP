// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System;

using ProtoIP;
using ProtoIP.Common;

namespace ProtoIP
{
      public class ProtoServer
      {
            protected List<ProtoStream> _clients = new List<ProtoStream>();
            protected TcpListener _listener;
            protected bool _isRunning = false;
            protected string _LastError = "";

            public ProtoServer() { }

            // Send data to the client and call the OnResponse() method
            public void Send(byte[] data, int userID)
            {
                  _clients[userID].Transmit(data);
                  OnResponse(userID);
            }

            // Receive data from the client and call the OnRequest() method
            public void Receive(int userID)
            {
                  _clients[userID].Receive();
                  OnRequest(userID);
            }

            // Starts the main server loop
            public void Start(int port)
            {
                  StartListening(port);
                  _isRunning = true;

                  while (_isRunning)
                  {
                        AcceptConnections();
                  }
            }

            // Stops the main server loop
            public void Stop()
            {
                  _listener.Stop();
                  _isRunning = false;
            }

            // Assembles a packet from the received data and returns it.
            public Packet AssembleReceivedDataIntoPacket(int userID)
            {
                  byte[] data = _clients[userID].GetDataAs<byte[]>();
                  Packet receivedPacket = Packet.Deserialize(data);
                  return receivedPacket; 
            }

            // Virtual functions
            public virtual void OnUserConnect(int userID) { Receive(userID); }
            public virtual void OnResponse(int userID) { }
            public virtual void OnRequest(int userID) { }

            // Creates a TCP listener and starts listening for connections
            private void StartListening(int port)
            {
                  _listener = new TcpListener(port);
                  _listener.Start();
            }

            // Accepts new connections and adds them to the clients List
            // Calls the OnUserConnect() method in a separate thread on every connect event
            private void AcceptConnections()
            {
                  try
                  {
                        TcpClient client = _listener.AcceptTcpClient();
                        NetworkStream stream = client.GetStream();
                        ProtoStream protoStream = new ProtoStream(stream);

                        _clients.Add(protoStream);

                        Thread thread = new Thread(() => OnUserConnect(_clients.Count - 1));
                        thread.Start();
                  }
                  catch (Exception e)
                  {
                        _LastError = e.Message;
                  }
            }
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
