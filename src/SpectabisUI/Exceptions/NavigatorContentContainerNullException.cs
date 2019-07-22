using System;

namespace SpectabisUI.Exceptions
{
    public class NavigatorContentContainerNullException : Exception
    {
        public NavigatorContentContainerNullException() : base(Message())
        {
        }

        private new static string Message()
        {
            return "PageNavigator Content Container cannot be null!";
        }
    }
}