using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class ToastService : INotificationService
    {
        public event EventHandler<EventArgs> ToastNotification;

        public ToastService()
        {
            System.Console.WriteLine("[ToastService] Created");
        }

        public void NotifyGamesFound(IEnumerable<string> filesFound)
        {

        }

        private Task DisplayToast()
        {
            return null;
        }
    }
}