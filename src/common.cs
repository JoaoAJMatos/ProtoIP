namespace ProtoIP
{
      namespace Common 
      {
            // Error definitions
            class Error {
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

            class Network {
                  public const int DEFAULT_BUFFER_SIZE = 1024;
                  public const int MAX_TRIES = 3;
                  public const int MAX_PACKETS = 1024;

                  // Network connection object
                  public struct Connection {
                        TcpClient client;
                        NetworkStream stream;
                  }

                  // Connect to a host and return a connection object
                  public Connection Connect(string host, int port) {
                        Connection connection = new Connection();

                        connection.client = new TcpClient(host, port);
                        connection.stream = connection.client.GetStream();

                        return connection;
                  }

                  // Disconnect from a host
                  public void Disconnect(Connection connection) {
                        connection.stream.Close();
                        connection.client.Close();
                  }
            }
      }
}