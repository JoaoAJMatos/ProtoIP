// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System.Net.Sockets;
using System.Text;
using System.IO;
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
            private bool peerAckReceive()
            {
                  this._buffer = new byte[BUFFER_SIZE];
                  if (this.TryRead(this._buffer) < BUFFER_SIZE)
                  {
                        this._LastError = "Failed to receive the ACK packet";
                        return false;
                  }

                  Packet packet = Packet.Deserialize(this._buffer);

                  if (packet._GetType() != (int)Packet.Type.ACK)
                  {
                        this._LastError = "Invalid packet type";
                        return false;
                  }

                  return true;
            }

            private bool peerAckSend()
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

            private bool peerTransmitionStartSend()
            {
                  this._buffer = Packet.Serialize(new Packet(Packet.Type.SOT));
                  if (this.TryWrite(this._buffer) < BUFFER_SIZE)
                  {
                        this._LastError = "Failed to send the START_OF_TRANSMISSION packet";
                        return false;
                  }

                  return true;
            }

            private bool peerTransmitionStartReceive()
            {
                  this._buffer = new byte[BUFFER_SIZE];
                  if (this.TryRead(this._buffer) < BUFFER_SIZE)
                  {
                        this._LastError = "Failed to receive the START_OF_TRANSMISSION packet";
                        return false;
                  }

                  Packet packet = Packet.Deserialize(this._buffer);
                  if (packet._GetType() != (int)Packet.Type.SOT)
                  {
                        this._LastError = "Invalid packet type";
                        return false;
                  }

                  return true;
            }

            private bool peerTransmitionEndSend()
            {
                  this._buffer = Packet.Serialize(new Packet(Packet.Type.EOT));
                  if (this.TryWrite(this._buffer) < BUFFER_SIZE)
                  {
                        this._LastError = "Failed to send the END_OF_TRANSMISSION packet";
                        return false;
                  }

                  return true;
            }

            private bool peerTransmitionEndReceive()
            {
                  this._buffer = new byte[BUFFER_SIZE];
                  if (this.TryRead(this._buffer) < BUFFER_SIZE)
                  {
                        this._LastError = "Failed to receive the END_OF_TRANSMISSION packet";
                        return false;
                  }

                  Packet packet = Packet.Deserialize(this._buffer);
                  if (packet._GetType() != (int)Packet.Type.EOT)
                  {
                        this._LastError = "Invalid packet type";
                        return false;
                  }

                  return true;
            }

            private bool ValidatePackets()
            {
                  for (int i = 0; i < this._packets.Count; i++)
                  {
                        // If the packet is null, return an error
                        if (this._packets[i] == null)
                        {
                              this._LastError = "Packet " + i + " is null";
                              return false;
                        }

                        // If the packet id is not the same as the index, return an error
                        if (this._packets[i]._GetId() != i)
                        {
                              this._LastError = "Packet " + i + " has an invalid id (Expected: " + i + ", Actual: " + this._packets[i]._GetId() + ")";
                              return false;
                        }
                  }

                  return true;
            }

            /*
             * Orders the packets by id and assembles the data buffer
             */
            private int Assemble()
            {
                  ProtoStream.OrderPackets(this._packets);

                  if (!this.ValidatePackets())
                  {
                        return -1;
                  }

                  // Get the total length of the data and create the buffer
                  int dataLength = 0;
                  for (int i = 0; i < this._packets.Count; i++) { dataLength += this._packets[i].GetDataAs<byte[]>().Length; }
                  byte[] data = new byte[dataLength];

                  // Copy the data from the packets to the buffer
                  int dataOffset = 0;
                  for (int i = 0; i < this._packets.Count; i++)
                  {
                        byte[] packetData = this._packets[i].GetDataAs<byte[]>();
                        Array.Copy(packetData, 0, data, dataOffset, packetData.Length);
                        dataOffset += packetData.Length;
                  }

                  // Set the buffer and return
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
                              this._stream.Write(data, bytesWritten, data.Length - bytesWritten);
                              bytesWritten += data.Length - bytesWritten;
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
             * Partitions the data into packets and adds them to the list
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
             * Transmits the data to the peer
             *
             * If the data passed is bigger than the buffer size, it will be split into packets.
             */
            public int Transmit(byte[] data)
            {
                  this.Partition(data);

                  this.peerTransmitionStartSend();
                  if (this.peerAckReceive() == false) { return -1; }

                  ProtoStream.SendPackets(this._stream, this._packets);

                  this.peerTransmitionEndSend();

                  if (this.peerAckReceive() == false) { return -1; }

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

            // Transmit a file
            public int Transmit(FileInfo file)
            {
                  if (!file.Exists)
                  {
                        this._LastError = "File does not exist";
                        return -1;
                  }

                  this.peerTransmitionStartSend();
                  if (this.peerAckReceive() == false) { return -1; }

                  // Send the file name
                  this.Transmit(Encoding.ASCII.GetBytes(file.Name));

                  // Send the file size
                  this.Transmit(Encoding.ASCII.GetBytes(file.Length.ToString()));

                  // Send the file data
                  using (FileStream fs = file.OpenRead())
                  {
                        byte[] data = new byte[fs.Length];
                        fs.Read(data, 0, data.Length);
                        this.Transmit(data);
                  }

                  this.peerTransmitionEndSend();

                  if (this.peerAckReceive() == false) { return -1; }
                  return 0;
            }

            /*
             * Receives data from the peer until the END_OF_TRANSMISSION packet is received
             */
            public int Receive()
            {
                  if (this.peerTransmitionStartReceive() == false) { 
                        return -1; 
                  }

                  if (this.peerAckSend() == false) { 
                        return -1; 
                  }

                  while (true)
                  {
                        if (this.TryRead(this._buffer) < BUFFER_SIZE) { return -1; }

                        Packet packet = Packet.Deserialize(this._buffer);

                        if (packet._GetType() == (int)Packet.Type.EOT) { break; }

                        this._packets.Add(packet);
                  }

                  if (this.peerAckSend() == false) { return -1; }
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