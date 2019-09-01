using SpectabisNext.Pages;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class PagePreloader : IPagePreloader
    {
        private IPageRepository pageRepository { get; set; }

        public PagePreloader()
        {

        }

        public void Preload(IPageRepository targetRepository)
        {
            pageRepository = targetRepository;

            LoadPage<GameLibrary>();
            LoadPage<Settings>();
        }

        private void LoadPage<T>()
        {
            var pageType = typeof(T);
            System.Console.WriteLine($"Preloading page: {pageType}");
            pageRepository.GetPage<T>();
        }


    }
}