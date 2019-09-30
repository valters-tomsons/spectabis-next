using System;
using Avalonia.Media.Imaging;

namespace SpectabisUI.Interfaces
{
    public interface IPageIcon
    {
        Page Destination { get; }
        event EventHandler InvokedCallback;
    }
}