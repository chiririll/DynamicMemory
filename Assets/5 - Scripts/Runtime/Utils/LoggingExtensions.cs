using UnityEngine;

namespace DynamicMem
{
    public static class LoggingExtensions
    {
        public static void LogMsg<T>(this T obj, string message, Object context = null) 
            => Log(LogType.Log, obj, message, context);

        public static void LogWarn<T>(this T obj, string message, Object context = null)
            => Log(LogType.Warning, obj, message, context);

        public static void LogErr<T>(this T obj, string message, Object context = null)
            => Log(LogType.Error, obj, message, context);
        private static void Log<T>(LogType type, T obj, string message, Object context = null)
        {
            message = $"<b>[{typeof(T).Name}]</b>: {message}";
            context ??= obj as Object;
            
            if (context != null)
            {
                Debug.unityLogger.Log(type, (object)message, context);
            }
            else
            {
                Debug.unityLogger.Log(type, message);
            }
        }
    }
}
