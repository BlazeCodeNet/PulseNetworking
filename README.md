# PulseNetworking
This is a C# library intended for use with Video Games that rely entirely on TCP/IP networking. It is designed to make packing data for network transmit easier and less tedious!

## Quick Start Guide
This library supports both server and client sided networking
### Server Side
To use this library server-side, make sure to add a reference to PulseNet.
Next, create a instance of the PulseServer class. This constructor requires a Int32 that reperesents the port to listen on.
Make sure to subscribe to the following events to properly utilize this library:
* OnClientConnected
* OnClientDisconnected

Next, you must call the ```Start``` method on your instance of PulseServer.

When ```OnClientConnected``` is fired, you must apply handlers. Read more about this in the handlers section below!

### Client Side
Client side is VERY similar to server side. Simply create a new PulseClient class instance. This constructor requires a String server address and a Int32 server port.
You can subscribe to the following events to fully utilize this library:
* OnConnectionEstablished

Then simply call the ```Connect``` method from the PulseClient class's instance.

#### NOTE
ClientSide setup on Unity3D engine requires some additional steps. See more on this below

### Unity3D Client Side
Client side with Unity3D is different because unity3D does NOT support multi-thread access. To use this library with Unity3D you must subscribe to
the ```OnPacketRecieved``` event from the PulseClient class, then call the method ```EnableCustomHandled``` . This will allow you to, obviously, 
enable custom packet handling for the client. You will then have to manually handle the recieved packets, which are all in the form of a PulsePacket class, on the
MAIN unity thread through use of the ```void Update``` provided by Unity3D's MonoBehaviour. 

### Handlers
Both client and server side of PulseNet needs to have handlers. These handlers
are simply classes that extend from the ```PulseHandler``` class and override the ```Handle(PulsePacket, PulseClient)``` method. You can use the 
various methods from ```PulsePacket``` to handle the recieved data, such as:
* ReadString
* ReadInt
* ReadLong
* ReadBool
* And many more...

you must then apply the handlers to your server and/or client class instances. You do this by calling
the method ```RegisterHandler``` in etiher ```PulseClient``` or ```PulseServer``` instances you have.
The method takes a Int32 which is the packet identifier number(you must handle these yourself. Just pick a random number).
The second parameter for the method is a NEW instance of your class that extends PulseHandler.
