using System;
using Avalonia.Media.Imaging;
using SpectabisNext.Controls;

namespace SpectabisNext.Interfaces
{
    public interface IPageIcon
    {
        Page Destination { get; }
        event EventHandler InvokedCallback;
    }
}