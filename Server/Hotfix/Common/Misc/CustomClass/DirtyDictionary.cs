using Fantasy.Entitas.Interface;
using Hotfix.Common.Misc.Interface;

namespace Hotfix.Common.Misc.CustomClass;

public class DirtyDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    public Action? OnChanged;

    public new bool TryAdd(TKey key, TValue value)
    {
        if (base.TryAdd(key, value))
        {
            BindTrackable(value);
            OnChanged?.Invoke();
            return true;
        }
        return false;
    }
    
    public new void Add(TKey key, TValue value)
    {
        base.Add(key, value);
        BindTrackable(value);
        OnChanged?.Invoke();
    }

    public new TValue this[TKey key]
    {
        get => base[key];
        set
        {
            var exists = base.TryGetValue(key, out var oldValue);
            if (!exists || !EqualityComparer<TValue>.Default.Equals(oldValue, value))
            {
                UnbindTrackable(oldValue);
                base[key] = value;
                BindTrackable(value);
                OnChanged?.Invoke();
            }
        }
    }

    public new bool Remove(TKey key)
    {
        if (base.TryGetValue(key, out var value))
        {
            UnbindTrackable(value);
        }

        var result = base.Remove(key);
        if (result) OnChanged?.Invoke();
        return result;
    }

    public new void Clear()
    {
        foreach (var value in Values)
        {
            UnbindTrackable(value);
        }

        base.Clear();
        OnChanged?.Invoke();
    }

    private void BindTrackable(TValue value)
    {
        if (value is ITrackable trackable)
        {
            // 多次绑定会覆盖，不会堆叠
            trackable.OnChanged = () => OnChanged?.Invoke();
        }
    }

    private void UnbindTrackable(TValue value)
    {
        if (value is ITrackable trackable)
        {
            trackable.OnChanged = null;
        }
    }
}

