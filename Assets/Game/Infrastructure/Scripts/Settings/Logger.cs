using UnityEngine;

public static class Logger
{
    public static void Log(this object obj, object message)
    {
        Debug.Log($"<color=cyan>[{obj.GetType().Name}]</color>: {message}");
    }
    
    public static void LogWarning(this object obj, object message)
    {
        Debug.Log($"<color=yellow>[{obj.GetType().Name}]</color>: {message}");
    }
    
    public static void LogError(this object obj, object message)
    {
        Debug.Log($"<color=#F80000>[{obj.GetType().Name}]</color>: {message}");
    }
}