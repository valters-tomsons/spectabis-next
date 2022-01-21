namespace SpectabisUI.Interfaces
{
    public interface IPage
    {
        string PageTitle { get; }
        bool ShowInTitlebar { get; }
        bool HideTitlebar { get; }
        bool ReloadOnNavigation { get; }

        void InitializeComponent();
    }
}