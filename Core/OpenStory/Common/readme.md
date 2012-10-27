# `OpenStory.Common`

This namespace mostly contains boring stuff that you can't go without.

## Credits

Credits go out to, and I never thought I'd say this, Java, for the `AtomicBoolean` class. My god, why does .NET not have `Interlocked` overloads for boolean values?

## Points of interest

### `IO` namespace

`OpenStory.Common.IO` contains the classes which handle little-endian reading and writing for packets. They are *very* well-documented, so I don't think there's much to explain about them.

### `Game` namespace

`OpenStory.Common.Game` contains game-related structures that have the same usage in both client and server endpoints.

### `Tools` namespace

`OpenStory.Common.Tools` contains various helper classes.