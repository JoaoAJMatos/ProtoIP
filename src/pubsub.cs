// Copyright (c) 2023, João Matos
// Check the end of the file for extended copyright notice.

using System;
using System.Collections.Generic;

namespace ProtoIP
{
      // Interface that defines the methods for Publishing, Subscribing and 
      // Unsubscribing to a channel.
      public interface IMessageBroker
      {
            void Publish(string channel, object message);
            void Subscribe(string channel, Action<object> callback);
            void Unsubscribe(string channel, Action<object> callback);
      }

      // Publisher class that implements the Publish method.
      public class Publisher
      {
            private readonly IMessageBroker _messageBroker;

            public Publisher(IMessageBroker messageBroker)
            {
                  _messageBroker = messageBroker;
            }

            public void Publish(string channel, object message)
            {
                  _messageBroker.Publish(channel, message);
            }
      }

      // Subscriber class that implements the Subscribe and Unsubscribe methods.
      public class Subscriber
      {
            private readonly IMessageBroker _messageBroker;

            public Subscriber(IMessageBroker messageBroker)
            {
                  _messageBroker = messageBroker;
            }

            public void Subscribe(string channel, Action<object> callback)
            {
                  _messageBroker.Subscribe(channel, callback);
            }

            public void Unsubscribe(string channel, Action<object> callback)
            {
                  _messageBroker.Unsubscribe(channel, callback);
            }
      }

      // Implementation of the IMessageBroker interface that manages
      // the subscriptions and publishes messages to the subscribers.
      public class PubSub : IMessageBroker
      {
            private readonly Dictionary<string, List<Action<object>>> _subscriptions = new Dictionary<string, List<Action<object>>>();

            // Publish a message to a channel
            public void Publish(string channel, object message)
            {
                  if (_subscriptions.ContainsKey(channel))
                  {
                        foreach (var callback in _subscriptions[channel])
                        {
                              callback(message);
                        }
                  }
            }

            // Subscribe to a channel
            public void Subscribe(string channel, Action<object> callback)
            {
                  if (!_subscriptions.ContainsKey(channel))
                  {
                        _subscriptions[channel] = new List<Action<object>>();
                  }

                  _subscriptions[channel].Add(callback);
            }

            // Unsubscribe from a channel
            public void Unsubscribe(string channel, Action<object> callback)
            {
                  if (_subscriptions.ContainsKey(channel))
                  {
                        _subscriptions[channel].Remove(callback);
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