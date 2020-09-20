using System;

namespace SimpleChat.Exceptions
{
    public class FoException : Exception
    {
        public FoException(string message) : base(message)
        {
        }
    }
}
