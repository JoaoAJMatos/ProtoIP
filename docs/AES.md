# AES - Advanced Encryption Standard

**ProtoCrypto** provides a simple interface for **symmetric encryption** with AES.

You can use AES to **encrypt** and **decrypt** data using a **shared secret key**. This can be useful for implementing **secure protocols** and **applications**.

Checkout [this](secure-protocols.md) example on how to use AES in a **real world application**.

## Members

- `_key` - byte array containing the AES key

## Constructors

- `AES()` - Creates a new AES instance.
- `AES(byte[] key)` - Creates a new AES instance with the given key.

## Methods

> Key generation

- `void GenerateKey()` - Generates a new AES key.

> Encryption and decryption

- `byte[] Encrypt(byte[] data)` - Encrypts the given data using AES.
- `byte[] Decrypt(byte[] data)` - Decrypts the given data using AES.

> Getters and logging

- `string GetKeyString()` - Returns the AES key as a string.
- `void ShowKey()` - Logs the AES key as a string to the console.

## Example

```csharp

using ProtoIP;

class Program
{
      static void Main()
      {
            // Create a new AES instance
            AES aes = new AES();
      
            // Generate a new AES key
            aes.GenerateKey();
      
            // Show the AES key
            aes.ShowKey();
      
            // The data to encrypt
            byte[] data = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05 };
      
            // Encrypt the data
            byte[] encryptedData = aes.Encrypt(data);
      
            // Decrypt the data
            byte[] decryptedData = aes.Decrypt(encryptedData);
      }
}
```

And to exchange the AES keys:

```csharp

using ProtoIP;

class Program
{
      static void Main()
      {
            // Create a new AES instance
            AES aes = new AES();
      
            // Generate a new AES key
            aes.GenerateKey();
      
            // Export the AES key, you can then send this to the other party
            byte[] exportedKey = aes._key;
      
            // The data to encrypt
            byte[] data = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05 };
      
            // Encrypt the data
            byte[] encryptedData = aes.Encrypt(data);
      
            // The other party receives the key and creates a new AES instance
            AES aes2 = new AES(exportedKey);

            // Decrypt the data using the exported key
            byte[] decryptedData = aes2.Decrypt(encryptedData);
      }
}

```
