using System.Collections.Generic;

namespace SpectabisUI.Interfaces
{
    public interface IPageRepository
    {
        IEnumerable<Page> All {get;}
        Page GetPage<T>();
    }
}