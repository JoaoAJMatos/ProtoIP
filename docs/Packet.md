# ProtoPacket

Packets provide the user with **low level access to protocol definitions**, similar to **ProtocolSI**. Allowing you to manipulate the contents of the packets such as the **headers** or the **payload**.

Packets have a fixed size of `1024` bytes, including the headers (which take up `12` bytes):

- **12 bytes** - Packet headers
- **1012 bytes** - Packet payload (*and padding*)

When assigning data to a packet, if the data isn't enough to fill the entire payload buffer, padding will be added to the packet until it fills an entire `1024` byte buffer to ensure every packet has the same size.

## Packet types

**ProtoIP** provides a set of default packet types that can be used to identify the type of data being transmitted. These types are defined in the `Common.Packet` class and are as follows:

- `ACK` - Acknowledgement.
- `HANDSHAKE_REQ` - Handshake request.
- `HANDSHAKE_RES` - Handshake response.
- `PUBLIC_KEY` - Public key.
- `BYTES` - Byte array.
- `REPEAT` - To repeat a given packet in a **PacketStream** (check [ProtoStreams](ProtoStream.md)).
- `SOT` - Start of transmission.
- `EOT` - End of transmission.
- `FTS` - File transfer start.
- `FTE` - File transfer end.
- `FILE_NAME` - File name.
- `FILE_SIZE` - File size.
- `CHECKSUM` - File Checksum.
- `PING` - Ping request.
- `PONG` - Ping response.
- `BROADCAST` - Broadcast packet.

These definitions serve as boilerplate for the user to use when creating their own protocol definitions. However, you can use **any integer value** as a packet type, giving you the flexibility you need to **define your own protocols**.

## Members

- `_type` - Int
- `_id` - Int (**PacketStream** _index_)
- `_dataSize` - Int
- `_data` - Byte array

## Methods

### Constructors

- `Packet()`
- `Packet(int type)` - Creates a Packet with a given type and the default packet id of `0`.
- `Packet(int type, int id, int dataSize, byte[] data)`
- `Packet(int type, int id, int dataSize, string data)`
- `Packet(string data)` - This method will automatically convert the string to bytes and assign it to the packet's payload with the default packet type of `BYTES`.

### Public functions

> Serialization

- `byte[] Serialize()` - Returns a byte array with the serialized packet.
- `Packet Deserialize(byte[] data)` - Deserializes the given byte array and constructs a packet.

> Data manipulation

- `void SetData(byte[] data)` - Sets the packet's payload to the given byte array and updates `_dataSize`.
- `void SetData(string data)` - Sets the packet's payload to the given string and updates `_dataSize`.

> Getters

- `string GetDataAsString()` - Returns the packet's payload as a string.
- `byte[] GetData()` - Returns the packet's payload as a byte array.
- `int GetId()` - Returns the packet's id.
- `int GetType()` - Returns the packet's type.
- `int GetDataSize()` - Returns the packet's payload size.

## Example

```csharp
using ProtoIP;

class Program 
{
      // Define your own packet types to implement
      // your own protocols
      private const int HELLO_PACKET = 1;

      static void Main() 
      {
            // Create a new packet
            Packet packet = new Packet(HELLO_PACKET);
            packet.SetData("Hello World!");

            // Or:
            // 
            // Packet packet = new Packet(HELLO_PACKET, 0, 12, "Hello World!");

            // Serialize the packet
            // You can send this buffer over the network and deserialize it on the other end
            byte[] serializedPacket = packet.Serialize();

            // Deserialize the packet
            Packet deserializedPacket = Packet.Deserialize(serializedPacket);

            // Get the packet's payload
            string payload = deserializedPacket.GetDataAs<string>();
      }
}
```
