namespace Hotfix.Common.Misc.Interface;

/// <summary>
/// 数据变化监听接口
/// </summary>
public interface ITrackable
{
    public Action? OnChanged { get; set; }
    
    protected void SetValue<T>(ref T field, T newValue)
    {
        if (!EqualityComparer<T>.Default.Equals(field, newValue))
        {
            field = newValue;
            OnChanged?.Invoke();
        }
    }
}