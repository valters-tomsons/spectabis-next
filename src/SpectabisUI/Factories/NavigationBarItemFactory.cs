using System;
using SpectabisUI.Controls.PageIcon;
using SpectabisUI.Exceptions;
using SpectabisUI.Interfaces;

namespace SpectabisUI.Factories
{
    public class NavigationBarItemFactory
    {
        public PageIcon Create(IPage page, EventHandler clickCallback)
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