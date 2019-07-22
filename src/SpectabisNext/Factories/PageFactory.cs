using System;
using Autofac;
using Autofac.Core;
using SpectabisUI.Controls;

namespace SpectabisNext.Factories
{
    public class PageFactory
    {
        private readonly ILifetimeScope _containerScope;

        public PageFactory(ILifetimeScope containerScope)
        {
            _containerScope = containerScope;
        }

        public Page Create<T>()
        {
            var ll = _containerScope.Resolve<T>();
            return ll as Page;
        }
    }
}