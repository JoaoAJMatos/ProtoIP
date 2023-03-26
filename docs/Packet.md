# Packet

Packets provide the user with **low level access to protocol definitions**. Allowing you to manipulate the contents of the packets such as the **headers** or the **payload**.

Packets have a fixed size of `1024` bytes, including the headers (which take up `12` bytes):

- **12 bytes** - Packet headers
- **1012 bytes** - Packet payload

When assigning data to a packet, if the data isn't enough to fill the entire payload buffer, padding will be added to the packet until it fills an entire 1024 byte buffer.

## Members

- `_type` - Int
- `_id` - Int (_index_)
- `_dataSize` - Int
- `_data` - Byte array

## Methods
