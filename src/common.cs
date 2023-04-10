// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System.Net.Sockets;
using System.Net;

namespace ProtoIP
{
      namespace Common 
      {
            // Error definitions
            public class Error {
                  public const string INVALID_PACKET = "Invalid packet";
                  public const string INVALID_PACKET_TYPE = "Invalid packet type";
                  public const string INVALID_PACKET_ID = "Invalid packet id";
                  public const string INVALID_PACKET_DATA_SIZE = "Invalid packet data size";
                  public const string INVALID_PACKET_DATA = "Invalid packet data";
                  public const string INVALID_PACKET_HEADER = "Invalid packet header";
                  public const string INVALID_PACKET_HEADER_SIZE = "Invalid packet header size";
                  public const string INVALID_PACKET_BUFFER = "Invalid packet buffer";
                  public const string INVALID_PACKET_BUFFER_SIZE = "Invalid packet buffer size";

                  public enum Code {
                        OK,
                        ERROR
                  }
            }

            public class Network {
                  public const int DEFAULT_BUFFER_SIZE = 1024;
                  public const int MAX_TRIES = 3;
                  public const int MAX_PACKETS = 1024;

                  // Network connection object
                  public struct Connection {
                        public TcpClient client;
                        public NetworkStream stream;
                  }

                  // Connect to a host and return a connection object
                  public static Connection Connect(string host, int port) {
                        Connection connection = new Connection();

                        connection.client = new TcpClient(host, port);
                        connection.stream = connection.client.GetStream();

                        return connection;
                  }

                  // Disconnect from a host
                  public static void Disconnect(Connection connection) {
                        connection.stream.Close();
                        connection.client.Close();
                  }

                  // Returns a random unused TCP port
                  public static int GetRandomUnusedPort()
                  {
                        var listener = new TcpListener(IPAddress.Loopback, 0);
                        listener.Start();

                        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
                        listener.Stop();

                        return port;
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
