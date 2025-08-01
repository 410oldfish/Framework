using Hotfix.Common.Misc.Interface;
namespace Hotfix.Common.Misc.Tool;

public static class TrackableUtils
{
    public static void SetValue<T>(this ITrackable self, ref T field, T newValue)
    {
        if (!EqualityComparer<T>.Default.Equals(field, newValue))
        {
            field = newValue;
            self.OnChanged?.Invoke();
        }
    }
}
