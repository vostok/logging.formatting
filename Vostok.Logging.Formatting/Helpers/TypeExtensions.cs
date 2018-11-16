using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vostok.Logging.Formatting.Helpers
{
    internal static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetTypesSilently(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(x => x != null);
            }
        }
    }
}