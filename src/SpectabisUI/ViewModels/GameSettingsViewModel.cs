using ReactiveUI;

namespace SpectabisUI.ViewModels
{
    public class GameSettingsViewModel : ReactiveObject
    {
        private string title;

        public string Title { get => title; set => this.RaiseAndSetIfChanged(ref title, value); }
    }
}