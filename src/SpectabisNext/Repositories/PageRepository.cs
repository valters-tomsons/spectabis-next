using System.Collections.Generic;
using System.Linq;
using SpectabisNext.Factories;
using SpectabisNext.Views;
using SpectabisUI.Controls;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly PageFactory _pageFactory;

        List<Page> Pages { get; set; }

        public PageRepository(PageFactory pageFactory)
        {
            Pages = new List<Page>();
            _pageFactory = pageFactory;
        }

        public IEnumerable<Page> All => Pages;

        public Page GetPage<T>()
        {
            var page = Pages.SingleOrDefault(x => x.GetType() == typeof(T)); 

            if(page == null || page.ReloadOnNavigation)
            {
                return _pageFactory.Create<T>();
            }

            return page;
        }
    }
}