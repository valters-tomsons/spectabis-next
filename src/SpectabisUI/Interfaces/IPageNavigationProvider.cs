using Avalonia.Controls;

namespace SpectabisUI.Interfaces
{
    public interface IPageNavigationProvider
    {
        void Navigate<T>();
        void ReferenceContainer(ContentControl ContentContainer);
    }
}