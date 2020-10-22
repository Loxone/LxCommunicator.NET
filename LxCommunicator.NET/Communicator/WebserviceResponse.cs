using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Loxone.Communicator {
	/// <summary>
	/// A container for a received Webservice
	/// </summary>
	public class WebserviceContainer {
		/// <summary>
		/// The content of the received Webservice
		/// </summary>
		[JsonProperty("LL")]
		public virtual WebserviceContent Response { get; set; }
	}
	/// <summary>
	/// A generic container for a received Webservice
	/// </summary>
	/// <typeparam name="T">The type of the value contained in the response</typeparam>
	public class WebserviceContainer<T> {
		/// <summary>
		/// The content of the received Webservice
		/// </summary>
		[JsonProperty("LL")]
		public WebserviceContent<T> Response { get; set; }
	}

	/// <summary>
	/// A container for the content received in webservices
	/// </summary>
	public class WebserviceContent {
		/// <summary>
		/// The control (command) which was sent to the miniserver
		/// </summary>
		[JsonProperty("control")]
		public string Control { get; set; }
		/// <summary>
		/// Http sttus code if sending and receiving the webservice succeeded
		/// </summary>
		[JsonProperty("Code")]
		public HttpStatusCode Code { get; set; }
	}
	/// <summary>
	/// A generic container for the content received in webservices
	/// </summary>
	/// <typeparam name="T">The type of the requested value</typeparam>
	public class WebserviceContent<T> : WebserviceContent {
		/// <summary>
		/// The value (answer) received from the miniserver
		/// </summary>
		[JsonProperty("value")]
		public T Value { get; set; }
	}

	/// <summary>
	/// A container for the messageHeader of each webservice
	/// </summary>
	public class MessageHeader {
		/// <summary>
		/// The messageType (e.g. textMessage, valueStateEvent, ...) of the received message
		/// </summary>
		public MessageType Type { get; private set; }
		/// <summary>
		/// Whether or not another header is sent by the miniserver before the actual message.
		/// </summary>
		public bool Estimated { get; private set; }
		/// <summary>
		/// The lenght of the received message
		/// </summary>
		public uint Length { get; private set; }

		public bool IsEventMessage {
			get {
				switch (Type) {
					case MessageType.EventTableValueStates:
					case MessageType.EventTableTextStates:
					case MessageType.EventTableDaytimerStates:
					case MessageType.EventTableWeatherStates:
						return true;
					default:
						return false;
				}
			}
		}

		/// <summary>
		/// Tries to parse received bytes into a message header.
		/// </summary>
		/// <param name="bytes">The bytes that should be parsed</param>
		/// <param name="header">The header that was parsed, if successful</param>
		/// <returns>Whether or not parsing the header succeeded</returns>
		public static bool TryParse(byte[] bytes, out MessageHeader header) {
			header = null;
			if (bytes == null || bytes.Length != 8 || bytes[0] != 3) {
				return false;
			}
			try {
				header = new MessageHeader() {
					Type = (MessageType)bytes[1],
					Length = BitConverter.ToUInt32(bytes, 4),
					Estimated = (bytes[2] & (byte)128) == 128
				};
				return true;
			}
			catch (Exception ex) {
				throw new WebserviceException("Parse MessageHeader failed: " + ex.Message, ex);
			}
		}
	}

	/// <summary>
	/// A container for a response received by the miniserver.
	/// </summary>
	public class WebserviceResponse {
		/// <summary>
		/// The header of the message. Contains information about the type and the length of the message.
		/// </summary>
		public MessageHeader Header { get; }
		/// <summary>
		/// The actual message received by the miniserver.
		/// </summary>
		public byte[] Content { get; }
		/// <summary>
		/// The error / success code the webserviceClient returned
		/// </summary>
		public int? ClientCode { get; }
		/// <summary>
		/// Initialises a new webserviceResponse
		/// </summary>
		/// <param name="header">The header of the message</param>
		/// <param name="content">The content of the message</param>
		/// <param name="clientCode">The error / success code the webserviceClient returned</param>
		public WebserviceResponse(MessageHeader header, byte[] content, int? clientCode) {
			Header = header;
			Content = content;
			ClientCode = clientCode;
		}

		/// <summary>
		/// Get the webserviceResponse as webserviceContent
		/// </summary>
		/// <returns>The webserviceContent (without value!)</returns>
		public WebserviceContent GetAsWebserviceContent() {
			try {
				string content = GetAsStringContent();
				return JsonConvert.DeserializeObject<WebserviceContainer>(content).Response;
			}
			catch {
				return null;
			}
		}

		/// <summary>
		/// Get the webserviceResponse as webserviceContent
		/// </summary>
		/// <typeparam name="T">The type of the value</typeparam>
		/// <returns>The webserviceContent (with value)</returns>
		public WebserviceContent<T> GetAsWebserviceContent<T>() {
			try {
				string content = GetAsStringContent();
				return JsonConvert.DeserializeObject<WebserviceContainer<T>>(content).Response;
			}
			catch {
				return null;
			}
		}

		/// <summary>
		/// Get the webserviceResponse as text
		/// </summary>
		/// <returns>the text containing the response</returns>
		public string GetAsStringContent() {
			return Encoding.UTF8.GetString(Content);
		}
		/// <summary>
		/// Get the webserviceResponse as text
		/// </summary>
		/// <returns>The text containing the response</returns>
		public override string ToString() {
			return GetAsStringContent();
		}
	}
	/// <summary>
	/// Possible types for a message
	/// </summary>
	public enum MessageType : byte {
		Text = 0,
		Binary = 1,
		EventTableValueStates = 2,
		EventTableTextStates = 3,
		EventTableDaytimerStates = 4,
		OutOfService = 5,
		Keepalive = 6,
		EventTableWeatherStates = 7
	}
	/// <summary>
	/// A container for the reponse received when authenticating to the miniserver with a token via websocket
	/// </summary>
	public class AuthResponse {
		/// <summary>
		/// Determines how long the used token is valid.
		/// </summary>
		[JsonProperty("validUntil")]
		public int ValidUntil { get; set; }
		/// <summary>
		/// The rights the current token provides to the user
		/// </summary>
		[JsonProperty("tokenRights")]
		public int TokenRights { get; set; }
		/// <summary>
		/// Determines whether the password of the current user if secured or not.
		/// </summary>
		[JsonProperty("unsecurePass")]
		public bool UnsecurePass { get; set; }
	}

}
