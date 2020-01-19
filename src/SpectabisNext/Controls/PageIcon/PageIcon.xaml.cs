using System;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Controls.PageIcon
{
    public class PageIcon : UserControl, IPageIcon
    {
        public Page Destination { get; private set; }
        public event EventHandler InvokedCallback;

        private Image DisplayImage { get; set; }
        private TextBlock DisplayText { get; set; }
        private Rectangle HoverOverlayRectangle { get; set; }
        private string FallbackDisplayString { get; set; }

        public PageIcon() {}

        public PageIcon(Page destination)
        {
            Initialize(destination);
            SetIconString(destination.PageTitle);
        }

        public PageIcon(Page destination, string stringDisplay)
        {

        }

        private void Initialize(Page destination)
        {
            InitializeComponent();
            RegisterChildren();
            RegisterEvents();

            Destination = destination;

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetIconImage(Bitmap bmp)
        {
            DisplayImage.Source = bmp;
            DisplayImage.IsVisible = true;
        }

        private void SetIconString(string str)
        {
            DisplayText.Text = str;
            DisplayText.IsVisible = true;
        }

        private void RegisterChildren()
        {
            DisplayImage = this.FindControl<Image>("DisplayImage");
            DisplayText = this.FindControl<TextBlock>("DisplayText");
            HoverOverlayRectangle = this.FindControl<Rectangle>("HoverOverlay");
        }

        private void RegisterEvents()
        {
            this.PointerReleased += OnPointerReleased;
            this.PointerEnter += OnMousePointerEnter;
            this.PointerLeave += OnMousePointerLeave;
        }

        private void OnPointerReleased(object sender, PointerReleasedEventArgs args)
        {
            InvokedCallback?.Invoke(this, EventArgs.Empty);
        }

        private void OnMousePointerLeave(object sender, PointerEventArgs e)
        {
            ShowHoverOverlay = false;
        }
        
        private bool ShowHoverOverlay
        {
            get
            {
                return HoverOverlayRectangle.IsVisible;
            }
            set
            {
                HoverOverlayRectangle.IsVisible = value;
            }
        }

        private void OnMousePointerEnter(object sender, PointerEventArgs e)
        {
            ShowHoverOverlay = true;
        }
    }
}