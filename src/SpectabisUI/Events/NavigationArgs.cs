using System;
using SpectabisUI.Interfaces;

namespace SpectabisUI.Events
{
    public class NavigationArgs : EventArgs
    {
        public NavigationArgs(IPage page)
        {
            Page = page;
        }

        public IPage Page { get; }
    }
}