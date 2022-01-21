using ReactiveUI;
using SpectabisUI.Views;

namespace SpectabisUISpectabisUI.ViewModels
{
	public class MainWindowViewModel : ReactiveObject
	{
		public MainWindowViewModel(ModalDialog modalDialog)
		{
			dialog = modalDialog;
		}

		private ModalDialog dialog;

		public ModalDialog Dialog { get => dialog; set => this.RaiseAndSetIfChanged(ref dialog, value); }

		private bool modalVisible;

		public bool ModalVisible { get => modalVisible; set => this.RaiseAndSetIfChanged(ref modalVisible, value); }
	}
}