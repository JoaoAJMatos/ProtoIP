// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;
using System.IO;
using System.IO.Compression;

namespace ProtoIP
{
      namespace Util
      {
            // Interface for interacting with the file system
            class FileSystem
            {
                  // File system basic utilities
                  public static bool Exists(string path) { return File.Exists(path) || Directory.Exists(path); }
                  public static bool IsFile(string path) { return File.Exists(path); }
                  public static bool IsDirectory(string path) { return Directory.Exists(path); }
                  public static void CreateFile(string path) { File.Create(path); }
                  public static void CreateDirectory(string path) { Directory.CreateDirectory(path); }

                  // Delete a file or directory
                  public static void Delete(string path)
                  {
                        if (IsFile(path))
                        {
                              File.Delete(path);
                        }
                        else if (IsDirectory(path))
                        {
                              Directory.Delete(path);
                        }
                  }

                  // Copy a file or directory
                  public static void Copy(string source, string destination)
                  {
                        if (IsFile(source))
                        {
                              File.Copy(source, destination);
                        }
                        else if (IsDirectory(source))
                        {
                              //Directory.Copy(source, destination);
                        }
                  }

                  // Move a file or directory
                  public static void Move(string source, string destination)
                  {
                        if (IsFile(source))
                        {
                              File.Move(source, destination);
                        }
                        else if (IsDirectory(source))
                        {
                              Directory.Move(source, destination);
                        }
                  }

                  // Rename a file or directory
                  public static void Rename(string path, string name)
                  {
                        if (IsFile(path))
                        {
                              File.Move(path, Path.Combine(Path.GetDirectoryName(path), name));
                        }
                        else if (IsDirectory(path))
                        {
                              Directory.Move(path, Path.Combine(Path.GetDirectoryName(path), name));
                        }
                  }
            }

            // Interface for compressing and decompressing data
            class Compression
            {
                  // Compress a byte array using GZip and return the compressed data
                  public static byte[] Compress(byte[] data)
                  {
                        using (var compressedStream = new MemoryStream())
                        {
                              using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                              {
                                    zipStream.Write(data, 0, data.Length);
                              }

                              return compressedStream.ToArray();
                        }
                  }

                  // Decompress a byte array using GZip and return the decompressed data
                  public static byte[] Decompress(byte[] data)
                  {
                        using (var compressedStream = new MemoryStream(data))
                        {
                              using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                              {
                                    using (var resultStream = new MemoryStream())
                                    {
                                          zipStream.CopyTo(resultStream);
                                          return resultStream.ToArray();
                                    }
                              }
                        }
                  }

                  // Compress a file using GZip and save it to a destination
                  public static void CompressFile(string filePath, string destination)
                  {
                        File.WriteAllBytes(destination, Compress(File.ReadAllBytes(filePath)));
                  }

                  // Decompress a file using GZip and save it to a destination
                  public static void DecompressFile(string compressedFilePath, string destination)
                  {
                        File.WriteAllBytes(destination, Decompress(File.ReadAllBytes(compressedFilePath)));
                  }
            }
      }
}