namespace SpectabisUI.Interfaces
{
    public interface Page
    {
        string PageTitle { get; }

        //replace with "InTitlebar"
        bool ShowInTitlebar { get; }

        //replace with "Hides"
        bool HideTitlebar { get; }

        //replace with "Keep in memory after"
        bool ReloadOnNavigation { get; }

        void InitializeComponent();
    }
}