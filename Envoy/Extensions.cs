using System;
using System.Linq;

namespace Envoy
{
    static class Extensions
    {
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
