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

            public void Send(string data)
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
            public virtual void OnConnect() { }
            public virtual void OnDisconnect() { }
            public virtual void OnSend() { }
            public virtual void OnReceive() { }
      }
}