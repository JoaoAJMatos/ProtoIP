using System.Text;
using System;

using stream;

using ProtoIP;
using ProtoIP.Common;

namespace ProtoIP
{
      class Client
      {
            private Common.Network.Connection _serverConnection;
            private ProtoIP.ProtoStream _protoStream;

            public Client()
            {
                  _serverConnection = null;
                  _protoStream = null;
            }

            // Connect to the remote host and create a new ProtoStream object.
            //
            // Call the OnConnect() method if the connection was successful, 
            // otherwise call OnConnectFailed().
            public void Connect(string serverIP, int serverPort)
            {
                  _serverConnection = Common.Network.Connect(serverIP, serverPort);
                  _protoStream = new Stream(_serverConnection.stream);

                  if (_serverConnection != null) { OnConnect(); }
            }

            // Disconnect from the remote host and destroy the ProtoStream object.
            //
            // Call the OnDisconnect() method.
            public void Disconnect()
            {
                  Common.Network.Disconnect(_serverConnection);
                  _serverConnection = null;

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

            // Receive data from a remote host using a ProtoStream.
            //
            // Call the OnReceive() method.
            public void Receive()
            {
                  _protoStream.Receive();
                  OnReceive();
            }

            // Virtual functions
            private virtual void OnConnect() { }
            private virtual void OnDisconnect() { }
            private virtual void OnSend() { }
            private virtual void OnReceive() { }
      }
}