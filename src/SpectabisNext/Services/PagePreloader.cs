using SpectabisNext.Pages;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class PagePreloader : IPagePreloader
    {
        private readonly IPageRepository _pageRepository;

        public PagePreloader(IPageRepository pageRepository)
        {
            System.Console.WriteLine("PagePreloader: " +this.GetHashCode());
            _pageRepository = pageRepository;
        }

        public void Preload()
        {
            LoadPage<GameLibrary>();
            LoadPage<Settings>();
            LoadPage<AddGame>();
        }

        private void LoadPage<T>()
        {
            var pageType = typeof(T);
            System.Console.WriteLine($"Preloading page: {pageType}");
            _pageRepository.GetPage<T>();
        }

    }
}