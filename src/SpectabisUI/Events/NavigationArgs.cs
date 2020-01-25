using System;
using SpectabisUI.Interfaces;

namespace SpectabisUI.Events
{
    public class NavigationArgs : EventArgs
    {
        private readonly IPage page;

        public NavigationArgs(IPage page)
        {
            this.page = page;
        }

        public IPage Page { get => page; }
    }
}