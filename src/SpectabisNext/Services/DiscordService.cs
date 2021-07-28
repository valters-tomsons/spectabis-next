using System.Threading;
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

        private readonly IConfigurationManager _configLoader;
        private static DiscordRpcClient _client;

        private static readonly SemaphoreSlim clientSemaphore = new(1);

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

            clientSemaphore.Wait();

            if(_client != null)
            {
                clientSemaphore.Release();
                return;
            }

            Logging.WriteLine("Starting DiscordService client");

            _client = new DiscordRpcClient(Constants.DiscordClientId);

            var initialized = _client.Initialize();

            if(!initialized)
            {
                Logging.WriteLine("Could not connect to Discord RPC");

                _client.Dispose();
                clientSemaphore.Release();

                return;
            }

            clientSemaphore.Release();
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

            if(_client == null)
            {
                InitializeDiscord();
            }

            if(_client?.IsDisposed != false)
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

            _client.SetPresence(presence);
        }

        ~DiscordService()
        {
            _client.Dispose();
        }
    }
}