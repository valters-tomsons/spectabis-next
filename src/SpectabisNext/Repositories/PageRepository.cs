using System.Collections.Generic;
using System.Linq;
using SpectabisNext.Views;
using SpectabisUI.Controls;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Repositories
{
    public class PageRepository : IPageRepository
    {
        List<Page> Pages { get; set; }

        // All views (pages) should be made to load auto with AutoFac
        public PageRepository(GameLibrary gameLibrary, Settings settingsPage, FirstTimeWizard firstTimeWizard)
        {
            Pages = new List<Page>();

            Pages.Add(gameLibrary);
            Pages.Add(settingsPage);
            Pages.Add(firstTimeWizard);
        }

        public void Add(Page page)
        {
            Pages.Add(page);
        }

        public void Add(IEnumerable<Page> pages)
        {
            Pages.AddRange(pages);
        }

        public IEnumerable<Page> All => Pages;

        public Page GetPage<T>()
        {
            return Pages.Single(x => x.GetType() == typeof(T));
        }
    }
}