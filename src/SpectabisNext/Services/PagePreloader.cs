using Common.Helpers;
using SpectabisUI.Pages;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class PagePreloader : IPagePreloader
    {
        private readonly IPageRepository _pageRepository;

        public PagePreloader(IPageRepository pageRepository)
        {
            Logging.WriteLine("PagePreloader: " + GetHashCode());
            _pageRepository = pageRepository;
        }

        public void Preload()
        {
            LoadPage<GameLibrary>();
            LoadPage<Settings>();
            LoadPage<CreateProfile>();
        }

        private void LoadPage<T>()
        {
            var pageType = typeof(T);
            Logging.WriteLine($"Preloading page: {pageType}");
            _pageRepository.GetPage<T>();
        }
    }
}