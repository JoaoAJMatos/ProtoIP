# ProtoIP

> From students to students - Instituto Politécnico de Leiria

**ProtoIP** is an open source alternative to **ProtocolSI**, offering a simple and up-to-date interface for introducing students to network programming and cryptography essentials in **C#**.

## Features

- **ProtoStream**: Send and receive large chunks of data in a single function call.
- **Packet Manipulation**: Low level access to packets and protocol definitions.
- **Protocol Definition**: Define your own protocols and use them in your application.
- **Cryptography**: Send and receive encrypted data using **RSA** or **AES**.
- **Client-Server Definitions**: Basic definitions for client-server applications.
- **Peer-to-Peer**: Basic definitions for peer-to-peer applications.

## Getting Started

### Prerequisites

- **.NET Framework 4.5** or higher
- **Visual Studio 2015** or higher

### Installing

- Download the latest release from the [releases page](https://github.com/JoaoAJMatos/ProtoIP/releases).
- Add the **ProtoIP.dll** to your project.
- Add the **ProtoIP** namespace to your code:

```csharp
using ProtoIP;
```

### Building from source

Alternatively, you can build the project from source:

```bash
git clone https://github.com/JoaoAJMatos/ProtoIP.git
cd ProtoIP
./build.sh
```

The `.dll` file will be located inside the `build` folder.

## Documentation

The documentation is available [here](https://joaoajmatos.github.io/ProtoIP/#/).

## Examples

Check out the [examples](/examples/) folder for some examples on how to use **ProtoIP**.

## Contributing

Please read [CONTRIBUTING.md](/CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests.

## Authors

- **João Matos** - *Initial work* - [JoaoAJMatos](https://github.com/JoaoAJMatos)

See also the list of [contributors]().

## License

This project is licensed under the MIT License - see the [LICENSE.md](/LICENSE.md) file for details.

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)
