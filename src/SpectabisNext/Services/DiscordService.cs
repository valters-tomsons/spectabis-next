using System;
using System.Threading;
using DiscordRPC;
using Common.Helpers;
using SpectabisLib;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisNext.Services
{
	public class DiscordService : IDiscordService
    {
        private static readonly ulong StartTime = (ulong) DateTimeOffset.Now.ToUnixTimeMilliseconds();
        private static readonly SemaphoreSlim ClientInitBarrier = new(1);

        private readonly IConfigurationManager _configLoader;
        private static DiscordRpcClient _client;

        public DiscordService(IConfigurationManager configLoader)
        {
            _configLoader = configLoader;
            InitializeDiscordClient();
        }

        public void SetMenuPresence()
        {
            SetStatus("Library");
        }

        public void SetGamePresence(GameProfile game)
        {
			if (game != null)
            {
                SetStatus(game.Title);
            }
        }

        private void SetStatus(string status)
        {
			if (!_configLoader.Spectabis.EnableDiscordRichPresence)
            {
                return;
            }

			if (_client == null)
            {
                Logging.WriteLine($"Discord client not active, ignoring status '{status}'");
                return;
            }

            // clientSemaphore.Wait();

            Logging.WriteLine($"Updating status to: '{status}'");

			var presence = new RichPresence()
			{
				Details = status,
				Timestamps = new Timestamps()
				{
					StartUnixMilliseconds = StartTime
				},
				Assets = new Assets()
				{
					LargeImageKey = "menus",
				}
			};

            _client.SetPresence(presence);
        }

        private void InitializeDiscordClient()
        {
			if (!_configLoader.Spectabis.EnableDiscordRichPresence)
            {
                return;
            }

            ClientInitBarrier.Wait();

			if (_client != null)
            {
				ClientInitBarrier.Release();
                return;
            }

            Logging.WriteLine("Creating DiscordRPC client");

			try
			{
				_client = new DiscordRpcClient(Constants.DiscordClientId);
				var initialized = _client.Initialize();

				if (initialized && _client.IsInitialized)
				{
					SetMenuPresence();
				}
			}
			catch (Exception e)
			{
				Logging.WriteLine("Could not connect to Discord RPC");
				Logging.WriteLine(e.Message);
			}
			finally
			{
				ClientInitBarrier.Release();
			}
        }

        ~DiscordService()
        {
            _client.Dispose();
        }
    }
}