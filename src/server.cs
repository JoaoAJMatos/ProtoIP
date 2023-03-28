using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System;

using ProtoIP;
using ProtoIP.Common;

namespace ProtoIP
{
      public class ProtoServer {
            private List<ProtoStream> _clients = new List<ProtoStream>();
            private TcpListener _listener;

            public ProtoServer() {}

            // Creates a TCP listener and starts listening for connections
            private void StartListening(int port) {
                  _listener = new TcpListener(port);
                  _listener.Start();
            }

            // Accepts new connections and adds them to the clients List
            // Calls the OnUserConnect() method in a separate thread on every connect event
            private void AcceptConnections() {
                  TcpClient client = _listener.AcceptTcpClient();
                  NetworkStream stream = client.GetStream();
                  ProtoStream protoStream = new ProtoStream(stream);

                  _clients.Add(protoStream);

                  Thread thread = new Thread(() => OnUserConnect(_clients.Count - 1));
                  thread.Start();
            }

            // Send data to the client and call the OnResponse() method
            public void Send(byte[] data, int userID) {
                  _clients[userID].Transmit(data);
                  OnResponse(userID);
            }

            // Receive data from the client and call the OnRequest() method
            public void Receive(int userID) {
                  _clients[userID].Receive();
                  OnRequest(userID);
            }

            // Starts the main server loop
            public void Start(int port) {
                  StartListening(port);
                  
                  while (true) {
                        AcceptConnections();
                  }
            }

            // Virtual functions
            public virtual void OnUserConnect(int userID) { Receive(userID); }
            public virtual void OnResponse(int userID) {}
            public virtual void OnRequest(int userID) {}
      }
}
