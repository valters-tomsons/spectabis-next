using System.Collections.Generic;
using SpectabisUI.Controls;

namespace SpectabisUI.Interfaces
{
    public interface IPageRepository
    {
        IEnumerable<Page> All {get;}
        Page GetPage<T>();
    }
}