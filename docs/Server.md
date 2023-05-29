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

> Starting and stopping the server

To start listening and accepting connections from clients, use the `Start()` method:

- `void Start(int port)` - Starts the main server loop.
- `void Stop()` - Stops the server.

> Sending and Receiving data

- `void Send(byte[] data, int userID)` - Sends data to a specific client.
- `void SendBroadcast(byte[] data)` - Broadcasts data to all of the clients.
- `void Receive(int userID)` - Receives data from a specific client.
- `Packet AssembleReceivedDataIntoPacket(int userID)` - Assembles the received data into a packet.

### Virtual functions

> Server events

You can define your own **server logic** by implementing the following virtual methods:

- `virtual void OnUserConnect(int userID)` - Called when a client connects to the server.
- `virtual void OnUserDisconnect(int userID)` - Called when a client disconnects from the server.
- `virtual void OnRequest(int userID)` - Called when the client makes a request.
- `virtual void OnResponse(int userID)` - Called when the server responds to a request.

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
            byte[] data = _clients[userID].GetDataAs<byte[]>();
            Packet receivedPacket = Packet.Deserialize(data);

            // Respond to PING packets
            if (receivedPacket._GetType() == (int)Packet.Type.PING)
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
