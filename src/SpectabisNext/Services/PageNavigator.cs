using System;
using Avalonia.Controls;
using SpectabisUI.Controls;
using SpectabisUI.Exceptions;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class PageNavigator : IPageNavigationProvider
    {
        private readonly IPageRepository _pageRepository;
        private ContentControl PageContentContainer { get; set; }
        private Page LastPageBuffer { get; set; }
        private IPagePreloader _pagePreloader { get; set; }

        public PageNavigator(IPageRepository pageRepository, IPagePreloader pagePreloader)
        {
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
    }
}