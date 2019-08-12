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
            var page = _containerScope.Resolve<T>();
            System.Console.WriteLine($"PageFactory: Creating new {page.GetType()}");
            return page as Page;
        }
    }
}