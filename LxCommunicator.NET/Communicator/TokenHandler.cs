using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Loxone.Communicator {
	/// <summary>
	/// A handler used for managing the Token. Can create, renew and kill tokens.
	/// When enabled, the <see cref="TokenHandler"/> also updates the token automatically
	/// </summary>
	public class TokenHandler : IDisposable {
		/// <summary>
		/// The webserviceClient used for communication with the miniserver
		/// </summary>
		public WebserviceClient WsClient { get; private set; }

		/// <summary>
		/// The token used for authentication
		/// </summary>
		public Token Token { get; private set; }

		/// <summary>
		/// The cancellationSource for cancelling autoRenew
		/// </summary>
		private readonly CancellationTokenSource CancellationSource = new CancellationTokenSource();

		/// <summary>
		/// The username of the current user
		/// </summary>
		public string Username { get; private set; }

		/// <summary>
		/// The password f the current user
		/// </summary>
		private string Password { get; set; }

		/// <summary>
		/// Whether the can tokenHandler renew the token automatically
		/// </summary>
		private bool NeedRenewToken = true;

		/// <summary>
		/// Whether the tokenHandler is allowed to renew the token automatically
		/// </summary>
		public bool CanRenewToken {
			get => NeedRenewToken;
			set {
				NeedRenewToken = value;
				RenewTokenOrScheduleIfNeeded().Wait();
			}
		}

		/// <summary>
		/// Event, fired when the token updates.
		/// Contains the tokenHandler with the updated token in the eventArgs
		/// </summary>
		public event EventHandler<ConnectionAuthenticatedEventArgs> OnUpdateToken;

		/// <summary>
		/// Initialises a new tokenHandler object.
		/// </summary>
		/// <param name="client">The webserviceClient that should be used for communication</param>
		/// <param name="user">The username of the user</param>
		/// <param name="token">The token object that should be used (optional)</param>
		/// <param name="canRenewToken">Whether or not the tokenHandler should be allowed to renew the token automatically (true if not set!)</param>
		public TokenHandler(WebserviceClient client, string user, Token token = null, bool canRenewToken = true) {
			WsClient = client;
			Username = user;
			Token = token;
			NeedRenewToken = canRenewToken;
			RenewTokenOrScheduleIfNeeded().Wait();
		}

		/// <summary>
		/// Disposes the current TokenHandler
		/// </summary>
		public void Dispose() {
			CancellationSource.Cancel();
		}

		/// <summary>
		/// Sets the password required for authentication
		/// </summary>
		/// <param name="password">The password</param>
		public void SetPassword(string password) {
			Password = password;
		}

		/// <summary>
		/// Checks if the token needs to be renewed and renews it if needed.
		/// </summary>
		private async Task RenewTokenOrScheduleIfNeeded() {
			CancellationSource.Cancel();
			if (Token != null && NeedRenewToken) {
				double seconds = (Token.ValidUntil - DateTime.Now).TotalSeconds * 0.9;
				if (seconds < 0) {
					await RenewToken();
				}
				else {
					Task task = Task.Delay(Convert.ToInt32(Math.Min(seconds * 1000, int.MaxValue)), CancellationSource.Token).ContinueWith(async (t) => {
						await RenewTokenOrScheduleIfNeeded();
					}, CancellationSource.Token);
				}
			}
		}

		/// <summary>
		/// Request a new Token from the Miniserver
		/// </summary>
		/// <returns>Wheter acquiring the new Token succeeded or not</returns>
		public async Task<bool> RequestNewToken() {
			if (Password == null) {
				throw new WebserviceException("Password is not set!");
			}

			UserKey userKey = await WsClient.Session.GetUserKey(Username);
			HashAlgorithm sha = userKey.GetHashAlgorithm();
			HMAC hmacSha = userKey.GetHMAC();
			string pwHash = Cryptography.GetHexFromByteArray(sha.ComputeHash(Encoding.UTF8.GetBytes($"{Password}:{userKey.Salt}"))).ToUpper();
			hmacSha.Key = Cryptography.GetByteArrayFromHex(userKey.Key);
			string hash = Cryptography.GetHexFromByteArray(hmacSha.ComputeHash(Encoding.UTF8.GetBytes($"{Username}:{pwHash}")));
			Token = (await WsClient.SendWebservice(new WebserviceRequest<Token>($"jdev/sys/getjwt/{hash}/{Username}/{WsClient.Session.TokenPermission}/{WsClient.Session.DeviceUuid}/{WsClient.Session.DeviceInfo}", EncryptionType.RequestAndResponse) { NeedAuthentication = false })).Value;
			await RenewTokenOrScheduleIfNeeded();
			if (Token != null && Token.JsonWebToken != default && Token.Key != default && Token.ValidUntil != default) {
				return true;
			}
			else {
				return false;
			}
		}

		/// <summary>
		/// Renew the current Token
		/// </summary>
		public async Task RenewToken() {
			if (WsClient is HttpWebserviceClient) {
				throw new WebserviceException("Renewing Tokens is not supported with HTTP!");
			}
			string hash = await GetTokenHash();
			Token = (await WsClient.SendWebservice(new WebserviceRequest<Token>($"jdev/sys/refreshjwt/{hash}/{Username}", EncryptionType.RequestAndResponse))).Value;
			OnUpdateToken?.Invoke(this, new ConnectionAuthenticatedEventArgs(this));
			await RenewTokenOrScheduleIfNeeded();
		}

		/// <summary>
		/// Kill the current Token
		/// </summary>
		public async Task KillToken() {
			string hash = await GetTokenHash();
			try {
				await WsClient.SendWebservice(new WebserviceRequest<object>($"jdev/sys/killtoken/{hash}/{Username}", EncryptionType.RequestAndResponse) { Timeout = 0 });
			}
			catch {
			}
			Token = null;
			CancellationSource.Cancel();
		}

		/// <summary>
		/// Gets the tokenHash required for authentication
		/// </summary>
		/// <returns></returns>
		public async Task<string> GetTokenHash() {
			UserKey userKey = await WsClient.Session.GetUserKey(Username);
			HMAC hmac = userKey.GetHMAC();
			hmac.Key = Cryptography.GetByteArrayFromHex(userKey.Key);
			return Cryptography.GetHexFromByteArray(hmac.ComputeHash(Encoding.UTF8.GetBytes(Token.JsonWebToken))).ToLower();
		}
	}
}