using System;
using DiscordRPC;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisLib.Helpers;
using SpectabisLib;

namespace SpectabisNext.Services
{
    public class DiscordService : IDiscordService
    {
        private static readonly ulong StartTime = (ulong) DateTimeOffset.Now.ToUnixTimeMilliseconds();
        private static DiscordRpcClient client;
        private readonly IConfigurationManager _configLoader;

        public DiscordService(IConfigurationManager configLoader)
        {
            _configLoader = configLoader;
        }

        public void InitializeDiscord()
        {
            if(!_configLoader.Spectabis.EnableDiscordRichPresence)
            {
                return;
            }

            Logging.WriteLine("Starting DiscordService client");

            client = new DiscordRpcClient(Constants.DiscordClientId);

            client.Initialize();

            if(client.CurrentUser == null)
            {
                Logging.WriteLine("Could not connect to Discord RPC");
                client.Dispose();
                return;
            }

            SetMenuPresence();
        }

        public void SetMenuPresence()
        {
            SetStatus("Library");
        }

        public void SetGamePresence(GameProfile game)
        {
            if(game != null)
            {
                SetStatus(game.Title);
            }
        }

        private void SetStatus(string status)
        {
            if(!_configLoader.Spectabis.EnableDiscordRichPresence)
            {
                return;
            }

            if(client == null)
            {
                InitializeDiscord();
            }

            if(client?.IsDisposed != false)
            {
                Logging.WriteLine($"Discord client not active, ignoring status '{status}'");
                return;
            }

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

            client.SetPresence(presence);
        }

        ~DiscordService()
        {
            client.Dispose();
        }
    }
}