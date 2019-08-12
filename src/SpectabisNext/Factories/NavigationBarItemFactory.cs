using System;
using SpectabisNext.Controls.PageIcon;
using SpectabisUI.Controls;
using SpectabisUI.Exceptions;

namespace SpectabisNext.Factories
{
    public class NavigationBarItemFactory
    {
        public NavigationBarItemFactory()
        {

        }

        public PageIcon Create(Page page, EventHandler clickCallback)
        {
            if(!page.ShowInTitlebar)
            {
                throw new PageIconCreatedNotAllowedException();
            }

            var pageIcon = new PageIcon(page);
            pageIcon.InvokedCallback += clickCallback;

            return pageIcon;
        }
    }
}