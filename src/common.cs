namespace ProtoIP
{
      namespace Common 
      {
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
            }

            class Packet {
                  public enum Type {
                        ACK,
                        HANDSHAKE_REQ,
                        HANDSHAKE_RES,
                        PUBLIC_KEY,
                        BYTES,
                        REPEAT,
                        END_OF_TRANSMISSION,
                        START_OF_TRANSMISSION,
                        FILE_TRANSMISSION_START,
                        FILE_TRANSMISSION_END,
                        FILE_NAME,
                        FILE_SIZE,
                        PING,
                        PONG,
                        CHECKSUM,
                        RELAY,
                        BROADCAST
                  }

                  public const int HEADER_SIZE = 12;
            }
      }
}