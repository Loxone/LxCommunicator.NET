using Loxone.Communicator.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loxone.Communicator {
	/// <summary>
	/// EventArgs for an event thrown at a received message
	/// </summary>
	public class MessageReceivedEventArgs : EventArgs {
		/// <summary>
		/// The received Message
		/// </summary>
		public WebserviceResponse Response { get; }

		/// <summary>
		/// Initialises the eventArgs
		/// </summary>
		/// <param name="response">The received Message</param>
		public MessageReceivedEventArgs(WebserviceResponse response) {
			Response = response;
		}
	}

	/// <summary>
	/// EventArgs for an event thrown at received valueStates
	/// </summary>
	public class EventStatesParsedEventArgs : EventArgs {
		/// <summary>
		/// The type of the received message/event
		/// </summary>
		public MessageType Type { get; }

		/// <summary>
		/// IEnumerable containing the acutal states
		/// </summary>
		public IEnumerable<EventState> States { get; }

		/// <summary>
		/// Initialises the eventArgs
		/// </summary>
		/// <param name="type">The received messageType</param>
		/// <param name="states">the actual states</param>
		public EventStatesParsedEventArgs(MessageType type, IEnumerable<EventState> states) {
			Type = type;
			States = states;
		}
	}

	/// <summary>
	/// EventArgs for an event thrown at authentication
	/// </summary>
	public class ConnectionAuthenticatedEventArgs : EventArgs {
		/// <summary>
		/// The tokenHandler containing the used token
		/// </summary>
		public TokenHandler TokenHandler;

		/// <summary>
		/// Initialises the eventArgs
		/// </summary>
		/// <param name="handler">The tokenHandler used for authentication</param>
		public ConnectionAuthenticatedEventArgs(TokenHandler handler) {
			TokenHandler = handler;
		}
	}
}
