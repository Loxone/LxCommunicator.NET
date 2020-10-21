using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loxone.Communicator {
	public class Token {
		/// <summary>
		/// The tokenString, used for authentication to the miniserver
		/// </summary>
		[JsonProperty("token")]
		public string JsonWebToken { get; private set; }
		/// <summary>
		/// The key of the token
		/// </summary>
		[JsonProperty("key")]
		public string Key { get; private set; }
		/// <summary>
		/// The dateTime the token is valid until
		/// </summary>
		[JsonProperty("validUntil")]
		[JsonConverter(typeof(LxDateTimeConverter))]
		public DateTime ValidUntil { get; private set; }
		/// <summary>
		/// The current permissions the token has
		/// </summary>
		[JsonProperty("tokenRights")]
		public int TokenRights { get; private set; }
		/// <summary>
		/// Flag, wheter the user has a safe password or not
		/// </summary>
		[JsonProperty("unsecurePass")]
		public bool UnsecurePass { get; private set; }

		/// <summary>
		/// Initialises a new token object used for authentication.
		/// </summary>
		/// <param name="token">The tokenString itself</param>
		/// <param name="key">The key of the token</param>
		/// <param name="validUntil">The dateTime the token is valid until</param>
		/// <param name="tokenRights">The current rights the token has</param>
		/// <param name="unsecurePass">Wheter the user has a safe password or not</param>
		public Token(string token, string key, DateTime validUntil, int tokenRights, bool unsecurePass) {
			JsonWebToken = token;
			Key = key;
			ValidUntil = validUntil;
			TokenRights = tokenRights;
			UnsecurePass = unsecurePass;
		}
	}
}
