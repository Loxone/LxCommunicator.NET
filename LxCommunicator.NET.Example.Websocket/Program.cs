using Loxone.Communicator;
using Loxone.Communicator.Events;
using System;
using System.Threading.Tasks;

namespace LxCommunicator.NET.Example.Websocket {
    internal class Program {
        private static WebsocketWebserviceClient client;

        private static async Task Main(string[] args) {
            using (client = new WebsocketWebserviceClient("testminiserver.loxone.com", 7777, 2, "098802e1-02b4-603c-ffffeee000d80cfd", "LxCommunicator.NET.Websocket")) {
                using (TokenHandler handler = new TokenHandler(client, "app")) {
                    handler.SetPassword("LoxLIVEpasswordTest");
                    client.OnReceiveEventTable += Client_OnReceiveEventTable;
                    client.OnAuthenticated += Client_OnAuthenticated;
					client.OnKeepalive += Client_OnKeepAlive;
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

		private static void Client_OnKeepAlive(object sender, KeepaliveEventArgs e) {
			Console.WriteLine(e.IsResponding ? "Got Keepalive!" : "Keepalive is missing!");
		}
	}
}