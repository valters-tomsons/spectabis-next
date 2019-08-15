using System;
using System.Linq;
using Avalonia.Controls;
using SpectabisNext.Controls.PageIcon;
using SpectabisNext.Factories;
using SpectabisUI.Controls;
using SpectabisUI.Exceptions;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class PageNavigator : IPageNavigationProvider
    {
        private readonly IPageRepository _pageRepository;
        private IPagePreloader _pagePreloader { get; set; }
        private readonly NavigationBarItemFactory _navItemFactory;
        private ContentControl PageContentContainer { get; set; }
        private StackPanel NavigationItemBar { get; set; }
        private EventHandler NavigationItemClick { get; set; }
        private Page LastPageBuffer { get; set; }

        public PageNavigator(IPageRepository pageRepository, IPagePreloader pagePreloader, NavigationBarItemFactory navItemFactory)
        {
            _navItemFactory = navItemFactory;
            _pagePreloader = pagePreloader;
            _pageRepository = pageRepository;

            PreloadPages();
        }

        public void ReferenceContainer(ContentControl container)
        {
            if (container == null)
            {
                Console.WriteLine("MainWindow not initialized properly");
                Console.WriteLine("Content [Control] container is null");
                return;
            }

            PageContentContainer = container;
        }

        public void ReferenceNavigationControls(StackPanel navigationBar, EventHandler itemClickEvent)
        {
            NavigationItemBar = navigationBar;
            NavigationItemClick = itemClickEvent;
        }

        public void Navigate<T>()
        {
            var pageResult = _pageRepository.GetPage<T>();

            if (PageContentContainer == null)
            {
                throw new PageIconCreatedNotAllowedException();
            }

            if (pageResult == null)
            {
                Console.WriteLine($"Page '{typeof(T)}' not found!");
                return;
            }

            if (LastPageBuffer != null)
            {
                LastPageBuffer = (Page) PageContentContainer.Content;
            }

            PageContentContainer.Content = pageResult;
        }

        private void PreloadPages()
        {
            _pagePreloader.Preload(_pageRepository);
        }

        public void GeneratePageIcons()
        {
            var loadedPages = _pageRepository.All.Where(x => x.ShowInTitlebar);

            foreach (var page in loadedPages)
            {
                System.Console.WriteLine($"Generating navigation icon for {page.GetType()}");
                var icon = _navItemFactory.Create(page, NavigationItemClick);
                NavigationItemBar.Children.Add(icon);
            }
        }
    }
}