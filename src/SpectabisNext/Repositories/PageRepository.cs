using System.Collections.Generic;
using System.Linq;
using SpectabisNext.Factories;
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

        /// <summary>
        /// Return an instance of a page from repository. If page does not exist yet, one is created.
        /// </summary>
        public Page GetPage<T>()
        {
            var page = Pages.SingleOrDefault(x => x.GetType() == typeof(T)); 

            if(page == null)
            {
                page = _pageFactory.Create<T>();

                if(!page.ReloadOnNavigation)
                {
                    Pages.Add(page);
                }
            }

            return page;
        }
    }
}