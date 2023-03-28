using System.Text;
using System.Threading;
using System.Net.Sockets;
using System;

using ProtoIP;
using ProtoIP.Common;

namespace ProtoIP
{
      public class Server {
            private ProtoIP.ProtoStream[] _protoStreamArrayClients;
            private int _serverPort;
            private TcpListener _listener;

            public Server(int port) {
                  this._serverPort = port;
            }

            private void StartListening() {
                  // Create a new TcpListener
                  _listener = new TcpListener(_serverPort);
                  _listener.Start();
            }

            private void AcceptConnections() {
                  // Accept a new connection
                  TcpClient client = _listener.AcceptTcpClient();
                  NetworkStream stream = client.GetStream();

                  // Create a new ProtoStream object
                  ProtoStream protoStream = new ProtoStream(stream);

                  // Add the new ProtoStream to the array
                  Array.Resize(ref _protoStreamArrayClients, _protoStreamArrayClients.Length + 1);
                  _protoStreamArrayClients[_protoStreamArrayClients.Length - 1] = protoStream;

                  // Start a new thread to handle the connection
                  Thread thread = new Thread(new ThreadStart(OnUserConnect));
            }

            public void Start() {
                  StartListening();
                  
                  while (true) {
                        AcceptConnections();
                  }
            }

            // Virtual functions
            public virtual void OnUserConnect() {}
            public virtual void OnUserDisconnect() {}
            public virtual void OnResponse() {}
            public virtual void OnRequest() {}
      }
}
