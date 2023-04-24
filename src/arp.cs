// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;

namespace ProtoIP
{
      // Provides an interface for creating and manipulating ARP packets
      // to be used with the NetPods.
      public class ARP
      {
            // Hardware types for the transmission medium
            public enum HardwareType : ushort
            {
                  Ethernet = 1,
                  IEEE802Networks = 6,
                  ARCNET = 7,
                  FrameRelay = 15,
                  ATM = 16,
                  HDLC = 17,
                  FibreChannel = 18,
                  ATM2 = 19,
                  SerialLine = 20
            }

            // Op codes for the ARP protocol
            public enum Operation : ushort
            {
                  ARPRequest = 1,
                  ARPReply = 2,
                  RARPRequest = 3,
                  RARPReply = 4,
                  DRARPRequest = 5,
                  DRARPReply = 6,
                  DRARPError = 7,
                  InARPRequest = 8,
                  InARPReply = 9
            }

            public ushort _hardwareType { get; set; }
            public ushort _protocolType { get; set; }
            public byte _hardwareLength { get; set; }
            public byte _protocolLength { get; set; }
            public ushort _operation { get; set; }
            public byte[] _senderHardwareAddress { get; set; }
            public byte[] _senderProtocolAddress { get; set; }
            public byte[] _targetHardwareAddress { get; set; }
            public byte[] _targetProtocolAddress { get; set; }

            /* CONSTRUCTORS */
            public ARP() { }

            public byte[] Serialize()
            {
                  byte[] serialized = new byte[28];
                  Array.Copy(BitConverter.GetBytes(_hardwareType), 0, serialized, 0, 2);
                  Array.Copy(BitConverter.GetBytes(_protocolType), 0, serialized, 2, 2);
                  serialized[4] = _hardwareLength;
                  serialized[5] = _protocolLength;
                  Array.Copy(BitConverter.GetBytes(_operation), 0, serialized, 6, 2);
                  Array.Copy(_senderHardwareAddress, 0, serialized, 8, 6);
                  Array.Copy(_senderProtocolAddress, 0, serialized, 14, 4);
                  Array.Copy(_targetHardwareAddress, 0, serialized, 18, 6);
                  Array.Copy(_targetProtocolAddress, 0, serialized, 24, 4);
                  return serialized;
            }

            public static ARP Deserialize(byte[] data)
            {
                  ARP arp = new ARP();
                  arp._hardwareType = BitConverter.ToUInt16(data, 0);
                  arp._protocolType = BitConverter.ToUInt16(data, 2);
                  arp._hardwareLength = data[4];
                  arp._protocolLength = data[5];
                  arp._operation = BitConverter.ToUInt16(data, 6);
                  arp._senderHardwareAddress = new byte[6];
                  Array.Copy(data, 8, arp._senderHardwareAddress, 0, 6);
                  arp._senderProtocolAddress = new byte[4];
                  Array.Copy(data, 14, arp._senderProtocolAddress, 0, 4);
                  arp._targetHardwareAddress = new byte[6];
                  Array.Copy(data, 18, arp._targetHardwareAddress, 0, 6);
                  arp._targetProtocolAddress = new byte[4];
                  Array.Copy(data, 24, arp._targetProtocolAddress, 0, 4);
                  return arp;
            }

            public override string ToString()
            {
                  return $"### [ARP] ###\n" +
                         $"\tHardware Type: {_hardwareType}\n" +
                         $"\tProtocol Type: {_protocolType}\n" +
                         $"\tHardware Length: {_hardwareLength}\n" +
                         $"\tProtocol Length: {_protocolLength}\n" +
                         $"\tOperation: {_operation}\n" +
                         $"\tSender Hardware Address: {BitConverter.ToString(_senderHardwareAddress)}\n" +
                         $"\tSender Protocol Address: {BitConverter.ToString(_senderProtocolAddress)}\n" +
                         $"\tTarget Hardware Address: {BitConverter.ToString(_targetHardwareAddress)}\n" +
                         $"\tTarget Protocol Address: {BitConverter.ToString(_targetProtocolAddress)}\n";
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
