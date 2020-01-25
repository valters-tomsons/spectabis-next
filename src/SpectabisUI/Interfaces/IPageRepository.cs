using System.Collections.Generic;

namespace SpectabisUI.Interfaces
{
    public interface IPageRepository
    {
        IEnumerable<IPage> All {get;}
        IPage GetPage<T>();
    }
}