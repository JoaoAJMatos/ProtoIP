# ProtoClient

The **ProtoClient** class serves as a **wrapper for the TcpClient class**. It provides a simple interface for sending and receiving data using **ProtoStreams** and other **ProtoIP** Interfaces.

It can be used to implement more **complex Client definitions** to be used in your own protocols by overriding virtual methods and defining **Client Events**.

## Members

- `_serverConnection` - [Network.Connection](NetWorkConnections.md)
- `_protoStream` - [ProtoStream](ProtoStream.md)

## Methods

### Constructors

- `ProtoClient()`

### Public functions

> Connection

- `Connect(string ip, int port)`
- `Disconnect()`

> Sending data

To send data over the network to a remote host using **ProtoClients**, you can use the following variations of the `Send()` method:

- `Send(byte[] data)`
- `Send(string data)`

Under the hood, the `Send()` method will call the `Transmit()` method of the **ProtoStream** object.

After sending the data, the virtual method `OnSend()` will be called.

> Receiving data

To receive data from a remote host, use ProtoClient's `Receive()` function:

- `Receive()`

After receiving the data, the virtual method `OnReceive()` will be called. **This is where the data should be processed**.

### Virtual functions

> Client events

You can define your own **client events** by overriding the following virtual methods:

- `OnConnect()`
- `OnDisconnect()`
- `OnReceive()`
- `OnSend()`

You can use these methods to implement your own **client logic**, like writing the received data to a file or to a database.

## Example

```csharp
using ProtoIP;

class ComplexClient : ProtoClient 
{
      public override void OnReceive() 
      {
            string data = _protoStream.GetDataAs<string>();
            Console.WriteLine(data);
      }
}

class Program 
{
      static void Main() 
      {
            // Create a new ComplexClient object
            ComplexClient client = new ComplexClient();

            // Connect to the server
            client.Connect("1.1.1.1", 1234);

            // Send a string to the server
            client.Send("Hello World!");

            // Receive the response
            // The OnReceive() method will be called
            client.Receive();

            // Disconnect from the server
            client.Disconnect();
      }
}
```
