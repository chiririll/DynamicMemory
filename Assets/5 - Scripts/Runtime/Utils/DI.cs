using System;
using System.Collections.Generic;

namespace DynamicMem
{
    public static class DI
    {
        private static Dictionary<Type, object> compositionRoot = new();

        public static T Get<T>()
        {
            return (T)compositionRoot.GetValueOrDefault(typeof(T));
        }

        public static void Add<T>(T item)
        {
            compositionRoot.Add(typeof(T), item);
        }
    }
}
