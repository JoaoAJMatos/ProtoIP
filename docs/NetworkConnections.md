# Network Conections

A **Network Connection**, or simply **Connection**, is a structure declared inside the `ProtoIP.Common` namespace which stores the **NetworkStream** information between two peers.

It is used by the `ProtoClient` class to store the connection information to the server.

## Members

- `client` - [TcpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient?view=netframework-4.8)
- `stream` - [NetworkStream](https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.networkstream?view=netframework-4.8)

### Structure

```csharp
struct Connection 
{
      TcpClient client;
      NetworkStream stream;
}
```

## Methods

- `Connect(string host, int port)` - Connects to a remote host and returns a **Connection** instance.
- `Disconnect(Connection connection)` - Disconnects from a remote host.
