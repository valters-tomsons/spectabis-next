using System;

namespace SpectabisUI.Exceptions
{
    public class NavigatorContentContainerNullException : Exception
    {
        public NavigatorContentContainerNullException() : base(Message())
        {
        }

        public NavigatorContentContainerNullException(string message) : base(message)
        {
        }

        public NavigatorContentContainerNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private new static string Message()
        {
            return "PageNavigator Content Container cannot be null!";
        }
    }
}