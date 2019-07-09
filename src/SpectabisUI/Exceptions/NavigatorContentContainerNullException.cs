using System;

namespace SpectabisUI.Exceptions
{
    public class NavigatorContentContainerNullException : Exception
    {
        public NavigatorContentContainerNullException() : base(message)
        {
        }

        private new static string Message()
        {
            return "PageNavigator Content Container cannot be null!";
        }
    }
}