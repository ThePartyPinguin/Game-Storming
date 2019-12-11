using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A class that contains a singleton dictionary
/// </summary>
/// <typeparam name="K">Key, must inherit from Enum</typeparam>
/// <typeparam name="V">Value, can be anything</typeparam>
/// <typeparam name="T">Must inherit from MemoryDataBase</typeparam>
public abstract class MemoryDatabase<K, V, T> : Singleton<T> where T : MemoryDatabase<K, V, T>, new()
{
    private readonly Dictionary<K, V> _databaseCollection;

    protected MemoryDatabase()
    {
        _databaseCollection = new Dictionary<K, V>();
    }

    protected V GetValue(K identifier)
    {
        if (_databaseCollection.ContainsKey(identifier))
            return _databaseCollection[identifier];

        return default(V);
    }

    protected T GetValue<T>(K identifier) where T : V
    {
        if (_databaseCollection.ContainsKey(identifier))
        {
            V value = _databaseCollection[identifier];

            try
            {
                return (T)value;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Requested type does not match saved type: " + e.Message);
            }
        }

        return default(T);
    }

    protected bool TryGetValue(K identifier, out V value)
    {
        if (!_databaseCollection.ContainsKey(identifier))
        {
            value = default;
            return false;
        }

        value = _databaseCollection[identifier];
        return true;
    }

    protected void RemoveKey(K key)
    {
        if (!_databaseCollection.ContainsKey(key))
        {
            return;
        }

        _databaseCollection.Remove(key);
    }

    protected bool AddNewValue(K identifier, V value)
    {
        if (_databaseCollection.ContainsKey(identifier))
            return false;

        _databaseCollection.Add(identifier, value);
        return true;
    }

    protected V[] GetAllValues()
    {
        return _databaseCollection.Values.ToArray();
    }

    protected int GetCount()
    {
        return _databaseCollection.Count;
    }

    protected bool KeyExists(K key)
    {
        return _databaseCollection.ContainsKey(key);
    }

    protected void ClearDatabase()
    {
        _databaseCollection.Clear();
    }
}
