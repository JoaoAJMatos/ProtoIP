# Implementing secure protocols using ProtoCrypto

You can implement **secure protocols** using **ProtoCrypto**. This article will show you how to implement a **secure protocol** using **AES** and **RSA**. But first, you should be familiar with each of these **cryptographic primitives** and their **limitations**.

## Asymetric cryptography

**Asymmetric cryptography**, also known as **public-key cryptography**, is a type of cryptographic system that uses a pair of keys - a **public key** and a **private key** - to securely transmit and receive messages over an **unsecure network**. Some types of asymmetric cryptography include **RSA** and **Elliptic Curve Cryptography**.

As the name suggests, the **public key** is **publicly available** and can be **shared** with anyone. The **private key** is **secret** and **should not be shared**. Messages **encrypted with the public key** can only be decrypted **with the corresponding private key**, this ensures that only the intened recipient can read the message.

**But there's a catch**. Encrypting and decrypting messages with **asymmetric cryptography** is **slow** and **computationally expensive**. This is because asymetric algoritms typically involve complex mathematical operations, such as **modular exponentiation** and **prime factorization**.

Because of this, **asymmetric cryptography** is **not suitable** for **real-time applications** that require high-speed encryption and decryption of large amounts of data, such as **video streaming** or **online gaming**.

## Symmetric cryptography

**Symmetric cryptography** is a type of cryptography where the **same key** is used for both **encryption** and **decryption**. This means that the **key** must be **shared** between the two parties in some way. Some types of symmetric cryptography include **AES** and **DES**. This is the **easiest** and **fastest** type of cryptography, but it has **several limitations**.

The main limitation, is that the **key** must be **transmitted** through a possibly **unsecure channel**. This means that the **key** can be **intercepted** or **compromised** by a **third party**. This is why **symmetric cryptography** is **not secure** by itself.

## Best of both worlds

Symmetric and asymmetric cryptography are **complementary**. **Asymmetric cryptography** is **slow** and **computationally expensive**, but it is **secure**. **Symmetric cryptography** is **fast** and **computationally cheap**, but it is **not secure** by itself, since it relies heavily on the secrecy of the **shared key** and the security of the **key exchange** process.

To overcome these issues, we can use **both** types of cryptography together when implementing our protocols. For example, we can create a **secure channel** using **asymmetric cryptography** to **exchange a symmetric key**. This way, we can use **symmetric cryptography** to **encrypt** and **decrypt** our messages, without having to worry about the **key** being **compromised**.

## Implementing a secure protocol

In our **client-server application** we can implement a handshake protocol that uses **RSA** to securely exchange an *AES* key. Like so:

- The **client** generates a new **RSA key pair** and sends the **public key** to the **server**.
- The **server** generates a new **AES key** and **encrypts** it with the **client's public key**.
- The **server** sends the **encrypted AES key** to the **client**.
- The **client** **decrypts** the **AES key** with its **private key**.
- The **client** and **server** can now use the **AES key** to **encrypt** and **decrypt** their messages.
