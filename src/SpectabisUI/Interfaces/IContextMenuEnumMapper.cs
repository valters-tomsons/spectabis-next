using System.Collections.Generic;
using SpectabisUI.Enums;

namespace SpectabisUI.Interfaces
{
    public interface IContextMenuEnumMapper
    {
        string GetDisplayName(GameContextMenuItem item);
        IEnumerable<string> GetDisplayNames();
    }
}