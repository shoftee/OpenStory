# OpenStory Core

Hello.

This folder contains the endpoint-invariant components of the library. Or in words niblets can understand, this means the code here can (and *should*) be used by clients and servers alike.

# Namespaces

## `Common`

`OpenStory.Common` contains logic that is endpoint-invariant for MapleStory, and also other utility classes and methods which are used across the OpenStory project. In other words, mostly boilerplate stuff. It tends to be riddled with parameter checking and XML documentation. If you find a place to add more of those, do so with pride!

## `Cryptography`

`OpenStory.Cryptography` contains cryptography logic... duh. Go check its readme for the details.

## `Networking`

`OpenStory.Networking` contains network handling logic for the socket accept process, and packet sending and receiving. It is written using the asynchronous execution pattern (`BeginReceive`, `EndReceive`, `BeginSend`, `EndSend`, etc.), it is however *not* multi-threaded.

## `Synchronization`

`OpenStory.Synchronization` contains logic related to scheduling the execution of... stuff.

