# NetPod

**NetPods** allow you to create an environment for **sending and receiving raw packets over a socket**. Enabling you to create and manipulate various types of packets from the different network layers, giving you **low level access** to the actual underlying protocol definitions.

**NetPods** are ideal for **learning how the various network layers work**, from Ethernet frames to IP packets and TCP segments. Allowing you to **visualize the content of the packets** and how they are structured.

> You can think of **NetPods** as a network packet laboratory, where you can create your own packets and experiment with them.

Additionaly, you can take advantage of the freedom that **NetPods** offer in terms of low level access to packets in order to create your own **Cyber-Security tools**. Such as **packet sniffers**, **packet injectors**, **arp spoofers**, and many more. Checkout [this]() example on how to implement a **Man-In-The-Middle attack** using **NetPods**.

## Members

The member variables in the NetPod class store the relevant information for each network layer, containing an instance to an object of the corresponding network layer class.

- `Ethernet ethernet` - The Ethernet layer.
- `ARP arp` - The ARP layer.
- `IP ip` - The IP layer.
- `TCP tcp` - The TCP layer.
- `UDP udp` - The UDP layer.
- `ICMP icmp` - The ICMP layer.

### Creating Network Layers

ProtoIP offers a variety of default network layer implementations that you can use to create your own NetPods. Like `Ethernet`, `ARP`, `IP`, `TCP`, `UDP` and `ICMP`.

You can checkout the documentation for each of these implementations here:

- [Ethernet](./Ethernet.md)
- [ARP](./ARP.md)
- [IP](./IP.md)
- [TCP](./TCP.md)
- [UDP](./UDP.md)
- [ICMP](./ICMP.md)

## Methods

### Constructors

- `NetPod()` - Creates a new NetPod object.
- `NetPod(Ethernet ethLayer)` - Creates a new NetPod object with an Ethernet layer and all the subsequent encapsulated layers inside it.

### Public functions

> Packet transmission

To send packets over the raw socket, you can use the following method:

- `static void Send(NetPod pod)` - Assembles the NetPod and sends the bytes over the socket.

> Packet sniffing

Packet sniffing is a technique whereby packet data flowing across the network is detected and observed, just like in [Wireshark](https://www.wireshark.org/).

To **sniff** packets on a given network interface you can use the following method:

- `static void Sniff(string interface, Action<NetPod> callback)` - Listens for packets on a given network interface and calls the callback function with the received NetPod. If no interface is passed, the default interface will be used.

> Visualization

In order to visualize the structure of the NetPod, you can use the following methods:

- `static void ShowStructure(NetPod netPod)` - Shows the structure of the NetPod.

### Example

After creating your network layers, you can instantiate a new `NetPod` object and add your network layers to it in order to create a structure like so: `Ethernet >> IP >> TCP >> ... Your Layer`.

```csharp
using ProtoIP;

class Program 
{
      static void Main() 
      {
            // Create the network layers
            Ethernet ethernet = new Ethernet(sourceMAC: "00:00:00:00:00:00", destinationMAC: "ff:ff:ff:ff:ff:ff");
            IP ip = new IP(sourceIP: "127.0.0.1", destinationIP: "127.0.0.1");
            TCP tcp = new TCP(sourcePort: 80, destinationPort: 80);

            // Create a new NetPod and layer the 
            // network abstractions using the 
            // composition operator ("/")
            NetPod pod = new NetPod(ethernet / ip / tcp);

            // Define a callback function to be triggered 
            // when a packet is received
            Action<NetPod> action = (NetPod receivedPod) => {
                  NetPod.ShowStructure(receivedPod);
            };

            // Sniff packets on the given interface
            Thread receiveThread = new Thread(() => { NetPod.Sniff("lo0", action); });
            receiveThread.Start();

            // Send the NetPod over the socket
            NetPod.Send(pod);
      }
}
```
