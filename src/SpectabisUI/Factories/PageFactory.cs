using Autofac;
using Common.Helpers;
using SpectabisUI.Interfaces;

namespace SpectabisUI.Factories
{
    public class PageFactory
    {
        private readonly ILifetimeScope _containerScope;

        public PageFactory(ILifetimeScope containerScope)
        {
            _containerScope = containerScope;
        }

        public IPage Create<T>()
        {
            var page = _containerScope.Resolve<T>();
            Logging.WriteLine($"Creating new {page.GetType()}");
            return page as IPage;
        }
    }
}