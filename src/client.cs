// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System.Text;
using System;

using ProtoIP;
using ProtoIP.Common;

namespace ProtoIP
{
      public class ProtoClient
      {
            protected Common.Network.Connection _serverConnection;
            protected ProtoStream _protoStream;

            public ProtoClient() {}

            // Connect to the remote host and create a new ProtoStream object.
            //
            // Call the OnConnect() method if the connection was successful, 
            // otherwise call OnConnectFailed().
            public void Connect(string serverIP, int serverPort)
            {
                  _serverConnection = Common.Network.Connect(serverIP, serverPort);
                  _protoStream = new ProtoStream(_serverConnection.stream);

                  OnConnect();
            }

            // Disconnect from the remote host and destroy the ProtoStream object.
            //
            // Call the OnDisconnect() method.
            public void Disconnect()
            {
                  Common.Network.Disconnect(_serverConnection);

                  OnDisconnect();
            }

            // Send data to a remote host using a ProtoStream.
            //
            // Call the OnSend() method.
            public void Send(byte[] data)
            {
                  _protoStream.Transmit(data);
                  OnSend();
            }

            // Send overload for sending string data
            public void Send(string data)
            {
                  _protoStream.Transmit(data);
                  OnSend();
            }

            // Receive data from a remote host using a ProtoStream.
            //
            // Call the OnReceive() method.
            public void Receive(bool shouldCallOnReceive)
            {
                  _protoStream.Receive();
                  if (shouldCallOnReceive)
                        OnReceive();
            }

            // Assembles a packet from the recived data and returns the packet.
            public Packet AssembleReceivedDataIntoPacket()
            {
                  byte[] data = _protoStream.GetDataAs<byte[]>();
                  Packet assembledPacket = Packet.Deserialize(data);
                  return assembledPacket;
            }

            // Virtual functions
            public virtual void OnConnect() { }
            public virtual void OnDisconnect() { }
            public virtual void OnSend() { }
            public virtual void OnReceive() { }
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
