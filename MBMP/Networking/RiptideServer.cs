using System;
using RiptideNetworking;
using RiptideNetworking.Transports.RudpTransport;
using BepInEx;

namespace MBMP
{
	public class RiptideServer : BaseUnityPlugin
	{
		private Server server;
		public bool IsListening => server.IsRunning;
		public string ListeningAddress => "Your server address here";

		public delegate void MessageReceivedHandler(ushort fromClientId, Message message);

		public RiptideServer(string address, ushort port, MessageReceivedHandler onMessageReceived)
		{
			server = new Server();
			server.ClientConnected += NewClientConnected;
			server.ClientDisconnected += ClientDisconnected;
			server.Start(port, 2);
			server.MessageReceived += (sender, e) => onMessageReceived(e.FromClientId, e.Message);
		}

		private void NewClientConnected(object sender, ServerClientConnectedEventArgs e)
		{
			Logger.LogInfo($"Client connected with ID: {e.Client.Id}");
		}

		private void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
		{
			Logger.LogInfo($"Client disconnected with ID: {e.Id}");
		}

		public void Update()
		{
			server.Tick();
		}

		public void Stop()
		{
			server.Stop();
		}
	}
}
