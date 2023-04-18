// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;
using System.Net;
using System.Net.Sockets;

namespace ProtoIP
{
      // Provides an interface for creating and manipulating TCP segments
      // to be used with the NetPods.
      public class TCP
      {
            public const int TCP_HEADER_LENGTH = 20;

            // Header
            public ushort _sourcePort         { get; private set; }
            public ushort _destinationPort    { get; private set; }
            public int _sequenceNumber        { get; private set; }
            public int _acknowledgementNumber { get; private set; }
            public ushort _dataOffset         { get; private set; }
            public ushort _reserved           { get; private set; }
            public ushort _flags              { get; private set; }
            public ushort _windowSize         { get; private set; }
            public ushort _checksum           { get; private set; }
            public ushort _urgentPointer      { get; private set; }
            public byte[] _options            { get; private set; }

            // Payload
            public byte[] _payload { get; private set; }

            // Serializes the packet and returns it as a byte Array
            public byte[] Serialize()
            {
                  byte[] packet = new byte[TCP_HEADER_LENGTH + _payload.Length];
                  packet[0] = (byte)(_sourcePort >> 8);
                  packet[1] = (byte)(_sourcePort & 0xFF);
                  packet[2] = (byte)(_destinationPort >> 8);
                  packet[3] = (byte)(_destinationPort & 0xFF);
                  packet[4] = (byte)(_sequenceNumber >> 24);
                  packet[5] = (byte)(_sequenceNumber >> 16);
                  packet[6] = (byte)(_sequenceNumber >> 8);
                  packet[7] = (byte)(_sequenceNumber & 0xFF);
                  packet[8] = (byte)(_acknowledgementNumber >> 24);
                  packet[9] = (byte)(_acknowledgementNumber >> 16);
                  packet[10] = (byte)(_acknowledgementNumber >> 8);
                  packet[11] = (byte)(_acknowledgementNumber & 0xFF);
                  packet[12] = (byte)((_dataOffset << 4) + _reserved);
                  packet[13] = (byte)_flags;
                  packet[14] = (byte)(_windowSize >> 8);
                  packet[15] = (byte)(_windowSize & 0xFF);
                  packet[16] = (byte)(_checksum >> 8);
                  packet[17] = (byte)(_checksum & 0xFF);
                  packet[18] = (byte)(_urgentPointer >> 8);
                  packet[19] = (byte)(_urgentPointer & 0xFF);
                  Array.Copy(_options, 0, packet, 20, _options.Length);
                  Array.Copy(_payload, 0, packet, TCP_HEADER_LENGTH, _payload.Length);
                  return packet;

                  /*byte[] packet = new byte[IP.IP_HEADER_LENGTH + _payload.Length];
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
                  Array.Copy(_payload, 0, packet, IP.IP_HEADER_LENGTH, _payload.Length);
                  return packet;*/
            }

            // Deserializes a byte Array into a TCP packet
            public static TCP Deserialize(byte[] packet)
            {
                  TCP tcp = new TCP();
                  tcp._sourcePort = (ushort)((packet[0] << 8) + packet[1]);
                  tcp._destinationPort = (ushort)((packet[2] << 8) + packet[3]);
                  tcp._sequenceNumber = (int)((packet[4] << 24) + (packet[5] << 16) + (packet[6] << 8) + packet[7]);
                  tcp._acknowledgementNumber = (int)((packet[8] << 24) + (packet[9] << 16) + (packet[10] << 8) + packet[11]);
                  tcp._dataOffset = (ushort)((packet[12] >> 4) + packet[13]);
                  tcp._reserved = (ushort)((packet[12] << 4) + packet[13]);
                  tcp._flags = (ushort)((packet[14] << 8) + packet[15]);
                  tcp._windowSize = (ushort)((packet[16] << 8) + packet[17]);
                  tcp._checksum = (ushort)((packet[18] << 8) + packet[19]);
                  tcp._urgentPointer = (ushort)((packet[20] << 8) + packet[21]);
                  tcp._options = new byte[tcp._dataOffset - TCP_HEADER_LENGTH];
                  Array.Copy(packet, 22, tcp._options, 0, tcp._options.Length);
                  tcp._payload = new byte[packet.Length - tcp._dataOffset];
                  Array.Copy(packet, tcp._dataOffset, tcp._payload, 0, tcp._payload.Length);
                  return tcp;

                  /*TCP tcp = new TCP();
                  tcp._sourcePort = (ushort)((packet[0] << 8) + packet[1]);
                  tcp._destinationPort = (ushort)((packet[2] << 8) + packet[3]);
                  tcp._sequenceNumber = (int)((packet[4] << 24) + (packet[5] << 16) + (packet[6] << 8) + packet[7]);
                  tcp._acknowledgementNumber = (int)((packet[8] << 24) + (packet[9] << 16) + (packet[10] << 8) + packet[11]);
                  tcp._dataOffset = (ushort)((packet[12] >> 4) + packet[13]);
                  tcp._reserved = (ushort)((packet[12] << 4) + packet[13]);
                  tcp._flags = (ushort)((packet[14] << 8) + packet[15]);
                  tcp._windowSize = (ushort)((packet[16] << 8) + packet[17]);
                  tcp._checksum = (ushort)((packet[18] << 8) + packet[19]);
                  tcp._urgentPointer = (ushort)((packet[20] << 8) + packet[21]);
                  tcp._options = new byte[tcp._dataOffset - 20];
                  Array.Copy(packet, 22, tcp._options, 0, tcp._dataOffset - 20);
                  tcp._payload = new byte[packet.Length - tcp._dataOffset];
                  Array.Copy(packet, tcp._dataOffset, tcp._payload, 0, packet.Length - tcp._dataOffset);
                  return tcp;*/
            }

            /* OPERATOR OVERLOADS */
            //
            // Operator overload for the / operator.
            // Similar to scapy's Ether() / IP() / TCP() syntax.
            // You can use it as a composition packet builder.
            //
            // Add raw data to the payload of a TCP fragment
            // using the composition operator.
            public static TCP operator / (TCP tcp, byte[] payload)
            {
                  tcp._payload = payload;
                  return tcp;
            }

            public override string ToString()
            {
                  return $"### [TCP] ###\n" +
                        $"Source Port: {_sourcePort}\n" +
                        $"Destination Port: {_destinationPort}\n" +
                        $"Sequence Number: {_sequenceNumber}\n" +
                        $"Acknowledgement Number: {_acknowledgementNumber}\n" +
                        $"Data Offset: {_dataOffset}\n" +
                        $"Reserved: {_reserved}\n" +
                        $"Flags: {_flags}\n" +
                        $"Window Size: {_windowSize}\n" +
                        $"Checksum: {_checksum}\n" +
                        $"Urgent Pointer: {_urgentPointer}\n" +
                        $"Options: {_options}\n";
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
