using System;
using System.Collections.Generic;

public static class Singletons
{
    public static void Register<T>(T instance)
    {
        if (_instances.ContainsKey(typeof(T)))
            throw new InvalidOperationException($"Only one singleton of type {typeof(T)} is allowed at runtime.");

        _instances.Add(typeof(T), instance);
    }

    public static T Get<T>()
    {
        if (!_instances.TryGetValue(typeof(T), out object instance))
            throw new InvalidOperationException($"Could not find a singleton of type {typeof(T)}.");

        return (T)instance;
    }

    private static readonly Dictionary<Type, object> _instances = new();
}
