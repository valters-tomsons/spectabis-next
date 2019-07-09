using System;
using Avalonia.Controls;
using SpectabisUI.Exceptions;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class PageNavigator : IPageNavigationProvider
    {
        private readonly IPageRepository _pageRepository;
        private ContentControl PageContentContainer { get; set; };

        public PageNavigator(IPageRepository pageRepository)
        {
            this._pageRepository = pageRepository;
        }

        public void ReferenceContainer(ContentControl container)
        {
            if(container == null)
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

            if(PageContentContainer == null)
            {
                throw new NavigatorContentContainerNullException();
            }

            if(pageResult == null)
            {
                Console.WriteLine($"Page '{typeof(T)}' not found!");
                return;
            }

            PageContentContainer.Content = _pageRepository.GetPage<T>();
        }
    }
}