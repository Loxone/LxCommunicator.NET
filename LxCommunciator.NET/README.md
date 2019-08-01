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

#### Use WecbsocketWebserviceClient
*Example:* `.\LxCommunicator.NET.Example.Http\LxCommunicator.NET.Example.Http.csproj`

#### Use HttpWebserviceClient
*Example:* `.\LxCommunicator.NET.Example.Http\LxCommunicator.NET.Example.Http.csproj`

> **Note for Http**<br>Every Webservice that needs authentication is sent as encrypted webservice.<br>Not all Webservices are supporting encryption, for example binary files.
