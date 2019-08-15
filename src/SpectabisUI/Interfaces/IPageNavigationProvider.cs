using System;
using Avalonia.Controls;

namespace SpectabisUI.Interfaces
{
    public interface IPageNavigationProvider
    {
        void Navigate<T>();
        void ReferenceContainer(ContentControl ContentContainer);
        void ReferenceNavigationControls(StackPanel NavigationBar, EventHandler NavigationItemClickEvent);
        void GeneratePageIcons();
    }
}