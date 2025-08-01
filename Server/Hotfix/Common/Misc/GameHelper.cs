namespace Hotfix.Common.Misc;

public static class GameHelper
{
    public const int AUTO_SAVE_INTERVAL = 5; // 自动保存间隔时间，单位为秒
    public static long GetCurrentTimeStamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}