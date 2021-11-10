using System;
using System.Collections.Generic;
using System.Linq;
using SpectabisUI.Enums;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class ContextMenuEnumMapper : IContextMenuEnumMapper
    {
        private readonly Dictionary<GameContextMenuItem, string> _items;

        public ContextMenuEnumMapper()
        {
            _items = InitializeDictionary();
        }

        public string GetDisplayName(GameContextMenuItem item)
        {
            return _items[item];
        }

        public IEnumerable<string> GetDisplayNames()
        {
            return _items.Select(x => x.Value);
        }

        private static Dictionary<GameContextMenuItem, string> InitializeDictionary()
        {
            return Enum.GetValues<GameContextMenuItem>().ToDictionary(x => x, x => GetItemDisplayName(x));
        }

        private static string GetItemDisplayName(GameContextMenuItem item)
        {
            return item switch
            {
                GameContextMenuItem.Launch => "Launch",
                GameContextMenuItem.Remove => "Remove",
                GameContextMenuItem.OpenWiki => "Open Wiki",
                GameContextMenuItem.Settings => "Settings",
                _ => $"{item}",
            };
        }
    }
}