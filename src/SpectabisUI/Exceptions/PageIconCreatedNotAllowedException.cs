using System;

namespace SpectabisUI.Exceptions
{
    public class PageIconCreatedNotAllowedException : Exception
    {
        public PageIconCreatedNotAllowedException() : base(Message())
        {
        }

        public PageIconCreatedNotAllowedException(string message) : base(message)
        {
        }

        public PageIconCreatedNotAllowedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private new static string Message()
        {
            return "This page does not allow creation of navigation icon!";
        }
    }
}