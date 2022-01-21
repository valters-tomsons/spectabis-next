using Common.Helpers;
using SpectabisLibSpectabisLib.Interfaces.Services;
using SpectabisUISpectabisUI.ViewModels;

namespace SpectabisNextSpectabisNext.Services
{
	public class ModalService : IModalService
    {
		private readonly MainWindowViewModel _mainViewModel;
		private readonly ModalDialogViewModel _modalViewModel;

		public ModalService(MainWindowViewModel mainViewModel, ModalDialogViewModel modalViewModel)
        {
			_mainViewModel = mainViewModel;
			_modalViewModel = modalViewModel;
		}

        public void DisplayModal(string text)
        {
            Logging.WriteLine("Trying to display modal");
            _modalViewModel.BodyText = text;
            _mainViewModel.ModalVisible = true;
        }
    }
}