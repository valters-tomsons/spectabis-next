using System;
using Avalonia.Media.Imaging;
using SpectabisUI.Controls;

namespace SpectabisUI.Interfaces
{
    public interface IPageIcon
    {
        Page Destination { get; }
        event EventHandler InvokedCallback;
    }
}