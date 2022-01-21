using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisUISpectabisUI.ViewModels;

namespace SpectabisUI.Views
{
	public class ModalDialog : UserControl
    {
		private readonly ModalDialogViewModel _viewModel;

		[Obsolete("XAMLIL placeholder", true)]
        public ModalDialog()
        {
        }

        public ModalDialog(ModalDialogViewModel viewModel)
        {
            AvaloniaXamlLoader.Load(this);

			_viewModel = viewModel;
            DataContext = _viewModel;
		}
	}
}