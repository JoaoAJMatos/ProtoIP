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
            public class FileSystem
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
            public class Compression
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
