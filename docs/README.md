# ProtoIP <small>1.0.0</small>

> From students to students - Instituto Politécnico de Leiria - [ESTG](https://www.ipleiria.pt/estg/)

**ProtoIP** is an open source alternative to **ProtocolSI**, offering a simple and up-to-date interface for introducing students to network programming and cryptography essentials in **C#**. Provides protocol definitions and a high level boilerplate codebase for school projects.

> **From the creator**: "First, *ProtoIP* teaches you how to drive, and then gives you tools to understand how the engine works. Not the other way around."

## Features

- **ProtoStream**: Send and receive large chunks of data in a single function call.
- **Packet Manipulation**: Low level access to packets and protocol definitions.
- **Protocol Definition**: Define your own protocols and use them in your applications using higher level Packet abstractions.
- **Cryptography**: Send and receive encrypted data using symmetric and asymmetric encryption.
- **Client-Server**: Implement your client-server application logic with events.
- **Peer-to-Peer**: Basic definitions for peer-to-peer applications.
- **PubSub**: Pub/Seb messaging pattern through a simple API.
- **Utilities**: File system utilities, compression, and more.

## Motivation

In class I noticed that most students were struggling to grasp the concepts of network programming and cryptography the way they were being taught in class. We often had to spend a lot of time writing boilerplate code and implementing everything from scratch.

This was a huge waste of time and energy, and it was also very frustrating for the students; as the codebase was often very messy, filled with comments and hard to understand.

Furthermore, we were provided with a library (*ProtocolSI*) with archaic documentation and a very limited set of features. In the end, we would end up with the same messy codebase, but this time, with an extra abstraction layer; which unexpectedly made things even worse.

To combat this, I decided to implement my own library with the sole purpose of providing an easy to understand, easy to use, and self-documenting codebase for students to use in their projects.

Hiding the complexity of the underlying protocols and communication security with high-level abstractions, while still providing low-level access to the actual packets and protocol definitions.

**ProtoIP** is not just a library, it's a tool that will hopefully guide you through the journey of **Network Programming**.

## Official Remote Repository

You can find the official remote repository hosted on GitHub [here](https://github.com/JoaoAJMatos/ProtoIP).

### Contribuitons

By supporting open-source projects you encourage the community to continue to develop cutting-edge software that empowers you to build the next generation of technology.

Please consider [donating](https://github.com/sponsors/JoaoAJMatos) to help maintain the project.

## Authors

- **João Matos** - *Initial work* - [JoaoAJMatos](https://github.com/JoaoAJMatos)
