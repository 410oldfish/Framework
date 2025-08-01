using Fantasy;
using Fantasy.Entitas.Interface;
using Fantasy.Event;
using Hotfix.Common.Misc;

namespace Hotfix.Common.Entity.Base;

//需要存库的模块基类
public class PlayerDataBase : Fantasy.Entitas.Entity
{
    private long _savetaskId;
    public long SaveTaskId
    {
        get => _savetaskId;
    }
    
    public void SetSaveTaskId(long taskId)
    {
        _savetaskId = taskId;
    }
    
    private long _playerId;
    public long PlayerId
    {
        get => _playerId;
        private set => SetValue(ref _playerId, value);
    }

    public void SetPlayerId(long playerId)
    {
        PlayerId = playerId;
    }

    public bool IsDirty { get; private set; }

    protected void SetValue<T>(ref T field, T newValue)
    {
        if (!EqualityComparer<T>.Default.Equals(field, newValue))
        {
            field = newValue;
            IsDirty = true;
        }
    }
    
    public void SetDirty()
    {
        IsDirty = true;
    }

    public void ClearDirty() => IsDirty = false;
}