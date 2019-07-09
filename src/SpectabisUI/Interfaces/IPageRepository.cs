using System.Collections.Generic;
using SpectabisUI.Controls;

namespace SpectabisUI.Interfaces
{
    public interface IPageRepository
    {
        void Add(Page page);
        void Add(IEnumerable<Controls.Page> pages);
        IEnumerable<Page> All {get;}
        Page GetPage<T>();
    }
}