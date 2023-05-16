# Client-Server Application - ProtoIP

You can use the [ProtoClient](Client.md) and [ProtoServer](Server.md) classes to implement your own protocols and applications.

In this example, we are going to create a basic **client-server** application. Our application will consist of a **PING**/**PONG** server, where the job of the server is to respond to incoming ping packets from the clients.

## Server

To do this, we can start of by defining our own server class derived from the [ProtoServer](Server.md) class. Let's call it **PingPongServer**:

```csharp
class PingPongServer : ProtoServer
{

}
```

Next, we can define **our own server logic** by overriding ProtoServer's virtual functions. These functions are called when specific events are triggered on the server. For example, **when a client connects**, or **when the server receives a request**. You can check out more on that [here](Server.md).

For now, we can override the `OnRequest` method, and check if the incoming packet is a **ping** request.

```csharp
class PingPongServer : ProtoServer
{
      public override void OnRequest(int userID)
      {
            Packet receivedPacket = AssembleReceivedDataIntoPacket(userID);

            if (receivedPacket._GetType() == (int)Packet.Type.PING)
            {
                  Console.WriteLine("SERVER: Received PING packet, sending PONG!");
                  Packet packet = new Packet((int)Packet.Type.PONG);
                  Send(Packet.Serialize(packet), userID);
            }
      }
}
```

As you can see, we are using the `userID` to get the data of a specific user as a **byte array**, and then using that data to assemble a [packet](Packet.md). We then check if the packet we received is a **ping** request. If so, we send back a **pong** packet.

This is our server logic done. Simple right? I know! 

## Client

Now let's implement our client logic!

Just like before, let's create our own **client implementation** derived from the [ProtoClient](Client.md) class:

```csharp
class PingPongClient : ProtoClient
{

}
```

Similarly to [ProtoServers](Server.md), [ProtoClients](Client.md) also let you define **client logic** by overriding virtual functions. In our case, we can override the `OnResponse` method, which is triggered every time we get a response back from the server.

We can check the received packet to see if it is a **ping** response, and if so, we can show a message on the screen. Like so:

```csharp
class PingPongClient : ProtoClient
{
      public override void OnReceive() 
      {
            Packet receivedPacket = AssembleReceivedDataIntoPacket();

            if (receivedPacket._GetType() == (int)Packet.Type.PONG)
            {
                  Console.WriteLine("CLIENT: Received PONG packet!");
            }
      }
}
```

**Great!** We have successfully implemented both the **client** and **server** logic. Now let's implement the main program functionality!

## Main

In our main class, we can start off by creating a new **PingPongServer** instance, and starting it on a separate thread. Like so:

```csharp
class ClientServerExample
{
      const int PORT = 1234;

      static void Main()
      {
            PingPongServer server = new PingPongServer();
            Thread serverThread = new Thread(() => server.Start(PORT));
            serverThread.Start();
      }
}
```

Then, we can create a new **client** instance and connect to the server:

```csharp
class ClientServerExample
{
      const int PORT = 1234;

      static void Main()
      {
            PingPongServer server = new PingPongServer();
            Thread serverThread = new Thread(() => server.Start(PORT));
            serverThread.Start();

            PingPongClient client = new PingPongClient();
            client.Connect("127.0.0.1", PORT);
      }
}
```

And finally, we can create a new **ping** packet, send it to the server, and wait for a response. In the end, we can disconnect from the server and shut it down:

```csharp
class ClientServerExample
{
      const int PORT = 1234;

      static void Main()
      {
            PingPongServer server = new PingPongServer();
            Thread serverThread = new Thread(() => server.Start(PORT));
            serverThread.Start();

            PingPongClient client = new PingPongClient();
            client.Connect("127.0.0.1", PORT);

            Packet pingPacket = new Packet(Packet.Type.PING);
            client.Send(Packet.Serialize(pingPacket));

            client.Receive();
            client.Disconnect();

            server.Stop();
      }
}
```

**And that's it!** ProtoIP let's you implement complex programs without having to worry about implementing all the infrastructure behind secure and robust network communications.

## Full code

Here is the full code for this guide:

```csharp
// Server
class PingPongServer : ProtoServer
{
      public override void OnRequest(int userID)
      {
            Packet receivedPacket = AssembleReceivedDataIntoPacket(userID);

            if (receivedPacket._GetType() == (int)Packet.Type.PING)
            {
                  Console.WriteLine("SERVER: Received PING packet, sending PONG!");
                  Packet packet = new Packet((int)Packet.Type.PONG);
                  Send(Packet.Serialize(packet), userID);
            }
      }
}

// Client
class PingPongClient : ProtoClient
{
      public override void OnReceive() 
      {
            Packet receivedPacket = AssembleReceivedDataIntoPacket();

            if (receivedPacket._GetType() == (int)Packet.Type.PONG)
            {
                  Console.WriteLine("CLIENT: Received PONG packet!");
            }
      }
}

// Main
class ClientServerExample
{
      const int PORT = 1234;

      static void Main()
      {
            PingPongServer server = new PingPongServer();
            Thread serverThread = new Thread(() => server.Start(PORT));
            serverThread.Start();

            PingPongClient client = new PingPongClient();
            client.Connect("127.0.0.1", PORT);

            Packet pingPacket = new Packet(Packet.Type.PING);
            client.Send(Packet.Serialize(pingPacket));

            client.Receive();
            client.Disconnect();

            server.Stop();
      }
}
```