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

        private Dictionary<GameContextMenuItem, string> InitializeDictionary()
        {
            return new Dictionary<GameContextMenuItem, string>()
            {
                { GameContextMenuItem.Launch, "Launch" },
                { GameContextMenuItem.Configure, "Configure"},
                { GameContextMenuItem.Remove, "Delete"},
                { GameContextMenuItem.OpenWiki, "Open in Wiki"}
            };
        }
    }
}