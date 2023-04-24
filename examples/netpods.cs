// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;
using System.Threading;

using ProtoIP;

class Program 
{
      static void Main() 
      {
            Ethernet ethernet = new Ethernet();
            IP ip = new IP(sourceIP: "192.168.1.211", destinationIP: "192.168.1.211");
            TCP tcp = new TCP(sourcePort: 80, destinationPort: 80);

            // Create a new NetPod and
            // add the network layers using
            // the composition operator ("/")
            NetPod pod = new NetPod(ethernet / ip / tcp);

            // Show the NetPod structure
            NetPod.ShowStructure(pod);

            Action<NetPod> action = (NetPod receivedPod) => {
                  Console.WriteLine("Received packet: " + receivedPod);
            };

            // Start sniffing packets on the "lo0" interface
            // in a separate thread
            Thread receiveThread = new Thread(() => {
                  NetPod.Sniff("lo0", action);
            });
            receiveThread.Start();

            NetPod.Send(pod);
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
