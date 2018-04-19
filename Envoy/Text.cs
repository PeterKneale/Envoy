using System;
using System.Linq;

namespace Envoy
{
    internal static class Text
    {
        public static string FindingHandler<T>() => $"Finding {typeof(T).ToGenericName()}...";
        public static string FoundHandler<T>(IHandler handler) where T : IHandler => $"Found {handler.GetType().ToGenericName()} for {typeof(T).ToGenericName()}";
        public static string FoundNoHandler<T>() where T : IHandler => $"No handler found for {typeof(T).ToGenericName()}";
        public static string HandlerCreationError<T>() => $"Handler for {typeof(T).ToGenericName()} could not be instantiated";
        public static string Handling(IHandler handler, IMessage message) => $"{handler.GetType().Name} handling {message.GetType().Name}";
        public static string Handled(IHandler handler, IMessage message) => $"{handler.GetType().Name} handled {message.GetType().Name}";
        public static string ToGenericName(this Type t)
        {
            if (t.IsGenericType)
            {
                return string.Format(
                    "{0}<{1}>",
                    t.Name.Substring(0, t.Name.LastIndexOf("`", StringComparison.InvariantCulture)),
                    string.Join(", ", t.GetGenericArguments().Select(x => x.ToGenericName())));
            }

            return t.Name;
        }
    }
}
