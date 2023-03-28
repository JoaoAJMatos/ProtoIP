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