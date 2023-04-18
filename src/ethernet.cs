// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;
using System.Net;
using System.Net.NetworkInformation;

namespace ProtoIP
{
      // Provides an interface for creating and manipulating Ethernet frames
      // to be used with the NetPods.
      public class Ethernet
      {
            public readonly static int MAC_ADDRESS_LENGTH = 6;
            public readonly static int ETH_HEADER_LENGTH = 14;
            public readonly static int ETH_TYPE_IP = 0x0800;
            public readonly static int ETH_TYPE_ARP = 0x0806;
            public readonly static byte[] BROADCAST_MAC = Ethernet.GetMACAddressBytesFromString("FF:FF:FF:FF:FF:FF");

            public byte[] _destinationMAC { get; private set; }
            public byte[] _sourceMAC { get; private set; }
            public ushort _type { get; private set; }
            public byte[] _payload { get; private set; }

            // Serializes the packet and returns it as a byte Array
            public byte[] Serialize()
            {
                  byte[] packet = new byte[ETH_HEADER_LENGTH + _payload.Length];
                  Array.Copy(_destinationMAC, 0, packet, 0, MAC_ADDRESS_LENGTH);
                  Array.Copy(_sourceMAC, 0, packet, MAC_ADDRESS_LENGTH, MAC_ADDRESS_LENGTH);
                  packet[12] = (byte)(_type >> 8);
                  packet[13] = (byte)(_type & 0xFF);
                  Array.Copy(_payload, 0, packet, ETH_HEADER_LENGTH, _payload.Length);
                  return packet;
            }

            // Deserializes the incoming byte array and returns an Ethernet object
            public static Ethernet Deserialize(byte[] packet)
            {
                  Ethernet ethernet = new Ethernet();
                  ethernet._destinationMAC = new byte[MAC_ADDRESS_LENGTH];
                  ethernet._sourceMAC = new byte[MAC_ADDRESS_LENGTH];
                  ethernet._payload = new byte[packet.Length - ETH_HEADER_LENGTH];

                  Array.Copy(packet, 0, ethernet._destinationMAC, 0, MAC_ADDRESS_LENGTH);
                  Array.Copy(packet, MAC_ADDRESS_LENGTH, ethernet._sourceMAC, 0, MAC_ADDRESS_LENGTH);
                  ethernet._type = (ushort)((packet[12] << 8) + packet[13]);
                  Array.Copy(packet, ETH_HEADER_LENGTH, ethernet._payload, 0, packet.Length - ETH_HEADER_LENGTH);
                  return ethernet;
            }

            // Returns the MAC address bytes of the specified MAC address
            public static byte[] GetMACAddressBytesFromString(string macAddress)
            {
                  string[] macAddressBytes = macAddress.Split(':');
                  byte[] bytes = new byte[MAC_ADDRESS_LENGTH];

                  for (int i = 0; i < MAC_ADDRESS_LENGTH; i++)
                  {
                        bytes[i] = Convert.ToByte(macAddressBytes[i], 16);
                  }

                  return bytes;
            }

            // Returns the MAC address bytes of the specified IP address.
            // Attempts to fetch the MAC address from an IP address using an ARP request.
            // If the ARP request fails, it iterates through all network interfaces
            // to find the one with the specified IP address.
            public static byte[] GetMACAddressBytesFromIP(string ipAddressString)
            {
                  byte[] macAddressBytes = null;
 
                  if (!ArpRequestWithWebClient(ipAddressString, macAddressBytes))
                  {
                        FetchNetworkInterfacesForIPAddress(ipAddressString, macAddressBytes);
                  }

                  return macAddressBytes;
            }

            // Iterates through all network interfaces to find the one with the specified IP address.
            // Populates the outputBuffer with the MAC address bytes of the specified IP address.
            //
            // Returns true if the MAC address was found, false otherwise.
            public static void FetchNetworkInterfacesForIPAddress(string ipAddressString, byte[] macAddressBytes)
            {
                  IPAddress ipAddress = IPAddress.Parse(ipAddressString);
                  foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                  {
                        foreach (UnicastIPAddressInformation ipAddressInfo in networkInterface.GetIPProperties().UnicastAddresses)
                        {
                              if (ipAddressInfo.Address.Equals(ipAddress))
                              {
                                    macAddressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
                              }
                        }
                  }
            }

            // Attempts to send an ARP request to a specified IP address using a WebClient.
            // Populates the outputBuffer with the MAC address bytes of the specified IP address
            // returned by the ARP request.
            //
            // Returns true if the ARP request was successful, false otherwise.
            public static bool ArpRequestWithWebClient(string ipAddress, byte[] outputBuffer)
            {
                   using (var client = new WebClient())
                  {
                        client.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                        try
                        {
                              byte[] response = client.DownloadData($"http://{ipAddress}");
                              if (response.Length >= MAC_ADDRESS_LENGTH)
                              {
                                    outputBuffer = new byte[MAC_ADDRESS_LENGTH];
                                    Array.Copy(response, 0, outputBuffer, 0, MAC_ADDRESS_LENGTH);
                              }
                        }
                        catch (WebException)
                        {
                              return false;
                        }
                  }

                  return outputBuffer != null;
            }

            // Returns the MAC address string of the specified MAC address bytes
            public static string MacAddressBytesToString(byte[] macAddressBytes)
            {
                  string macAddress = string.Empty;
                  for (int i = 0; i < macAddressBytes.Length; i++)
                  {
                        macAddress += macAddressBytes[i].ToString("X2");
                        if (i != macAddressBytes.Length - 1)
                        {
                              macAddress += ":";
                        }
                  }
                  return macAddress;
            }

            /* OPERATOR OVERLOADS */
            //
            // Operator overload for the / operator.
            // Similar to scapy's Ether() / IP() / TCP() syntax.
            // You can use it as a composition packet builder.
            //
            // Add raw data to the payload of an Ethernet frame
            // using the composition operator.
            public static Ethernet operator / (Ethernet ethernet, byte[] data)
            {
                  ethernet._payload = data;
                  return ethernet;
            }

            // Encapsulate an IP packet inside an Ethernet frame
            public static Ethernet operator / (Ethernet ethernet, IP ipPacket)
            {
                  byte[] payload = ipPacket.Serialize();
                  return ethernet / payload;
            }

            public override string ToString()
            {
                  return $"### [Ethernet] ###\n" +
                        $"Destination MAC: {MacAddressBytesToString(_destinationMAC)}\n" +
                        $"Source MAC: {MacAddressBytesToString(_sourceMAC)}\n" +
                        $"Type: {(_type == ETH_TYPE_IP ? "IP" : "ARP")}\n";
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
