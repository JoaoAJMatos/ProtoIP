// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;
using System.Net;
using System.Net.Sockets;

namespace ProtoIP
{
      // NetPods implement an abstraction for sending and receiving data over a raw socket.
      // This allows you to create and manipulate raw packets from all kinds of network layers.
      public class NetPod
      {
            private Ethernet _ethernet;
            private ARP _arp;
            private IP _ip;
            private UDP _udp;
            private TCP _tcp;
            private ICMP _icmp;

            // Creates a new NetPod instance.
            public NetPod()
            {
                  _ethernet = new Ethernet();
                  _arp = new ARP();
                  _ip = new IP();
                  _udp = new UDP();
                  _tcp = new TCP();
                  _icmp = new ICMP();
            }
      
            // Creates a new netpod object with an Ethernet layer and
            // all the subsequent layers encapsulated inside it.
            public NetPod(Ethernet ethernet)
            {
                  if (ethernet._type == Ethernet.ETH_TYPE_IP) { ipDeserialization(ethernet, this); }
                  else if (ethernet._type == Ethernet.ETH_TYPE_ARP) { arpDeserialization(ethernet, this); }
                  else { throw new Exception("Unknown ethernet type."); } 
            }

            // Sends a NetPod over a raw socket.
            public static void Send(NetPod pod)
            {
                  byte[] data = Assemble(pod);
                  Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
                  IPEndPoint ipEndPoint = new IPEndPoint(pod._ip._destinationAddress, 0); 
                  socket.SendTo(data, ipEndPoint);
            }

            // Listens for every incomming packet on a Network Interface
            // and calls the callback function with the NetPod as a parameter.
            public static void Sniff(string networkInterface, Action<NetPod> callback)
            {
                  Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
                  socket.Bind(new IPEndPoint(IPAddress.Any, 0));
                  socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
                  byte[] data = new byte[4096];
                  EndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                  while (true)
                  {
                        socket.ReceiveFrom(data, ref ipEndPoint);
                        NetPod pod = NetPod.Disassemble(data);
                        NetPod.ShowStructure(pod);
                        callback(pod);
                  }
            }

            // Deserializes an Ethernet frame and it's subsequent layers and assigns
            // them to a netpod instance.
            private static void ipDeserialization(Ethernet ethernet, NetPod pod)
            {
                  var deserializedIPPacket = IP.Deserialize(ethernet._payload);
                  byte[] ipPayload = deserializedIPPacket._payload;

                  if (deserializedIPPacket == null) { return; }

                  pod._ethernet = ethernet;
                  pod._ip = deserializedIPPacket;

                  switch (deserializedIPPacket._protocol)
                  {
                        case (byte)IP.IPProtocolPacketType.TCP:
                              var deserializedTCPPacket = TCP.Deserialize(ipPayload);
                              if (deserializedTCPPacket != null) { pod._tcp = deserializedTCPPacket; }
                              break;
                        case (byte)IP.IPProtocolPacketType.UDP:
                              var deserializedUDPPacket = UDP.Deserialize(ipPayload);
                              if (deserializedUDPPacket != null) { pod._udp = deserializedUDPPacket; }
                              break;
                        case (byte)IP.IPProtocolPacketType.ICMP:
                              var deserializedICMPPacket = ICMP.Deserialize(ipPayload);
                              if (deserializedICMPPacket != null) { pod._icmp = deserializedICMPPacket; }
                              break;
                  }
            }

            // Deserializes an Ethernet frame and the ARP packet inside it
            // and assigns the objects to a netpod instance.
            private static void arpDeserialization(Ethernet ethernet, NetPod pod)
            {
                  var deserializedARPPacket = ARP.Deserialize(ethernet._payload);
                  if (deserializedARPPacket != null) { return; }

                  pod._ethernet = ethernet;
                  pod._arp = deserializedARPPacket;
            }

            // Assembles the NetPod into a byte array.
            // This byte array can be sent over a raw socket.
            private static byte[] Assemble(NetPod pod)
            {
                  byte[] data = pod._ethernet.Serialize();
                  return data;
            }

            // Disassembles the NetPod from a byte array.
            // This byte array can be received from a raw socket.
            private static NetPod Disassemble(byte[] data)
            {
                  Ethernet ethernet = Ethernet.Deserialize(data);
                  return new NetPod(ethernet);
            }
 
            // Shows the netpod structure in a human readable format.
            public static void ShowStructure(NetPod pod)
            {
                  string header = "### [NetPod Structure] ###\n";
                  string etherLayer = pod._ethernet.ToString();
                  string ipLayer = pod._ip.ToString();
                  string lastLayer = "";

                  switch (pod._ip._protocol)
                  {
                        case (byte)IP.IPProtocolPacketType.TCP:
                              lastLayer = pod._tcp.ToString();
                              break;
                        case (byte)IP.IPProtocolPacketType.UDP:
                              lastLayer = pod._udp.ToString();
                              break;
                        case (byte)IP.IPProtocolPacketType.ICMP:
                              lastLayer = pod._icmp.ToString();
                              break;
                  }

                  Console.WriteLine(header + etherLayer + "\n" + ipLayer + "\n" + lastLayer);
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
