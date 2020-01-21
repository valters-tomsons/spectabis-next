using System;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Threading;
using SpectabisNext.Factories;
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

        public PageNavigator(IPageRepository pageRepository, IPagePreloader pagePreloader, NavigationBarItemFactory navItemFactory)
        {
            _navItemFactory = navItemFactory;
            _pagePreloader = pagePreloader;
            _pageRepository = pageRepository;
        }

        public void ReferenceContainer(ContentControl container)
        {
            PreloadPages();

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

        public void NavigatePage(Page page)
        {
            var type = page.GetType();
            MethodInfo genericMethod = typeof(PageNavigator).GetMethod("Navigate");
            MethodInfo specificMethod = genericMethod.MakeGenericMethod(type);
            specificMethod.Invoke(this, null);
        }

        public void Navigate<T>() where T : Page
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

            Dispatcher.UIThread.InvokeAsync(new Action(() => PageContentContainer.Content = pageResult));
        }

        private void PreloadPages()
        {
            _pagePreloader.Preload();
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