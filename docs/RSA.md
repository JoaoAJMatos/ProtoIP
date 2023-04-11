# RSA - Rivest–Shamir–Adleman

**ProtoCrypto** provides a simple interface for **asymmetric encryption** with RSA.

You can use RSA to **encrypt** and **decrypt** data using a **pair of cryptographic keys**. This can be useful for implementing **secure protocols** and **applications**.

Checkout [this](secure-protocols.md) example on how to use RSA in a **real world application**.

## Members

- `RSAParameters _privateKey` - RSA private key
- `RSAParameters _publicKey` - RSA public key

## Constructors

- `RSA()` - Creates a new RSA instance.
- `RSA(RSAParameters privateKey, RSAParameters publicKey)` - Creates a new RSA instance with the given private and public keys.

## Methods

> Key generation

- `GenerateKeyPair()` - Generates a new RSA key pair.

> Encryption and decryption

- `static byte[] Encrypt(byte[] data, byte[] publicKey)` - Encrypts the given data using a given exported RSA key.
- `byte[] Decrypt(byte[] data)` - Decrypts the given data using RSA.

> Digital signatures

- `byte[] Sign(byte[] data)` - Signs the given data using RSA and returns a signature.
- `bool Verify(byte[] data, byte[] signature, byte[] exportedPublicKey)` - Verifies the given data using RSA and the given signature.

> Key exchange

You can export the RSA public key in a pre-defined format in order to exchange it with other parties.

- `byte[] ExportPublicKey()` - Exports the RSA public key in a pre-defined format.

## Example

```csharp
using ProtoIP;

class Program
{
      static void Main()
      {
            // Create a new RSA instance
            Crypto.RSA rsa = new Crypto.RSA();
      
            // Generate a new RSA key-pair
            rsa.GenerateKeyPair();
      
            // The data to encrypt
            byte[] data = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05 };
      
            // Encrypt the data
            byte[] encryptedData = Crypto.RSA.Encrypt(data, rsa.ExportPublicKey());
      
            // Decrypt the data
            byte[] decryptedData = rsa.Decrypt(encryptedData);
      }
}

```
