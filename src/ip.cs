// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;
using System.Net;

namespace ProtoIP
{
      // IP protocol packet types.
      public enum IPProtocolPacketType
      {
            ICMP = 1,
            TCP = 6,
            UDP = 17
      }

      // Provides an interface for creating and manipulating IP packets
      // to be used with the NetPods.
      public class IP
      {
            private const int IP_HEADER_LENGTH = 20;
            private const int IPV4 = 4;
            private const int IPV6 = 6;

            // Headers
            public byte _version { get; private set; }
            public byte _headerLength { get; private set; }
            public byte _typeOfService { get; private set; }
            public ushort _totalLength { get; private set; }
            public ushort _identification { get; private set; }
            public ushort _flags { get; private set; }
            public ushort _fragmentOffset { get; private set; }
            public byte _timeToLive { get; private set; }
            public byte _protocol { get; private set; }
            public ushort _headerChecksum { get; private set; }
            public IPAddress _sourceAddress { get; private set; }
            public IPAddress _destinationAddress { get; private set; }

            // Payload
            public byte[] _payload { get; private set; }

            // Serializes the packet and returns it as a byte Array
            public byte[] Serialize()
            {
                  byte[] packet = new byte[IP_HEADER_LENGTH + _payload.Length];
                  packet[0] = (byte)((_version << 4) + _headerLength);
                  packet[1] = _typeOfService;
                  packet[2] = (byte)(_totalLength >> 8);
                  packet[3] = (byte)(_totalLength & 0xFF);
                  packet[4] = (byte)(_identification >> 8);
                  packet[5] = (byte)(_identification & 0xFF);
                  packet[6] = (byte)((_flags << 5) + _fragmentOffset);
                  packet[7] = (byte)(_fragmentOffset & 0xFF);
                  packet[8] = _timeToLive;
                  packet[9] = _protocol;
                  packet[10] = (byte)(_headerChecksum >> 8);
                  packet[11] = (byte)(_headerChecksum & 0xFF);
                  Array.Copy(_sourceAddress.GetAddressBytes(), 0, packet, 12, 4);
                  Array.Copy(_destinationAddress.GetAddressBytes(), 0, packet, 16, 4);
                  Array.Copy(_payload, 0, packet, IP_HEADER_LENGTH, _payload.Length);
                  return packet;
            }

            // Deserializes a byte Array and returns an IP object
            public static IP Deserialize(byte[] packet)
            {
                  if (packet.Length < IP_HEADER_LENGTH) { return null; }

                  IP ip = new IP();
                  ip._version = (byte)(packet[0] >> 4);
                  ip._headerLength = (byte)(packet[0] & 0x0F);
                  ip._typeOfService = packet[1];
                  ip._totalLength = (ushort)((packet[2] << 8) + packet[3]);
                  ip._identification = (ushort)((packet[4] << 8) + packet[5]);
                  ip._flags = (ushort)(packet[6] >> 5);
                  ip._fragmentOffset = (ushort)(((packet[6] & 0x1F) << 8) + packet[7]);
                  ip._timeToLive = packet[8];
                  ip._protocol = packet[9];
                  ip._headerChecksum = (ushort)((packet[10] << 8) + packet[11]);
                  ip._sourceAddress = new IPAddress(new byte[] { packet[12], packet[13], packet[14], packet[15] });
                  ip._destinationAddress = new IPAddress(new byte[] { packet[16], packet[17], packet[18], packet[19] });
                  ip._payload = new byte[ip._totalLength - IP_HEADER_LENGTH];
                  Array.Copy(packet, IP_HEADER_LENGTH, ip._payload, 0, ip._totalLength - IP_HEADER_LENGTH);
                  return ip;
            }

            /* OPERATOR OVERLOADS */
            //
            // Operator overload for the / operator.
            // Similar to scapy's Ether() / IP() / TCP() syntax.
            // You can use it as a composition packet builder.
            //
            // Add raw data to the payload of an IP packet
            // using the composition operator.
            public static IP operator / (IP ip, byte[] data)
            {
                  ip._payload = data;
                  ip._totalLength = (ushort)(IP_HEADER_LENGTH + ip._payload.Length);
                  return ip;
            }

            // Encapsulate a TCP fragment into an IP packet.
            public static IP operator / (IP ip, TCP tcp)
            {
                  ip._protocol = (byte)IPProtocolPacketType.TCP;
                  byte[] tcpSerializedData = tcp.Serialize();
                  return ip / tcpSerializedData;
            }

            // Encapsulate a UDP packet into an IP packet.
            public static IP operator / (IP ip, ICMP icmp) 
            {
                  ip._protocol = (byte)IPProtocolPacketType.ICMP;
                  byte[] icmpSerializedData = icmp.Serialize();
                  return ip / icmpSerializedData;
            }            

            // Returns a string representation of the IP packet.
            public override string ToString()
            {
                  return $"### [IP] ###\n" +
                         $"\tVersion: {_version}\n" +
                         $"\tHeader Length: {_headerLength}\n" +
                         $"\tType of Service: {_typeOfService}\n" +
                         $"\tTotal Length: {_totalLength}\n" +
                         $"\tIdentification: {_identification}\n" +
                         $"\tFlags: {_flags}\n" +
                         $"\tFragment Offset: {_fragmentOffset}\n" +
                         $"\tTime to Live: {_timeToLive}\n" +
                         $"\tProtocol: {_protocol}\n" +
                         $"\tHeader Checksum: {_headerChecksum}\n" +
                         $"\tSource Address: {_sourceAddress}\n" +
                         $"\tDestination Address: {_destinationAddress}\n";
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
