using System;

namespace SpectabisUI.Interfaces
{
    public interface INotificationService
    {
        event EventHandler<EventArgs> ToastNotification;
    }
}