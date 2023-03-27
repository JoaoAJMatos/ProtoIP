using System.Text;
using System;

using ProtoIP.Common;

namespace ProtoIP
{
      class Packet {
            const int BUFFER_SIZE = Common.Network.DEFAULT_BUFFER_SIZE;

            // Header
            private int _type;
            private int _id;
            private int _dataSize;

            // Payload
            private byte[] _data;

            /* CONSTRUCTORS */
            public Packet() {}

            public Packet(int type, int id, int dataSize, byte[] data) {
                  this._type = type;
                  this._id = id;
                  this._dataSize = dataSize;
                  this._data = data;
            }

            public Packet(int type, int id, int dataSize, string data) {
                  this._type = type;
                  this._id = id;
                  this._dataSize = dataSize;
                  this._data = Encoding.ASCII.GetBytes(data);
            }

            public Packet(int type) {
                  this._type = type;
                  this._id = 0;
                  this._dataSize = 0;
                  this._data = null;
            }

            public Packet(string stringData) {
                  this._type = Common.Packet.Type.BYTES;
                  this._id = 0;
                  this._dataSize = stringData.Length;
                  this._data = Encoding.ASCII.GetBytes(stringData);
            }

            static public byte[] Serialize(Packet packet) {
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
                  if (buffer.Length < BUFFER_SIZE) {
                        byte[] padding = new byte[BUFFER_SIZE - buffer.Length];
                        Buffer.BlockCopy(padding, 0, buffer, buffer.Length, padding.Length);
                  }

                  return buffer;
            }

            static public Packet Deserialize(byte[] buffer) {
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

            public override string ToString() {
                  return Encoding.ASCII.GetString(this._data);
            }

            /* GETTERS & SETTERS */
            public int _GetType() { return this._type; }
            public int _GetId() { return this._id; }
            public int _GetDataSize() { return this._dataSize; }
            public byte[] _GetData() { return this._data; }

            public void _SetType(Common.Packet.Type type) { this._type = type; }
            public void _SetId(int id) { this._id = id; }
            public void _SetDataSize(int dataSize) { this._dataSize = dataSize; }
            public void _SetData(byte[] data) { this._data = data; this._dataSize = data.Length; }
            public void _SetData(string data) { this._data = Encoding.ASCII.GetBytes(data); this._dataSize = data.Length; }
      }
}