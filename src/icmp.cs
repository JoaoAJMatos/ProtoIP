// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;

namespace ProtoIP
{
      // Provides an interface for creating and manipulating ICMP packets
      // to be used with the NetPods.
      public class ICMP
      {
            // ICMP Packet types
            public enum ICMPType
            {
                  EchoReply = 0,
                  DestinationUnreachable = 3,
                  SourceQuench = 4,
                  Redirect = 5,
                  EchoRequest = 8,
                  TimeExceeded = 11,
                  ParameterProblem = 12,
                  TimestampRequest = 13,
                  TimestampReply = 14,
                  InformationRequest = 15,
                  InformationReply = 16,
                  AddressMaskRequest = 17,
                  AddressMaskReply = 18
            }

            public const int ICMP_HEADER_LENGTH = 20;

            // ICMP headers
            public byte _type { get; set; }
            public byte _code { get; set; }
            public ushort _checksum { get; set; }

            // ICMP payload
            public byte[] _payload { get; set; }

            // Serializes the packet and returns it as a byte Array
            public byte[] Serialize()
            {
                  byte[] packet = new byte[4 + _payload.Length];
                  packet[0] = _type;
                  packet[1] = _code;
                  packet[2] = (byte)(_checksum >> 8);
                  packet[3] = (byte)(_checksum & 0xFF);
                  Array.Copy(_payload, 0, packet, 4, _payload.Length);
                  return packet;
            }

            // Deserializes a byte Array into an ICMP packet
            public static ICMP Deserialize(byte[] packet)
            {
                  if (packet.Length < ICMP_HEADER_LENGTH) { return null; }

                  ICMP icmp = new ICMP();
                  icmp._type = packet[0];
                  icmp._code = packet[1];
                  icmp._checksum = (ushort)((packet[2] << 8) + packet[3]);
                  icmp._payload = new byte[packet.Length - 4];
                  Array.Copy(packet, 4, icmp._payload, 0, packet.Length - 4);
                  return icmp;
            }
 
            public override string ToString()
            {
                  return $"### [ICMP] ###\n" +
                         $"\n\nType: {_type}\n" +
                         $"\n\nCode: {_code}\n" +
                         $"\n\nChecksum: {_checksum}\n";
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
