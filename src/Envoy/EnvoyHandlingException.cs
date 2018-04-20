using System;

namespace Envoy
{
    public class EnvoyException : Exception
    {
        public EnvoyException(string message) : base(message)
        {
        }

        public EnvoyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}