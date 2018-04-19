using System;

namespace Envoy
{
    internal static class Guard
    {
        public static void AgainstNull<T>(T parameter, string name) where T : class
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}