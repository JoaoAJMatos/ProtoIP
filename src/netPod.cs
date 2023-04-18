// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;
using System.Net;
using System.Net.Sockets;

namespace ProtoIP
{
      // NetPods implement an abstraction for sending and receiving data over a raw socket.
      // This allows you to create and manipulate raw packets from all kinds of network layers.
      class NetPod
      {
            private Ethernet _ethernet;
            private IP _ip;
            private UDP _udp;
            private TCP _tcp;
            private ICMP _icmp;

            // Creates a new NetPod instance.
            public NetPod()
            {
                  _ethernet = new Ethernet();
                  _ip = new IP();
                  _udp = new UDP();
                  _tcp = new TCP();
                  _icmp = new ICMP();
            }

            // Removes the layer with the given type from the NetPod.
            public void RemoveLayer<TLayer>() 
            {
                  if (typeof(TLayer) == typeof(Ethernet)) { _ethernet = null; }
                  else if (typeof(TLayer) == typeof(IP)) { _ip = null; }
                  else if (typeof(TLayer) == typeof(UDP)) { _udp = null; }
                  else if (typeof(TLayer) == typeof(TCP)) { _tcp = null; }
                  else if (typeof(TLayer) == typeof(ICMP)) { _icmp = null; }
            }

            // Returns the layer with the given type from the NetPod.
            public TLayer GetLayer<TLayer>() 
            {
                  if (typeof(TLayer) == typeof(Ethernet)) { return (TLayer)(object)_ethernet; }
                  else if (typeof(TLayer) == typeof(IP)) { return (TLayer)(object)_ip; }
                  else if (typeof(TLayer) == typeof(UDP)) { return (TLayer)(object)_udp; }
                  else if (typeof(TLayer) == typeof(TCP)) { return (TLayer)(object)_tcp; }
                  else if (typeof(TLayer) == typeof(ICMP)) { return (TLayer)(object)_icmp; }
                  else { return default(TLayer); }
            }

            // Sends a NetPod over a raw socket.
            public static void Send(NetPod pod)
            {
            
            }

            // Receives a NetPod from a raw socket.
            public static void Receive(NetPod pod)
            {

            }

            // Shows the netpod structure in a human-readable format.
            public void ShowStructure()
            {

            }

            // Shows the netpod structure in a hexdump format.
            public void HexDump()
            {

            }

            // Assembles the NetPod into a byte array.
            // This byte array can be sent over a raw socket.
            private void Assemble()
            {

            }

            // Disassembles the NetPod from a byte array.
            // This byte array can be received from a raw socket.
            private void Disassemble(byte[] data)
            {

            }

            /* OPERATOR OVERLOADS */
            //
            // Operator overload for the = operator.
            //
            // This allows you to assign layers to the netpod.
            // By assigning an Ethernet object to the NetPod the
            // NetPod will have an Ethernet layer, as well as all of the
            // subsequent layers encapsulated in the Ethernet layer.
            public static NetPod operator / (NetPod pod, Ethernet ethernet)
            {
                  byte[] etherPayload = ethernet._payload;
                  var deserializedIPPacket = IP.Deserialize(etherPayload);
                  
                  // If the deserializedIPPacket is null,
                  // then the payload is not an IP packet.
                  if (deserializedIPPacket == null) { return pod; }

                  // Assign the basic layers to the pod.
                  pod._ethernet = ethernet;
                  pod._ip = deserializedIPPacket;

                  byte[] ipPayload = deserializedIPPacket._payload;
                  
                  // Deserialize the payload according to the protocol.
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

                  return pod;
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
