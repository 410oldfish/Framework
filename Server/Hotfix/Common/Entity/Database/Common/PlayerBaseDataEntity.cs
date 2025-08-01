using Fantasy;
using Fantasy.Entitas.Interface;
using Hotfix.Common.Entity.Base;
using Hotfix.Common.Misc;

namespace Hotfix.Common.Entity.Database.Common;

public sealed class On_PlayerCoreDataEntity_AwakeSystem : AwakeSystem<PlayerCoreDataEntity>
{
    protected override void Awake(PlayerCoreDataEntity self)
    {
        var worldDataBase = self.Scene.World.DataBase;
        //开启自动存库任务
        var timerId = self.Scene.TimerComponent.Net.RepeatedTimer(1000 * GameHelper.AUTO_SAVE_INTERVAL, () =>
        {
            if (self.IsDirty)
            {
                worldDataBase.Save<PlayerCoreDataEntity>(self);
                self.ClearDirty(); // 清除脏标记
                Log.Debug("Auto Save PlayerCoreDataEntity: {0}", self.RuntimeId);
            }
        });
        
        self.SetSaveTaskId(timerId);
    }
}

public sealed class On_PlayerCoreDataEntity_Deserialize : DeserializeSystem<PlayerCoreDataEntity>
{
    protected override void Deserialize(PlayerCoreDataEntity self)
    {
        var worldDataBase = self.Scene.World.DataBase;
        //开启自动存库任务
        var timerId = self.Scene.TimerComponent.Net.RepeatedTimer(1000 * GameHelper.AUTO_SAVE_INTERVAL, () =>
        {
            if (self.IsDirty)
            {
                worldDataBase.Save<PlayerCoreDataEntity>(self);
                self.ClearDirty(); // 清除脏标记
                Log.Debug("Auto Save PlayerCoreDataEntity: {0}", self.RuntimeId);
            }
        });
        
        self.SetSaveTaskId(timerId);
    }
}

public sealed class On_PlayerCoreDataEntity_Dispose : DestroySystem<PlayerCoreDataEntity>
{
    protected override void Destroy(PlayerCoreDataEntity self)
    {
        self.Scene.TimerComponent.Net.Remove(self.SaveTaskId);
        Log.Debug( "Dispose PlayerCoreDataEntity: {0}", self.RuntimeId);
    }
}
//玩家基本数据实体
public class PlayerCoreDataEntity : PlayerDataBase
{
    private string _nickName;

    public string NickName
    {
        get => _nickName;
        private set => SetValue(ref _nickName, value);
    }

    private int _lv;
    public int Lv
    {
        get => _lv;
        private set => SetValue(ref _lv, value);
    }
    
    private int _exp;
    public int Exp
    {
        get => _exp;
        private set => SetValue(ref _exp, value);
    }

    public void Init(long playerId, string nickName, int lv, int exp)
    {
        
    }

    public void SetPlayerData(long playerId, string nickName, int lv, int exp)
    {
        SetPlayerId(playerId);
        ChangeNickName(nickName);
        SetLv(lv);
        SetExp(exp);
    }
    
    public bool ChangeNickName(string nickName)
    {
        if (string.IsNullOrEmpty(nickName))
        {
            return false;
        }
        
        NickName = nickName;
        return true;
    }
    
    public bool SetLv(int lv)
    {
        if (lv < Lv || lv <= 0)
        {
            return false;
        }
        
        Lv = lv;
        return true;
    }
    
    public bool SetExp(int exp)
    {
        if (exp < 0)
        {
            return false;
        }
        
        Exp = exp;
        return true;
    }
}