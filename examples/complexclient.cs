// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;
using System.Net.Sockets;
using System.Text;

// Include the ProtoIP namespace
using ProtoIP;

class ComplexClient : ProtoClient 
{
      // We can override the OnConnect() method to describe our Client logic
      // In this example, we are printing out the message we get back from the
      // remote host.
      public override void OnReceive() 
      {
            string data = _protoStream.GetDataAs<string>();
            Console.WriteLine(data);
      }
}

class Program 
{
      static void Main() 
      {
            // Create a new ComplexClient object and connect to the server
            ComplexClient client = new ComplexClient();
            client.Connect("1.1.1.1", 1234);

            // Send data to the server
            client.Send("Hello World!");

            // Receive the response
            // The OnReceive() method will be called
            client.Receive();

            // Disconnect from the server
            client.Disconnect();
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