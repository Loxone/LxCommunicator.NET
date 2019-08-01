using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Communicator {
	/// <summary>
	/// Client to handle Webservices to Loxone Minsierver. Derive from <see cref="WebserviceClient"/> to implement your own Client
	/// </summary>
	public abstract class WebserviceClient : IDisposable {
		/// <summary>
		/// The ip of the miniserver
		/// </summary>
		internal string IP { get; set; }
		/// <summary>
		/// The port of the miniserver
		/// </summary>
		internal int Port { get; set; }
		/// <summary>
		/// The tokenhandler that should be used for managing the token
		/// </summary>
		internal TokenHandler TokenHandler { get; set; }
		/// <summary>
		/// The session object used for storing information about the connection
		/// </summary>
		public Session Session { get; internal set; }

		/// <summary>
		/// Establish an authenticated connection to the miniserver
		/// </summary>
		/// <param name="handler">The tokenhandler that should be used</param>
		public abstract Task Authenticate(TokenHandler handler);

		/// <summary>
		/// Sends a webservice to the miniserver
		/// </summary>
		/// <typeparam name="T">The object that should be returned in Value</typeparam>
		/// <param name="request">The Request that should be sent</param>
		/// <returns>The Response the miniserver returns</returns>
		public async Task<WebserviceContent<T>> SendWebservice<T>(WebserviceRequest<T> request) {
			return (await SendWebservice((WebserviceRequest)request))?.GetAsWebserviceContent<T>();
		}

		/// <summary>
		/// Sends a webservice to the miniserver
		/// </summary>
		/// <param name="request">The Request that should be sent</param>
		/// <returns>The Response the miniserver returns</returns>
		public virtual async Task<WebserviceResponse> SendWebservice(WebserviceRequest request) {
			return await Task.FromResult<WebserviceResponse>(null);
		}

		/// <summary>
		/// Disposes the WebserviceClient
		/// </summary>
		public virtual void Dispose() {
			TokenHandler?.Dispose();
		}
	}
}
