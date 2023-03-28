using System.Text;
using System.Threading;
using System;

using stream;

using ProtoIP;
using ProtoIP.Common;

namespace ProtoIP
{
      class Server {
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
                  TcpClient client = listener.AcceptTcpClient();
                  NetworkStream stream = client.GetStream();

                  // Create a new ProtoStream object
                  _protoStream = new Stream(stream);

                  // Add the new ProtoStream to the array
                  _protoStreamArray.Append(_protoStream);

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
