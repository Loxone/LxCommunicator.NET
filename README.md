# LxCommunicator.NET v0.9.0.0
This library exposes all necessary classes to establish a secure and encrypted connection to a Loxone Miniserver.
<br>
LxCommunicator can be installed using NuGet 

## Disclaimer
- Loxone Electronics GmbH doesn't provide any support for this library
- Please submit an issue or file an pull request if you find any issue

## Use LxCommunicator.NET
Use the namespace `Loxone.Communicator` and in case of handling events via websocket also `Loxone.Communicator.Events`
<br>
The libary manages the requiring and refreshing of token authentication. Storing, loading and killing of tokens needs to be implemented by the application layer.

#### Use `WecbsocketWebserviceClient`
The `WecbsocketWebserviceClient` class handles a websocket connection with a Loxone Miniserver including token authentication and encryption.
<br>
With this `WebserviceClient` implementation it is possible to receive live updates from the Loxone Miniserver via `StateEvents`
*Example:* `.\LxCommunicator.NET.Example.Http\LxCommunicator.NET.Example.Http.csproj`

#### Use `HttpWebserviceClient`
The `HttpWebserviceClient` class handles a Http webservice request with a Loxone Miniserver, including token authentication and encryption.
<br>
With this `WebserviceClient` implementation it is possible to receive live updates from the Loxone Miniserver via `StateEvents`
*Example:* `.\LxCommunicator.NET.Example.Http\LxCommunicator.NET.Example.Http.csproj`

> **Note**<br>Every Webservice that needs authentication is sent as encrypted webservice.<br>The Loxone Miniserver does not support encryption on every webservice, for example binary files or images are not supported.<br>For more detail please see our [API documetation](https://www.loxone.com/enen/kb/api/ ).
