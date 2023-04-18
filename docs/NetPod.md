# NetPod

**NetPods** allow you to create an environment for **sending and receiving raw packets over a socket**. Enabling you to create and manipulate various types of packets from the different network layers, giving you **low level access** to the actual underlying protocol definitions.

**NetPods** are ideal for **learning how the various network layers work**, from Ethernet frames to IP packets and TCP segments. Allowing you to **visualize the content of the packets** and how they are structured.

> You can think of **NetPods** as a network packet laboratory, where you can create your own packets and experiment with them.

## Implementation

NetPods were inspired by [Scapy](https://scapy.net/), a tool for packet manipulation written in Python.

Similar to Scapy, NetPods make it easy to create and manipulate packets from different network layers in a simple and easy to use interface.

By layering different network abstraction layers on top of each other, you are able to forge your own packets from scratch, or manipulate existing packets to your liking.

## Methods

### Constructors

- `NetPod()` - Creates a new NetPod object.

### Public functions

> Network layer manipulation

You can add and remove network layers from the NetPod using the following methods:

- `void AddLayer(NetworkLayer layer)` - Adds a network layer to the NetPod.

- `void RemoveLayer(NetworkLayer layer)` - Removes a network layer from the NetPod.

> Packet transmission

To send and receive packets over the socket, you can use the following methods:

- `void SendPacket(Packet packet)` - Sends a packet over the socket.

- `Packet ReceivePacket()` - Receives a packet from the socket.

> Visualization

In order to visualize the structure of the NetPod, you can use the following methods:

- `static void ShowStructure(NetPod netPod)` - Shows the structure of the NetPod.

## Usage

### Creating Network Layers

ProtoIP offers a variety of default network layer implementations that you can use to create your own NetPods. Like `Ethernet`, `IP`, `TCP`, `UDP` and `ICMP`.

You can checkout the documentation for each of these implementations here:

- [Ethernet](./Ethernet.md)
- [IP](./IP.md)
- [TCP](./TCP.md)
- [UDP](./UDP.md)
- [ICMP](./ICMP.md)

You can also create your own network layer implementations by inheriting from the `NetworkLayer` class.

### Creating NetPods

After creating your network layers, you can instantiate a new `NetPod` object and add your network layers to it in order to create a structure like so: `Ethernet >> IP >> TCP >> ... Your Layer`.

```csharp
using ProtoIP;

class Program 
{
      static void Main() 
      {
            // Create the network layers
            Ethernet ethernet = new Ethernet(SourceMAC: "00:00:00:00:00:00", DestinationMAC: "ff:ff:ff:ff:ff:ff");
            IP ip = new IP(sourceIP: "127.0.0.1", destinationIP: "127.0.0.1");
            TCP tcp = new TCP(sourcePort: 80, destinationPort: 80)

            // Create a new NetPod
            NetPod pod = new NetPod();

            // Add the layers to the NetPod 
            // using the composition operator
            pod = ethernet / ip / tcp;

            // Show the NetPod structure
            NetPod.ShowStructure(pod);
      }
}
```
