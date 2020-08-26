using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using SpectabisLib.Helpers;
using SpectabisNext.Controls.PageIcon;
using SpectabisNext.Pages;
using SpectabisUI.Interfaces;
using SpectabisLib.Repositories;
using SpectabisLib.Interfaces;
using ServiceClient.Interfaces;
using Avalonia.Input;

namespace SpectabisNext.Views
{
    public class MainWindow : Window
    {
        private readonly IConfigurationLoader _configuration;
        private readonly IPageNavigationProvider _navigationProvider;
        private readonly CancellationTokenRepository _cancelRepo;
        private readonly IGameLauncher _gameLauncher;
        private readonly IDiscordService _discordService;
        private readonly ITelemetry _telemetry;
        private readonly INotificationService _notify;

        private Rectangle Titlebar;
        private StackPanel TitlebarPanel;
        private ContentControl ContentContainer;
        private Grid NotificationPanel;
        private Rectangle NotificationPanelBackground;
        private Grid NotificationButton;

        [Obsolete("XAMLIL placeholder", true)]
        public MainWindow()
        {
        }

        public MainWindow(IConfigurationLoader configurationLoader, IPageNavigationProvider navigationProvider, CancellationTokenRepository tokenRepo, IGameLauncher gameLauncher, IDiscordService discordService, ITelemetry telemetry, INotificationService notify)
        {
            _configuration = configurationLoader;
            _navigationProvider = navigationProvider;
            _cancelRepo = tokenRepo;
            _gameLauncher = gameLauncher;
            _discordService = discordService;
            _telemetry = telemetry;
            _notify = notify;

            Closed += OnWindowClosed;

            if(configurationLoader.Spectabis.EnableTelemetry)
            {
                _telemetry.EnableTelemetry();
            }

            InitializeFileSystem.Initialize();

            InitializeComponent();
            RegisterChildern();

            _navigationProvider.ReferenceContainer(ContentContainer);
            _navigationProvider.ReferenceNavigationControls(TitlebarPanel, OnIconPress);

            FillElementColors();

            ContentContainer.PropertyChanged += OnContentContainerPropertyChanged;

            SetInitialPage();
            _navigationProvider.GeneratePageIcons();

            // _notify.ToastNotification +=
            NotificationButton.PointerPressed += OnNotificationButtonPress;
            NotificationPanelBackground.PointerReleased += OnNotificationBackgroundPress;
        }

        private void OnNotificationBackgroundPress(object sender, PointerReleasedEventArgs e)
        {
            NotificationPanel.IsVisible = false;
        }

        private void OnNotificationButtonPress(object sender, PointerPressedEventArgs e)
        {
            NotificationPanel.IsVisible = true;
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            _gameLauncher.StopGame();
            _cancelRepo.CancelToken(SpectabisLib.Enums.CancellationTokenKey.SpectabisApp);
        }

        private void OnIconPress(object sender, EventArgs e)
        {
            var icon = (PageIcon)sender;
            var page = icon.Destination;

            _navigationProvider.NavigatePage(page);
        }

        private void FillElementColors()
        {
            Background = _configuration.UserInterface.UIBackgroundGradient;
            Titlebar.Fill = _configuration.UserInterface.TitlebarGradient;
        }

        private void RegisterChildern()
        {
            Titlebar = this.FindControl<Rectangle>(nameof(Titlebar));
            TitlebarPanel = this.FindControl<StackPanel>(nameof(TitlebarPanel));
            ContentContainer = this.FindControl<ContentControl>(nameof(ContentContainer));

            NotificationButton = this.FindControl<Grid>(nameof(NotificationButton));
            NotificationPanel = this.FindControl<Grid>(nameof(NotificationPanel));
            NotificationPanelBackground = this.FindControl<Rectangle>(nameof(NotificationPanelBackground));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _discordService.InitializeDiscord();
        }

        private void OnContentContainerPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Content")
            {
                NavigationBarVisiblity((IPage)e.NewValue);
            }
        }

        private void NavigationBarVisiblity(IPage newContent)
        {
            this.FindControl<Grid>("TitlebarContainer").IsVisible = !newContent.HideTitlebar;
        }

        private void SetInitialPage()
        {
            _navigationProvider.Navigate<FirstTimeWizard>();
        }
    }
}