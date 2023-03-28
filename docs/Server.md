# ProtoServer

Similarly to [**ProtoClients**](Client.md), the **ProtoServer** class provides a simple interface for implementing **server logic** using **Server Events**.

Users can inherit this class to implement their own **server definitions** to be used in their own protocols by overriding virtual methods.

## Members

- `_clients` - [ProtoStream](ProtoStream.md) List
- `_listener` - [TcpListener](https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=net-5.0)

## Methods

### Constructors

- `ProtoServer()`

### Public functions

> Starting the server

To start listening and accepting connections from clients, use the `Start()` method:

- `Start(int port)` - Starts the main server loop.

Once a client connects, the virtual method `OnClientConnect()` will be executed in a separate thread (see [server events](#virtual-functions)).

> Sending and Receiving data

- `Send(byte[] data, int userID)` - Sends data to a specific client.
- `Receive(int userID)` - Receives data from a specific client.

### Virtual functions

> Server events

You can define your own **server logic** by implementing the following virtual methods:

- `OnClientConnect(int userID)` - Called when a client connects to the server.
- `OnRequest(int userID)` - Called when the client makes a request.
- `OnResponse(int userID)` - Called when the server responds to a request.

## Example

```csharp
using ProtoIP;

class ComplexServer : ProtoServer
{
      // Once the user makes a request, we can build the packet from the protoStream
      // and respond accordingly
      public override void OnRequest(int userID)
      {
            // Get the data from the ProtoStream and deserialize the packet
            byte[] data = _protoStreamArrayClients[userID].GetDataAs<byte[]>();
            Packet receivedPacket = Packet.Deserialize(data);

            // Respond to PING packets
            if (receivedPacket._GetType() == Packet.Type.PING)
            {
                  Packet packet = new Packet(Packet.Type.PONG);
                  Send(packet.Serialize(), userID);
            }
      }
}

class Program 
{
      const int PORT = 1234;

      static void Main()
      {
            // Create the server and start it
            ComplexServer server = new ComplexServer(PORT);
            server.Start();
      }
}
```