// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;
using System.Net;

namespace ProtoIP
{  
      // Provides an interface for creating and manipulating IP packets
      // to be used with the NetPods.
      public class IP
      {
            // IP protocol packet types.
            public enum IPProtocolPacketType
            {
                  ICMP = 1,
                  TCP = 6,
                  UDP = 17
            }

            public const int IP_HEADER_LENGTH = 20;
            public const int IPV4 = 4;
            public const int IPV6 = 6;

            // Headers
            public byte _version { get; set; }
            public byte _headerLength { get; set; }
            public byte _typeOfService { get; set; }
            public ushort _totalLength { get; set; }
            public ushort _identification { get; set; }
            public ushort _flags { get; set; }
            public ushort _fragmentOffset { get; set; }
            public byte _timeToLive { get; set; }
            public byte _protocol { get; set; }
            public ushort _headerChecksum { get; set; }
            public IPAddress _sourceAddress { get; set; }
            public IPAddress _destinationAddress { get; set; }

            // Payload
            public byte[] _payload { get; set; }

            /* CONSTRUCTORS */
            public IP() { }

            public IP(string sourceIP, string destinationIP)
            {
                  _version = IPV4;
                  _headerLength = 5;
                  _typeOfService = 0;
                  _totalLength = IP_HEADER_LENGTH;
                  _identification = 0;
                  _flags = 0;
                  _fragmentOffset = 0;
                  _timeToLive = 64;
                  _protocol = 0;
                  _headerChecksum = 0;
                  _sourceAddress = IPAddress.Parse(sourceIP);
                  _destinationAddress = IPAddress.Parse(destinationIP);
            }

            // Serializes the packet and returns it as a byte Array
            public byte[] Serialize()
            {
                  byte[] packet = new byte[IP_HEADER_LENGTH + _payload.Length];

                  // Header
                  packet[0] = (byte)((_version << 4) | _headerLength);
                  packet[1] = _typeOfService;
                  packet[2] = (byte)(_totalLength >> 8);
                  packet[3] = (byte)(_totalLength & 0xFF);
                  packet[4] = (byte)(_identification >> 8);
                  packet[5] = (byte)(_identification & 0xFF);
                  packet[6] = (byte)((_flags << 5) | (_fragmentOffset >> 8));
                  packet[7] = (byte)(_fragmentOffset & 0xFF);
                  packet[8] = _timeToLive;
                  packet[9] = _protocol;
                  packet[10] = (byte)(_headerChecksum >> 8);
                  packet[11] = (byte)(_headerChecksum & 0xFF);

                  byte[] sourceAddressBytes = _sourceAddress.GetAddressBytes();
                  packet[12] = sourceAddressBytes[0];
                  packet[13] = sourceAddressBytes[1];
                  packet[14] = sourceAddressBytes[2];
                  packet[15] = sourceAddressBytes[3];

                  byte[] destinationAddressBytes = _destinationAddress.GetAddressBytes();
                  packet[16] = destinationAddressBytes[0];
                  packet[17] = destinationAddressBytes[1];
                  packet[18] = destinationAddressBytes[2];
                  packet[19] = destinationAddressBytes[3];

                  Array.Copy(_payload, 0, packet, IP_HEADER_LENGTH, _payload.Length);

                  return packet;
            }

            // Deserializes a byte Array and returns an IP object
            public static IP Deserialize(byte[] packet)
            {
                  if (packet.Length < IP_HEADER_LENGTH) { return null; }

                  IP ip = new IP();

                  // Header
                  ip._version = (byte)(packet[0] >> 4);
                  ip._headerLength = (byte)(packet[0] & 0x0F);
                  ip._typeOfService = packet[1];
                  ip._totalLength = (ushort)((packet[2] << 8) | packet[3]);
                  ip._identification = (ushort)((packet[4] << 8) | packet[5]);
                  ip._flags = (ushort)(packet[6] >> 5);
                  ip._fragmentOffset = (ushort)(((packet[6] & 0x1F) << 8) | packet[7]);
                  ip._timeToLive = packet[8];
                  ip._protocol = packet[9];
                  ip._headerChecksum = (ushort)((packet[10] << 8) | packet[11]);
                  ip._sourceAddress = new IPAddress(new byte[] { packet[12], packet[13], packet[14], packet[15] });
                  ip._destinationAddress = new IPAddress(new byte[] { packet[16], packet[17], packet[18], packet[19] });

                  // Payload
                  ip._payload = new byte[ip._totalLength - IP_HEADER_LENGTH];
                  Array.Copy(packet, IP_HEADER_LENGTH, ip._payload, 0, ip._payload.Length);

                  return ip;
            }             

            // Returns a string representation of the IP packet.
            public override string ToString()
            {
                  return $"  ### [IP] ###\n" +
                         $"  Version: {_version}\n" +
                         $"  Header Length: {_headerLength}\n" +
                         $"  Type of Service: {_typeOfService}\n" +
                         $"  Total Length: {_totalLength}\n" +
                         $"  Identification: {_identification}\n" +
                         $"  Flags: {_flags}\n" +
                         $"  Fragment Offset: {_fragmentOffset}\n" +
                         $"  Time to Live: {_timeToLive}\n" +
                         $"  Protocol: {_protocol}\n" +
                         $"  Header Checksum: {_headerChecksum}\n" +
                         $"  Source Address: {_sourceAddress}\n" +
                         $"  Destination Address: {_destinationAddress}";
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
