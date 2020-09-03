using System;

namespace SpectabisUI.Interfaces
{
    public interface IPageIcon
    {
        IPage Destination { get; }
        event EventHandler InvokedCallback;
    }
}