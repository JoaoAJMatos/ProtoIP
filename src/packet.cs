// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System.Text;
using System;

using ProtoIP.Common;

namespace ProtoIP
{
      public class Packet
      {
            // Default packet types for the ProtoIP library
            public enum Type
            {
                  ACK = 1,
                  HANDSHAKE_REQ,
                  HANDSHAKE_RES,
                  PUBLIC_KEY,
                  AES_KEY,
                  BYTES,
                  REPEAT,
                  EOT,
                  SOT,
                  FTS,
                  FTE,
                  FILE_NAME,
                  FILE_SIZE,
                  PING,
                  PONG,
                  CHECKSUM,
                  RELAY,
                  BROADCAST
            }

            public const int HEADER_SIZE = 12;
            const int BUFFER_SIZE = Common.Network.DEFAULT_BUFFER_SIZE;

            /* MEMBER VARIABLES */
            private int _type;
            private int _id;
            private int _dataSize;
            
            private byte[] _data;

            /* CONSTRUCTORS */
            public Packet() { }

            public Packet(int type, int id, int dataSize, byte[] data)
            {
                  this._type = type;
                  this._id = id;
                  this._dataSize = dataSize;
                  this._data = data;
            }

            public Packet(int type, int id, int dataSize, string data)
            {
                  this._type = type;
                  this._id = id;
                  this._dataSize = dataSize;
                  this._data = Encoding.ASCII.GetBytes(data);
            }

            public Packet(Type type, int id, int dataSize, byte[] data)
            {
                  this._type = (int)type;
                  this._id = id;
                  this._dataSize = dataSize;
                  this._data = data;
            }

            public Packet(int type)
            {
                  this._type = type;
                  this._id = 0;
                  this._dataSize = 0;
                  this._data = new byte[0];
            }

            public Packet(Type type)
            {
                  this._type = (int)type;
                  this._id = 0;
                  this._dataSize = 0;
                  this._data = new byte[0];
            }

            public Packet(string stringData)
            {
                  this._type = (int)Type.BYTES;
                  this._id = 0;
                  this._dataSize = stringData.Length;
                  this._data = Encoding.ASCII.GetBytes(stringData);
            }

            static public byte[] Serialize(Packet packet)
            {
                  byte[] buffer = new byte[BUFFER_SIZE];

                  // Packet Header
                  byte[] type = BitConverter.GetBytes(packet._type);
                  byte[] id = BitConverter.GetBytes(packet._id);
                  byte[] dataSize = BitConverter.GetBytes(packet._dataSize);

                  // Packet Payload
                  byte[] data = packet._data;

                  // Copy to buffer
                  Buffer.BlockCopy(type, 0, buffer, 0, type.Length);
                  Buffer.BlockCopy(id, 0, buffer, type.Length, id.Length);
                  Buffer.BlockCopy(dataSize, 0, buffer, type.Length + id.Length, dataSize.Length);
                  Buffer.BlockCopy(data, 0, buffer, type.Length + id.Length + dataSize.Length, data.Length);

                  // Add padding if needed
                  if (buffer.Length < BUFFER_SIZE)
                  {
                        byte[] padding = new byte[BUFFER_SIZE - buffer.Length];
                        Buffer.BlockCopy(padding, 0, buffer, buffer.Length, padding.Length);
                  }

                  return buffer;
            }

            static public Packet Deserialize(byte[] buffer)
            {
                  Packet packet = new Packet();

                  // Packet Header
                  byte[] type = new byte[4];
                  byte[] id = new byte[4];
                  byte[] dataSize = new byte[4];

                  // Packet Payload
                  byte[] data = new byte[BUFFER_SIZE - 12];

                  // Copy from buffer
                  Buffer.BlockCopy(buffer, 0, type, 0, type.Length);
                  Buffer.BlockCopy(buffer, type.Length, id, 0, id.Length);
                  Buffer.BlockCopy(buffer, type.Length + id.Length, dataSize, 0, dataSize.Length);
                  Buffer.BlockCopy(buffer, type.Length + id.Length + dataSize.Length, data, 0, data.Length);

                  // Set packet
                  packet._type = BitConverter.ToInt32(type, 0);
                  packet._id = BitConverter.ToInt32(id, 0);
                  packet._dataSize = BitConverter.ToInt32(dataSize, 0);
                  packet._data = data;

                  return packet;
            }

            // Shows the packet in the console
            static public void ByteDump(Packet packet)
            {
                  byte[] buffer = Serialize(packet);
                  int type = BitConverter.ToInt32(buffer, 0);
                  int id = BitConverter.ToInt32(buffer, 4);
                  int dataSize = BitConverter.ToInt32(buffer, 8);

                  // Show the header of the packet
                  Console.WriteLine("┌HEADERS");

                  // Show the type
                  Console.Write("├Type: ");
                  for (int i = 0; i < 4; i++) { Console.Write(buffer[i].ToString("X2") + " "); }
                  Console.Write("(" + type + " - ProtoIP Packet Mapping - " + (Type)type + ")");
                  Console.WriteLine();

                  // Show the ID
                  Console.Write("├ID: ");
                  for (int i = 4; i < 8; i++) { Console.Write(buffer[i].ToString("X2") + " "); }
                  Console.Write("(" + id + ")");
                  Console.WriteLine();

                  // Show the data size
                  Console.Write("├Data Size: ");
                  for (int i = 8; i < 12; i++) { Console.Write(buffer[i].ToString("X2") + " "); }
                  Console.WriteLine("(" + dataSize + ")");
                  Console.WriteLine("│");


                  // Show the body of the packet
                  Console.WriteLine("├PAYLOAD: ");
                  Console.Write("└Data: ");

                  if (dataSize == 0) 
                  { 
                        Console.WriteLine("No payload"); 
                  }
                  else
                  {
                        for (int i = 0; i < dataSize; i++)
                        {
                              if (i % 16 == 0 && i != 0) { Console.WriteLine(); Console.Write("   "); }
                              Console.Write(buffer[i + 12].ToString("X2") + " ");
                        }


                        // Show in string format
                        Console.Write("('");
                        for (int i = 0; i < dataSize; i++)
                        {
                              Console.Write((char)buffer[i + 12]);
                        }
                        Console.Write("') ");
                        Console.Write("( + " + (BUFFER_SIZE - dataSize) + " bytes of padding)");
                        Console.WriteLine();
                  }
            }

            /* GETTERS & SETTERS */
            public int _GetType() { return this._type; }
            public int _GetId() { return this._id; }
            public int _GetDataSize() { return this._dataSize; }

            // Returns the payload of the packet as the given data type
            public T GetDataAs<T>()
            {
                  byte[] data;

                  switch (typeof(T).Name)
                  {
                        case "String":
                              data = new byte[this._dataSize];
                              Buffer.BlockCopy(this._data, 0, data, 0, this._dataSize);
                              return (T)Convert.ChangeType(Encoding.ASCII.GetString(data), typeof(T));
                        case "Int32":
                              data = new byte[4];
                              Buffer.BlockCopy(this._data, 0, data, 0, 4);
                              return (T)Convert.ChangeType(BitConverter.ToInt32(data, 0), typeof(T));
                        case "Int64":
                              data = new byte[8];
                              Buffer.BlockCopy(this._data, 0, data, 0, 8);
                              return (T)Convert.ChangeType(BitConverter.ToInt64(data, 0), typeof(T));
                        case "UInt32":
                              data = new byte[4];
                              Buffer.BlockCopy(this._data, 0, data, 0, 4);
                              return (T)Convert.ChangeType(BitConverter.ToUInt32(data, 0), typeof(T));
                        case "UInt64":
                              data = new byte[8];
                              Buffer.BlockCopy(this._data, 0, data, 0, 8);
                              return (T)Convert.ChangeType(BitConverter.ToUInt64(data, 0), typeof(T));
                        case "Byte[]":
                              data = new byte[this._dataSize];
                              Buffer.BlockCopy(this._data, 0, data, 0, this._dataSize);
                              return (T)Convert.ChangeType(data, typeof(T));
                        default:
                              return (T)Convert.ChangeType(this._data, typeof(T));
                  }
            }

            public void _SetType(Type type) { this._type = (int)type; }
            public void _SetId(int id) { this._id = id; }
            public void _SetDataSize(int dataSize) { this._dataSize = dataSize; }
            public void SetPayload(byte[] data) { this._data = data; this._dataSize = data.Length; }
            public void SetPayload(string data) { this._data = Encoding.ASCII.GetBytes(data); this._dataSize = data.Length; }
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
