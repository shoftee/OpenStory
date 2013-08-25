# `OpenStory.Networking`

This is the networking component of OpenStory. Duh.

## Credits

This is roughly inspired by Astaelan's Chronicle project. People who have seen it will notice the usage of the word "Descriptor".

## Points of interest

### `NetworkSession` and `EncryptedNetworkSession` classes

These are the classes which handle the sending and receiving of packet data. `EncryptedNetworkSession` is a decorator around `NetworkSession`, which does the encryption and decryption along with the rest.

### `SendDescriptor` class

This is a nifty class which handles outbound packets. Asynchronously. You can find interesting notes in the code itself.

### `ReceiveDescriptor` class

This is a nifty class which handles inbound packets. Asynchronously. You can find interesting notes in the code itself.