// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;
using System.Net;

namespace ProtoIP
{
      // Provides an interface for creating and manipulating UDP packets
      // to be used with the NetPods.
      public class UDP 
      {
            public const int UDP_HEADER_LENGTH = 8;

            // HEADER
            public ushort _sourcePort      { get; set; }
            public ushort _destinationPort { get; set; }
            public ushort _length          { get; set; }
            public ushort _checksum        { get; set; }

            // PAYLOAD
            public byte[] _payload   { get; set; }

            // Serializes the UDP packet into a byte array.
            public byte[] Serialize()
            {
                  byte[] serialized = new byte[_length];
                  Array.Copy(BitConverter.GetBytes(_sourcePort), 0, serialized, 0, 2);
                  Array.Copy(BitConverter.GetBytes(_destinationPort), 0, serialized, 2, 2);
                  Array.Copy(BitConverter.GetBytes(_length), 0, serialized, 4, 2);
                  Array.Copy(BitConverter.GetBytes(_checksum), 0, serialized, 6, 2);
                  Array.Copy(_payload, 0, serialized, 8, _payload.Length);
                  return serialized;
            }

            // Deserializes a byte array into a UDP packet.
            public static UDP Deserialize(byte[] serialized)
            {
                  if (serialized.Length < UDP_HEADER_LENGTH) { return null; }

                  UDP udp = new UDP();
                  udp._sourcePort = BitConverter.ToUInt16(serialized, 0);
                  udp._destinationPort = BitConverter.ToUInt16(serialized, 2);
                  udp._length = BitConverter.ToUInt16(serialized, 4);
                  udp._checksum = BitConverter.ToUInt16(serialized, 6);
                  udp._payload = new byte[udp._length - 8];
                  Array.Copy(serialized, 8, udp._payload, 0, udp._length - 8);
                  return udp;
            }
             
            public override string ToString()
            {
                  return $"### [UDP] ###\n" +
                         $"Source Port: {_sourcePort}\n" +
                         $"Destination Port: {_destinationPort}\n" +
                         $"Length: {_length}\n" +
                         $"Checksum: {_checksum}\n" +
                         $"Payload: {_payload}\n";
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
