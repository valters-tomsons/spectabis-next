using System.Collections.Generic;
using SpectabisNext.Controls;
using SpectabisNext.Views;

namespace SpectabisNext.Repositories
{
    public class PageRepository
    {
        List<Page> Pages { get; set; }

        public PageRepository(GameLibrary gameLibrary, Settings settingsPage)
        {
            Pages = new List<Page>();

            Pages.Add(gameLibrary);
            Pages.Add(settingsPage);
        }

        public List<Page> GetAll()
        {
            return Pages;
        }
    }
}