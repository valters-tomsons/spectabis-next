using System;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Threading;
using Common.Helpers;
using SpectabisUI.Factories;
using SpectabisUI.Events;
using SpectabisUI.Exceptions;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class PageNavigator : IPageNavigationProvider
    {
        private readonly IPageRepository _pageRepository;
        private readonly IPagePreloader _pagePreloader;
        private readonly NavigationBarItemFactory _navItemFactory;

        private ContentControl PageContentContainer { get; set; }
        private StackPanel NavigationItemBar { get; set; }
        private EventHandler PageNavigationClicked { get; set; }

        public event EventHandler<NavigationArgs> PageNavigationEvent;

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
                Logging.WriteLine("MainWindow not initialized properly");
                Logging.WriteLine("Content [Control] container is null");
                return;
            }

            PageContentContainer = container;
        }

        public void ReferenceNavigationControls(StackPanel navigationBar, EventHandler itemClickEvent)
        {
            NavigationItemBar = navigationBar;
            PageNavigationClicked = itemClickEvent;
        }

        public void NavigatePage(IPage page)
        {
            var type = page.GetType();
            MethodInfo genericMethod = typeof(PageNavigator).GetMethod("Navigate");
            MethodInfo specificMethod = genericMethod.MakeGenericMethod(type);
            specificMethod.Invoke(this, null);
        }

        public void Navigate<T>() where T : IPage
        {
            var pageResult = _pageRepository.GetPage<T>();

            if (PageContentContainer == null)
            {
                throw new PageIconCreatedNotAllowedException();
            }

            if (pageResult == null)
            {
                Logging.WriteLine($"Page '{typeof(T)}' not found!");
                return;
            }

            OnPageNavigation(this, pageResult);
            Dispatcher.UIThread.InvokeAsync(new Action(() => PageContentContainer.Content = pageResult));
        }

        private void OnPageNavigation(object sender, IPage page)
        {
            var args = new NavigationArgs(page);
            PageNavigationEvent?.Invoke(sender, args);
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
                Logging.WriteLine($"Generating navigation icon for {page.GetType()}");
                var icon = _navItemFactory.Create(page, PageNavigationClicked);
                NavigationItemBar.Children.Add(icon);
            }
        }
    }
}