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

            public void Stop()
            {
                  _listener.Stop();
                  _isRunning = false;
            }

            // Virtual functions
            public virtual void OnUserConnect(int userID) { Receive(userID); }
            public virtual void OnResponse(int userID) { }
            public virtual void OnRequest(int userID) { }
      }
}
