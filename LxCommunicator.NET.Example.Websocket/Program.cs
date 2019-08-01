using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loxone.Communicator;
using Loxone.Communicator.Events;

namespace LxCommunicator.NET.Example.Websocket {
	class Program {
		static WebsocketWebserviceClient client;

		static async Task Main(string[] args) {
			using (client = new WebsocketWebserviceClient("testminiserver.loxone.com", 7777, 2, "098802e1-02b4-603c-ffffeee000d80cfd", "LxCommunicator.NET.Websocket")) {
				using (TokenHandler handler = new TokenHandler(client, "app")) {
					handler.SetPassword("LoxLIVEpasswordTest");
					client.OnReceiveEventTable += Client_OnReceiveEventTable;
					client.OnAuthenticated += Client_OnAuthenticated;
					await client.Authenticate(handler);
					string result = (await client.SendWebservice(new WebserviceRequest<string>("jdev/sps/enablebinstatusupdate", EncryptionType.None))).Value;
					Console.ReadLine();
					await handler.KillToken();
				}
			}
		}

		private static void Client_OnAuthenticated(object sender, ConnectionAuthenticatedEventArgs e) {
			Console.WriteLine("Successfully authenticated!");
		}

		private static void Client_OnReceiveEventTable(object sender, EventStatesParsedEventArgs e) {
			foreach (EventState state in e.States) {
				Console.WriteLine(state.ToString());
			}
		}
	}
}
