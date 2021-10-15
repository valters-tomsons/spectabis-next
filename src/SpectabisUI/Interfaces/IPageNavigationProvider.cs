using System;
using Avalonia.Controls;
using SpectabisUI.Events;

namespace SpectabisUI.Interfaces
{
    public interface IPageNavigationProvider
    {
        void Navigate<T>() where T : IPage;
        void NavigatePage(IPage page);
        void Internals_ReferenceContainer(ContentControl ContentContainer);
        void Internals_ReferenceNavigationControls(StackPanel NavigationBar, EventHandler NavigationItemClickEvent);
        void GeneratePageIcons();
        event EventHandler<NavigationArgs> PageNavigationEvent;
    }
}