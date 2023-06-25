// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System.Security.Cryptography;
using System.Text;
using System;

namespace ProtoIP
{
      namespace Crypto
      {
            public class HASH
            {
                  public byte[] _digest { get; set; }

                  // Get the digest as a hexadecimal string
                  public string GetDigestString()
                  {
                        StringBuilder sb = new StringBuilder();
                        foreach (byte b in _digest)
                        {
                              sb.Append(b.ToString("x2"));
                        }
                        return sb.ToString();
                  }

                  public static byte[] GenerateRandomBytes(int length)
                  {
                        byte[] bytes = new byte[length];
                        using (var rng = new RNGCryptoServiceProvider())
                        {
                              rng.GetBytes(bytes);
                        }
                        return bytes;
                  }
            }

            // Secure Hash Algorithm 256 (SHA256)
            // Provides an interface for hashing data using the SHA256 algorithm
            public class SHA256 : HASH
            {
                  // Hash a given byte array and set the digest
                  public SHA256(byte[] data) { _digest = Hash(data); }

                  public static byte[] Hash(byte[] data)
                  {
                        using (var sha256 = SHA256Managed.Create())
                        {
                              return sha256.ComputeHash(data);
                        }
                  }

                  // Hash a given byte array and salt and set the digest
                  public static byte[] Hash(byte[] data, byte[] salt)
                  {
                        byte[] dataWithSalt = new byte[data.Length + salt.Length];
                        Buffer.BlockCopy(data, 0, dataWithSalt, 0, data.Length);
                        Buffer.BlockCopy(salt, 0, dataWithSalt, data.Length, salt.Length);
                        return Hash(dataWithSalt);
                  }
            }

            // Message-Digest Algorithm 5 (MD5)
            // Provides an interface for hashing and verifying data using the MD5 algorithm
            public class MD5 : HASH
            {
                  // Hash a given byte array and set the digest
                  public MD5(byte[] data) { _digest = Hash(data); }

                  public static byte[] Hash(byte[] data)
                  {
                        using (var md5 = MD5CryptoServiceProvider.Create())
                        {
                              return md5.ComputeHash(data);
                        }
                  }

                  public static byte[] Hash(byte[] data, byte[] salt)
                  {
                        byte[] dataWithSalt = new byte[data.Length + salt.Length];
                        Buffer.BlockCopy(data, 0, dataWithSalt, 0, data.Length);
                        Buffer.BlockCopy(salt, 0, dataWithSalt, data.Length, salt.Length);
                        return Hash(dataWithSalt);
                  }
            }

            // Advanced Encryption Standard (AES)
            // Provides methods for encrypting and decrypting data using AES
            public class AES
            {
                  private const int KEY_SIZE = 256;

                  public byte[] _key { get; private set; }

                  // Constructors
                  public AES() { }
                  public AES(byte[] key) { this._key = key; }

                  // Generate a random AES key
                  public void GenerateKey()
                  {
                        using (var aes = Aes.Create())
                        {
                              aes.KeySize = KEY_SIZE;
                              aes.GenerateKey();
                              _key = aes.Key;
                        }
                  }

                  public static byte[] DeriveKeyFromPassword(string password, byte[] salt)
                  {
                        using (var aes = Aes.Create())
                        {
                              aes.KeySize = KEY_SIZE;
                              return new Rfc2898DeriveBytes(password, salt).GetBytes(aes.KeySize / 8);
                        }
                  }

                  // Encrypt a byte array using the generated key
                  // Returns a byte array containing the IV and the encrypted data
                  public byte[] Encrypt(byte[] data)
                  {
                        if (_key == null) throw new Exception("Key not set");
                        using (var aes = Aes.Create())
                        {
                              aes.KeySize = KEY_SIZE;
                              aes.Key = _key;
                              aes.GenerateIV();
                              byte[] iv = aes.IV;
                              byte[] encrypted = aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
                              byte[] result = new byte[iv.Length + encrypted.Length];
                              Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                              Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);
                              return result;
                        }
                  }

                  // Decrypt a byte array using the generated key
                  // Returns a byte array containing the decrypted data
                  public byte[] Decrypt(byte[] data)
                  {
                        if (_key == null) throw new Exception("Key not set");
                        using (var aes = Aes.Create())
                        {
                              aes.KeySize = KEY_SIZE;
                              aes.Key = _key;
                              byte[] iv = new byte[aes.IV.Length];
                              byte[] encrypted = new byte[data.Length - iv.Length];
                              Buffer.BlockCopy(data, 0, iv, 0, iv.Length);
                              Buffer.BlockCopy(data, iv.Length, encrypted, 0, encrypted.Length);
                              aes.IV = iv;
                              return aes.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
                        }
                  }
 
                  // Show the generated key as a string
                  public string GetKeyString() { return Convert.ToBase64String(_key); }
                  public byte[] GetKeyBytes() { return _key; }
                  public void ShowKey() { Console.WriteLine(GetKeyString()); }
            }

            // Rivest–Shamir–Adleman (RSA)
            // Provides methods for asymetric encryption and digital signatures using RSA
            public class RSA
            {
                  private RSAParameters _privateKey;
                  private RSAParameters _publicKey;

                  // Constructors
                  public RSA() { }

                  public RSA(RSAParameters privateKey, RSAParameters publicKey)
                  {
                        _privateKey = privateKey;
                        _publicKey = publicKey;
                  }

                  // Generate a random RSA key pair
                  public void GenerateKeyPair()
                  {
                        using (var rsa = new RSACryptoServiceProvider())
                        {
                              _privateKey = rsa.ExportParameters(true);
                              _publicKey = rsa.ExportParameters(false);
                        }
                  }

                  // Encrypt a byte array using the public key
                  public static byte[] Encrypt(byte[] data, byte[] publicKey)
                  {
                        byte[] modulus = new byte[128];
                        byte[] exponent = new byte[3];
                        Buffer.BlockCopy(publicKey, 0, modulus, 0, 128);
                        Buffer.BlockCopy(publicKey, 128, exponent, 0, 3);

                        using (var rsa = new RSACryptoServiceProvider())
                        {
                              var rsaParams = new RSAParameters
                              {
                                    Modulus = modulus,
                                    Exponent = exponent
                              };
                              rsa.ImportParameters(rsaParams);
                              return rsa.Encrypt(data, false);
                        }     
                  }

                  // Decrypt a byte array using the private key
                  public byte[] Decrypt(byte[] data)
                  {
                        using (var rsa = new RSACryptoServiceProvider())
                        {
                              rsa.ImportParameters(_privateKey);
                              return rsa.Decrypt(data, false);
                        }
                  }

                  // Sign a byte array using the private key
                  public byte[] Sign(byte[] data)
                  {
                        using (var rsa = new RSACryptoServiceProvider())
                        {
                              rsa.ImportParameters(_privateKey);
                              return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                        }
                  }

                  // Verify a signature using a public key
                  // 
                  // The public key is exported in a format specified in the comments
                  // for the ExportPublicKey method. To use it we must extract the information
                  // from the byte array and construct a new RSAParameters object
                  public bool Verify(byte[] data, byte[] signature, byte[] exportedPublicKey)
                  {
                        byte[] modulus = new byte[128];
                        byte[] exponent = new byte[3];
                        Buffer.BlockCopy(exportedPublicKey, 0, modulus, 0, 128);
                        Buffer.BlockCopy(exportedPublicKey, 128, exponent, 0, 3);

                        using (var rsa = new RSACryptoServiceProvider())
                        {
                              rsa.ImportParameters(new RSAParameters { Modulus = modulus, Exponent = exponent });
                              return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                        }
                  }

                  // Exports the modulus and exponent of the public key
                  // Returns a byte array containing the modulus and the exponent
                  //
                  // I had to implement this method because none of the export methods
                  // provided by the .NET Framework seemed to work on my platform
                  public byte[] ExportPublicKey()
                  {
                        byte[] modulus = _publicKey.Modulus;
                        byte[] exponent = _publicKey.Exponent;

                        byte[] publicKey = new byte[modulus.Length + exponent.Length];
                        Buffer.BlockCopy(modulus, 0, publicKey, 0, modulus.Length);
                        Buffer.BlockCopy(exponent, 0, publicKey, modulus.Length, exponent.Length);

                        return publicKey;
                  }
            }

            // Universal Unique Identifier (UUID) version 4
            // Provides methods for generating UUIDs
            public class UUIDv4
            {
                  // Generate a random UUID
                  // Returns a byte array containing the UUID
                  private static byte[] GenerateRandomBytes()
                  {
                        byte[] uuid = new byte[16];
                        using (var rng = new RNGCryptoServiceProvider())
                        {
                              rng.GetBytes(uuid);
                        }
                        uuid[6] = (byte)((uuid[6] & 0x0F) | 0x40);
                        uuid[8] = (byte)((uuid[8] & 0x3F) | 0x80);
                        return uuid;
                  }

                  // Generate a new UUID of type T
                  public static T newID<T>()
                  {
                        if (typeof(T) == typeof(byte[]))
                        {
                              return (T)(object)GenerateRandomBytes();
                        }
                        else if (typeof(T) == typeof(string))
                        {
                              return (T)(object)ToString(GenerateRandomBytes());
                        }
                        else
                        {
                              throw new Exception("Invalid type");
                        }
                  }

                  // Show a UUID as a string
                  public static string ToString(byte[] uuid)
                  {
                        return BitConverter.ToString(uuid).Replace("-", "");
                  }
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
