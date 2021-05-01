using System;
using ReactiveUI;

namespace SpectabisUI.ViewModels
{
    public class GameSettingsViewModel : ReactiveObject
    {
        private Guid guid;
        private string title;
        private bool fullscreen;

        public Guid Id { get => guid; set => this.RaiseAndSetIfChanged(ref guid, value); }
        public string Title { get => title; set => this.RaiseAndSetIfChanged(ref title, value); }
        public bool Fullscreen { get => fullscreen; set => this.RaiseAndSetIfChanged(ref fullscreen, value); }
    }
}