using Hotfix.Common.Misc.Interface;

namespace Hotfix.Common.Misc.CustomClass;

public class DirtyList<T> : List<T> where T : notnull
{
    public Action? OnChanged;

    public new void Add(T item)
    {
        base.Add(item);
        BindItemChanged(item);
        OnChanged?.Invoke();
    }
    
    public new void AddRange(IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            base.Add(item);
            BindItemChanged(item);
        }
        OnChanged?.Invoke();
    }

    public new T this[int index]
    {
        get => base[index];
        set
        {
            base[index] = value;
            BindItemChanged(value);
            OnChanged?.Invoke();
        }
    }

    public new void Clear()
    {
        UnbindAll();
        base.Clear();
        OnChanged?.Invoke();
    }

    public new bool Remove(T item)
    {
        UnbindItemChanged(item);
        var result = base.Remove(item);
        if (result) OnChanged?.Invoke();
        return result;
    }

    private void BindItemChanged(T item)
    {
        if (item is ITrackable trackable)
        {
            trackable.OnChanged = OnChanged;
        }
    }

    private void UnbindItemChanged(T item)
    {
        if (item is ITrackable trackable)
        {
            trackable.OnChanged = null;
        }
    }

    private void UnbindAll()
    {
        foreach (var item in this)
        {
            UnbindItemChanged(item);
        }
    }
}