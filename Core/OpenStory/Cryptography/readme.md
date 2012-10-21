# `OpenStory.Cryptography`

This is the cryptography component of OpenStory. Duh.

## Credits

Thanks go out to the people who ripped out the IV shuffle routine and cryptographic keys from the MapleStory binaries. Personal thanks to @Diamondo25 for his KMST stuff <3

## Points of interest

### `CryptoTransformBase` class

This class contains half of the cryptography logic. 

It contains the following variables:
1. `table`, which accepts a 256-byte array used for the shuffle transformations;
2. `initialValue`, which accepts a 4-byte array used as the initial vector for the shuffle transformation);

It uses mostly two methods, `ShuffleIv` and `TransformArraySegment`. 
The classes derived from this one can contain custom cryptographic transformation code.

### `RollingIv` class

This class stores the rolling IV for a session. It spends most of its time hanging out, but when it's doing work, it's checking packet headers for validity. Otherwise, it lets a `ICryptoAlgorithm` instance do most of the work.

### `RollingIvFactory` class

This class stores a pair of `ICryptoAlgorithm` instances and a version, so they can be reused over a bunch of `RollingIv` instances. This is useful because the `ICryptoAlgorithm` objects and the version don't change at all, so it saves some complexity when creating new `RollingIv`s.

### `CustomEncryption` class

This class does the other half of the cryptography. It uses some bit magic to encrypt and decrypt packets. IT"S MAGIC!

### `EndpointCrypto`, `ServerCrypto` and `ClientCrypto` classes

These are useful for keeping the related encryption-decryption pair of `RollingIv` instances in one place, so they can be used to more easily handle packet encryption and decryption *depending on the endpoint*.

### `IvFactories` class

This class contains `RollingIvFactory` factory methods. Mostly?
