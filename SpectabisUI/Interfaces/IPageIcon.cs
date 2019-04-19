using System;
using Avalonia.Media.Imaging;
using SpectabisNext.Controls;
using SpectabisUI.Controls;

namespace SpectabisUI.Interfaces
{
    public interface IPageIcon
    {
        Page Destination { get; }
        event EventHandler InvokedCallback;
    }
}