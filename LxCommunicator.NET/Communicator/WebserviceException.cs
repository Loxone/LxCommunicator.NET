using System;
using System.Collections.Generic;
using System.Text;

namespace Loxone.Communicator {
	/// <summary>
	/// Exception that indicates an error in a webservice.
	/// </summary>
	public class WebserviceException : Exception {
		/// <summary>
		/// The response from communicating with the webservice
		/// </summary>
		public WebserviceResponse Response { get; }
		/// <summary>
		/// Throws a new WebserviceException
		/// </summary>
		public WebserviceException() { }
		/// <summary>
		/// Throws a new WebserviceException
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="response">the response that caused the error</param>
		public WebserviceException(string message, WebserviceResponse response) : base(message) {
			Response = response;
		}
		/// <summary>
		/// Throws a new WebserviceException
		/// </summary>
		/// <param name="message">The error message</param>
		public WebserviceException(string message) : base(message) { }
		/// <summary>
		/// Throws a new WebserviceException
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="inner">An inner exception</param>
		public WebserviceException(string message, Exception inner) : base(message, inner) { }
		protected WebserviceException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
