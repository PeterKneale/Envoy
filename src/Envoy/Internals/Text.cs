using System;
using System.Linq;

namespace Envoy.Internals
{
    internal static class Text
    {
        public static string FindingHandler<T>() => $"Attempting to resolve handler that implements '{typeof(T).ToGenericName()}'";
        public static string FoundHandler<T>(IHandler handler) where T : IHandler => $"Resolved handler '{handler.GetType().ToGenericName()}' that implements '{typeof(T).ToGenericName()}'";
        public static string FoundNoHandler<T>() where T : IHandler => $"No handler could be resolved that implements '{typeof(T).ToGenericName()}'";
        public static string HandlerCreationError<T>() => $"Handler for '{typeof(T).ToGenericName()}' could not be instantiated";
        public static string Handling(IHandler handler, IMessage message) => $"Attempting to handle '{message.GetType().Name}' with handler '{handler.GetType().Name}'";
        public static string Handled(IHandler handler, IMessage message) => $"Handled '{message.GetType().Name}' with handler '{handler.GetType().Name}'";
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
