using System;
using System.Collections.Generic;
using System.Reactive;
using EmuConfig.Enums;
using ReactiveUI;
using SpectabisLib.Interfaces.Controllers;

namespace SpectabisUI.ViewModels
{
    public class GameSettingsViewModel : ReactiveObject
    {
        public GameSettingsViewModel(IGameSettingsController _controller)
        {
            LaunchPCSX2 = ReactiveCommand.CreateFromTask(async () => await _controller.LaunchConfiguration(Id));
        }

        private bool showsettings;
        private Guid guid;
        private string title;
        private bool fullscreen;
        private string resolution;

        public Guid Id { get => guid; set => this.RaiseAndSetIfChanged(ref guid, value); }
        public string Title { get => title; set => this.RaiseAndSetIfChanged(ref title, value); }
        public bool Fullscreen { get => fullscreen; set => this.RaiseAndSetIfChanged(ref fullscreen, value); }
        public string Resolution { get => resolution; set => this.RaiseAndSetIfChanged(ref resolution, value); }

        public bool ShowSettings { get => showsettings; set => this.RaiseAndSetIfChanged(ref showsettings, value); }
        public static IEnumerable<string> Resolutions { get => Enum.GetNames(typeof(UpscaleFactor)); }

        public ReactiveCommand<Unit, Unit> LaunchPCSX2 { get; }
    }
}