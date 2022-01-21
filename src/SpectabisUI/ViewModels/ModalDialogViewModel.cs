using System;
using ReactiveUI;

namespace SpectabisUISpectabisUI.ViewModels
{
    public class ModalDialogViewModel : ReactiveObject
    {
		private string bodyText;

        public string BodyText { get => bodyText; set => this.RaiseAndSetIfChanged(ref bodyText, value); }
    }
}