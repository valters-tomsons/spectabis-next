using System;
using Avalonia.Media.Imaging;

namespace SpectabisUI.Interfaces
{
    public interface IPageIcon
    {
        IPage Destination { get; }
        event EventHandler InvokedCallback;
    }
}