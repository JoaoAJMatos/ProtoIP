// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

using ProtoIP.Common;
using static ProtoIP.Packet;

namespace ProtoIP
{
      public class ProtoStream
      {
            const int BUFFER_SIZE = Common.Network.DEFAULT_BUFFER_SIZE;

            private NetworkStream _stream;
            private List<Packet> _packets = new List<Packet>();
            private byte[] _buffer;
            private string _LastError;
            
            /* CONSTRUCTORS */
            public ProtoStream() { }
            public ProtoStream(NetworkStream stream) { this._stream = stream; }
            public ProtoStream(List<Packet> packets) { this._packets = packets; }
            public ProtoStream(List<Packet> packets, NetworkStream stream) { this._packets = packets; this._stream = stream; }

            /* PRIVATE METHODS & HELPER FUNCTIONS */

            /*
             * Tries to read from the stream to construct a packet
             * Returns the deserialized packet
             */
            private Packet receivePacket()
            {
                  this._buffer = new byte[BUFFER_SIZE];
                  if (this.TryRead(this._buffer) < BUFFER_SIZE)
                  {
                        this._LastError = "Failed to receive the packet";
                        return null;
                  }

                  return Packet.Deserialize(this._buffer);
            }

            /*
             * Receives an ACK packet from the peer
             * Returns true if the packet was received successfully, false otherwise
             */
            private bool peerReceiveAck()
            {
                  Packet packet = this.receivePacket();
                  if (packet._GetType() != (int)Packet.Type.ACK)
                  {
                        this._LastError = "Invalid packet type";
                        return false;
                  }

                  return true;
            }

            /*
            * Sends the ACK packet to the peer
            * Returns true if the packet was sent successfully, false otherwise
            */
            private bool peerSendAck()
            {
                  Packet packet = new Packet(Packet.Type.ACK);

                  this._buffer = Packet.Serialize(packet);
                  if (this.TryWrite(this._buffer) < BUFFER_SIZE)
                  {
                        this._LastError = "Failed to send the ACK packet";
                        return false;
                  }

                  return true;
            }

            /*
            * Sends the SOT packet to the peer
            * Returns true if the packet was sent successfully, false otherwise
            */
            private bool peerSendSot()
            {
                  this._buffer = Packet.Serialize(new Packet(Packet.Type.SOT));
                  if (this.TryWrite(this._buffer) < BUFFER_SIZE)
                  {
                        this._LastError = "Failed to send the START_OF_TRANSMISSION packet";
                        return false;
                  }

                  return true;
            }

            /*
            * Receives the SOT packet from the peer
            * Returns true if the packet was received successfully, false otherwise
            */
            private bool peerReceiveSot()
            {
                  Packet packet = this.receivePacket();
                  if (packet != null && packet._GetType() != (int)Packet.Type.SOT)
                  {
                        this._LastError = "Invalid packet type";
                        return false;
                  }

                  return true;
            }

            /*
            * Sends the EOT packet to the peer
            * Returns true if the packet was sent successfully, false otherwise
            */
            private bool peerSendEot()
            {
                  this._buffer = Packet.Serialize(new Packet(Packet.Type.EOT));
                  if (this.TryWrite(this._buffer) < BUFFER_SIZE)
                  {
                        this._LastError = "Failed to send the END_OF_TRANSMISSION packet";
                        return false;
                  }

                  return true;
            }

            /*
            * Receives the EOT packet from the peer
            * Returns true if the packet was received successfully, false otherwise
            */
            private bool peerReceiveEot()
            {
                  Packet packet = this.receivePacket();
                  if (packet._GetType() != (int)Packet.Type.EOT)
                  {
                        this._LastError = "Invalid packet type";
                        return false;
                  }

                  return true;
            }

            /*
            * Sends a REPEAT packet with the missing packet IDs to the peer
            */
            private bool peerSendRepeat(List<int> missingPackets)
            {
                  Packet packet = new Packet(Packet.Type.REPEAT);

                  byte[] payload = new byte[missingPackets.Count * sizeof(int)];
                  Buffer.BlockCopy(missingPackets.ToArray(), 0, payload, 0, payload.Length);

                  packet.SetPayload(payload);
                  this._buffer = Packet.Serialize(packet);

                  if (this.TryWrite(this._buffer) < BUFFER_SIZE)
                  {
                        this._LastError = "Failed to send the REPEAT packet";
                        return false;
                  }

                  return true;
            }

            /*
            * Receives the REPEAT packet from the requesting peer
            * Returns the missing packet IDs
            */
            private List<int> peerReceiveRepeat()
            {
                  Packet packet = this.receivePacket();
                  if (packet._GetType() != (int)Packet.Type.REPEAT)
                  {
                        this._LastError = "Invalid packet type";
                        return null;
                  }

                  byte[] payload = packet.GetDataAs<byte[]>();
                  int[] missingPackets = new int[payload.Length / sizeof(int)];
                  Buffer.BlockCopy(payload, 0, missingPackets, 0, payload.Length);

                  return new List<int>(missingPackets);
            }

            /*
            * Resends the missing packets to the peer
            */
            private bool peerResendMissingPackets(List<int> packetIDs)
            {
                  for (int i = 0; i < packetIDs.Count; i++)
                  {
                        this._buffer = Packet.Serialize(this._packets[packetIDs[i]]);
                        if (this.TryWrite(this._buffer) < BUFFER_SIZE)
                        {
                              this._LastError = "Failed to send the packet " + packetIDs[i];
                              return false;
                        }
                  }

                  return true;
            }
 
            /*
            * Receives the missing packets from the peer and adds them to the packet List
            */
            private bool peerReceiveMissingPackets(int packetCount)
            {
                  for (int i = 0; i < packetCount; i++)
                  {
                        this._buffer = new byte[BUFFER_SIZE];
                        if (this.TryRead(this._buffer) < BUFFER_SIZE)
                        {
                              this._LastError = "Failed to receive the packet " + i;
                              return false;
                        }

                        Packet packet = Packet.Deserialize(this._buffer);
                        this._packets.Add(packet);
                  }

                  return true;
            }

            /*
             * Validate the packet List
             *
             * Check if there are any null packets or if there are any ID jumps
             */
            private bool ValidatePackets()
            {
                  for (int i = 0; i < this._packets.Count; i++)
                  {
                        if (this._packets[i] == null)
                        {
                              this._LastError = "Packet " + i + " is null";
                              return false;
                        }

                        if (this._packets[i]._GetId() != i)
                        {
                              this._LastError = "Packet " + i + " has an invalid id (Expected: " + i + ", Actual: " + this._packets[i]._GetId() + ")";
                              return false;
                        }
                  }

                  return true;
            }

            /*
             * Returns a list with the missing packet IDs
             *
             * Check for any ID jumps, if there is an ID jump, add the missing IDs to the list
             */
            private List<int> GetMissingPacketIDs()
            {
                  List<int> missingPackets = new List<int>();
                  int lastId = 0;

                  foreach (Packet packet in this._packets)
                  {
                        if (packet._GetId() - lastId > 1)
                        {
                              for (int i = lastId + 1; i < packet._GetId(); i++)
                              {
                                    missingPackets.Add(i);
                              }
                        }

                        lastId = packet._GetId();
                  }

                  return missingPackets;
            }

            /*
             * Orders the packets by id and assembles the data buffer
             *
             * Allocates a buffer with the total length of the data and copies the data from the packets to the buffer
             */
            private int Assemble()
            {
                  ProtoStream.OrderPackets(this._packets);

                  if (!this.ValidatePackets())
                  {
                        return -1;
                  }

                  int dataLength = 0;
                  for (int i = 0; i < this._packets.Count; i++) { dataLength += this._packets[i].GetDataAs<byte[]>().Length; }
                  byte[] data = new byte[dataLength];

                  int dataOffset = 0;
                  for (int i = 0; i < this._packets.Count; i++)
                  {
                        byte[] packetData = this._packets[i].GetDataAs<byte[]>();
                        Array.Copy(packetData, 0, data, dataOffset, packetData.Length);
                        dataOffset += packetData.Length;
                  }

                  this._buffer = data;
                  return 0;
            }

            /*
             * Attemps to write the data to the stream until it succeeds or the number of tries is reached
             */
            private int TryWrite(byte[] data, int tries = Network.MAX_TRIES)
            {
                  int bytesWritten = 0;

                  while (bytesWritten < data.Length && tries > 0)
                  {
                        try
                        {
                              if (this._stream.CanWrite)
                              {
                                    this._stream.Write(data, bytesWritten, data.Length - bytesWritten);
                                    bytesWritten += data.Length - bytesWritten;
                              }
                        }
                        catch (Exception e)
                        {
                              tries--;
                        }
                  }

                  return bytesWritten;
            }

            /*
             * Attemps to read the data from the stream until it succeeds or the number of tries is reached
             */
            private int TryRead(byte[] data, int tries = Network.MAX_TRIES)
            {
                  int bytesRead = 0;

                  while (bytesRead < data.Length && tries > 0)
                  {
                        try
                        {
                              if (this._stream.DataAvailable || this._stream.CanRead)
                                    bytesRead += this._stream.Read(data, bytesRead, data.Length - bytesRead);
                        }
                        catch (Exception e)
                        {
                              tries--;
                        }
                  }

                  return bytesRead;
            }

            /*
             * Partitions the data into packets and adds them to the Packet list
             */
            private void Partition(byte[] data)
            {
                  if (data.Length > (BUFFER_SIZE) - Packet.HEADER_SIZE)
                  {
                        int numPackets = (int)Math.Ceiling((double)data.Length / ((BUFFER_SIZE) - Packet.HEADER_SIZE));
                        int packetSize = (int)Math.Ceiling((double)data.Length / numPackets);

                        this._packets = new List<Packet>();
                        for (int i = 0; i < numPackets; i++)
                        {
                              int packetDataSize = (i == numPackets - 1) ? data.Length - (i * packetSize) : packetSize;
                              byte[] packetData = new byte[packetDataSize];
                              Array.Copy(data, i * packetSize, packetData, 0, packetDataSize);

                              this._packets.Add(new Packet(Packet.Type.BYTES, i, packetDataSize, packetData));
                        }
                  }
                  else
                  {
                        this._packets = new List<Packet>();
                        this._packets.Add(new Packet(Packet.Type.BYTES, 0, data.Length, data));
                  }
            }

            /* PUBLIC METHODS */
            /*
             * Checks if a peer is connected to the stream
             */
            public bool IsConnected()
            {
                  return this._stream != null && this._stream.CanRead && this._stream.CanWrite;
            }

            /*
             * Transmits the data to the peer
             *
             * Ensures that the peer is ready to receive the data.
             * Partitions the data into packets and sends them to the peer.
             * Waits for the peer to acknowledge the data.
             * Allows the peer to request missing packets until all the data
             * has been received or the maximum number of tries has been reached.
             */
            public int Transmit(byte[] data)
            {
                  this.Partition(data);

                  this.peerSendSot();
                  if (this.peerReceiveAck() == false) { return -1; }
                  ProtoStream.SendPackets(this._stream, this._packets);
                  this.peerSendEot();

                  int tries = 0;

                  while (tries < Network.MAX_TRIES)
                  {
                        Packet response = this.receivePacket();
                        if (response == null) { return -1; }
                        if (response._GetType() == (int)Packet.Type.ACK) { break; }
                        
                        else if (response._GetType() == (int)Packet.Type.REPEAT)
                        {
                              List<int> missingPacketIDs = this.peerReceiveRepeat();
                              if (missingPacketIDs.Any()) { this.peerResendMissingPackets(missingPacketIDs); }
                              else { return -1; }
                        }

                        tries++;
                  }

                  this._packets = new List<Packet>();
                  return 0;
            }

            /*
             * Sends a string to the peer
             */
            public int Transmit(string data)
            {
                  return this.Transmit(Encoding.ASCII.GetBytes(data));
            }

            /*
             * Receives data from the peer until the EOT packet is received
             */
            public int Receive()
            {
                  bool dataFullyReceived = false;
                  int tries = Network.MAX_TRIES;

                  if (this.peerReceiveSot() == false) { return -1; }
                  if (this.peerSendAck() == false) { return -1; }

                  while (true)
                  {
                        if (this.TryRead(this._buffer) < BUFFER_SIZE) { return -1; }

                        Packet packet = Packet.Deserialize(this._buffer);

                        if (packet._GetType() == (int)Packet.Type.EOT) { break; }

                        this._packets.Add(packet);
                  }

                  while (!dataFullyReceived && tries > 0)
                  {
                        List<int> missingPacketIDs = GetMissingPacketIDs();
                        if (missingPacketIDs.Count > 0)
                        {
                              this.peerSendRepeat(missingPacketIDs);
                              this.peerReceiveMissingPackets(missingPacketIDs.Count);
                        }
                        else
                        {
                              if (this.peerSendAck() == false) { return -1; }
                              dataFullyReceived = true;
                        }

                        tries--;
                  }

                  return 0;
            }

            // Return the assembled data as a primitive type T
            public T GetDataAs<T>()
            {
                  this.Assemble();

                  byte[] data = this._buffer;

                  // Convert the data to the specified type
                  switch (typeof(T).Name)
                  {
                        case "String":
                              return (T)(object)Encoding.ASCII.GetString(data);
                        case "Int32":
                              return (T)(object)BitConverter.ToInt32(data, 0);
                        case "Int64":
                              return (T)(object)BitConverter.ToInt64(data, 0);
                        case "Byte[]":
                              return (T)(object)data;
                        default:
                              this._LastError = "Invalid type";
                              return default(T);
                  }
            }

            /* STATIC METHODS */
            /*
            * OrderPackets
            * Orders the packets by id
            */
            static void OrderPackets(List<Packet> packets)
            {
                  packets.Sort((x, y) => x._GetId().CompareTo(y._GetId()));
            }

            /*
            * SendPackets
            * Sends the packets to the peer
            */
            static void SendPackets(NetworkStream stream, List<Packet> packets)
            {
                  for (int i = 0; i < packets.Count; i++)
                  {
                        SendPacket(stream, packets[i]);
                  }
            }

            public static void SendPacket(NetworkStream stream, Packet packet)
            {
                  stream.Write(Packet.Serialize(packet), 0, BUFFER_SIZE);
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
