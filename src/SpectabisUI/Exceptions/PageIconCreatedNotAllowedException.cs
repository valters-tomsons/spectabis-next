using System;

namespace SpectabisUI.Exceptions
{
    public class PageIconCreatedNotAllowedException : Exception
    {
        public PageIconCreatedNotAllowedException() : base(Message())
        {
        }

        private new static string Message()
        {
            return "This page does not allow creation of navigation icon!";
        }
    }
}