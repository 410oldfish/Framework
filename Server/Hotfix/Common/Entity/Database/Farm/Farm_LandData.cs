using Fantasy.Entitas.Interface;
using GameConfig.farm;
using Hotfix.Common.Misc.CustomClass;
using Hotfix.Common.Misc.Interface;
using Hotfix.Common.Misc.Tool;

namespace Hotfix.Common.Entity.Database;

public class Farm_LandData
{
    private int _landId;
    public int LandId
    {
        get => _landId;
        set => _landId = value;
    }

    private ELandType _landType;
    public ELandType LandType
    {
        get => _landType;
        set => _landType = value;
    }

    private long _landUnlockTime;
    public long LandUnlockTime
    {
        get => _landUnlockTime;
        set => _landUnlockTime = value;
    }

    private int _seedId;
    public int SeedId
    {
        get => _seedId;
        set => _seedId = value;
    }

    private long _startTime;
    public long StartTime
    {
        get => _startTime;
        set => _startTime = value;
    }

    private readonly List<long> _gainTimeList = new();
    public List<long> GainTimeList
    {
        get => _gainTimeList;
    }

    private readonly DirtyList<long> _stealPlayerList = new();
    public DirtyList<long> StealPlayerList
    {
        get => _stealPlayerList;
    }

    private readonly int _stealYieldCount;
    public int StealYieldCount
    {
        get => _stealYieldCount;
    }

    private long _nextWaterTime;
    public long NextWaterTime
    {
        get => _nextWaterTime;
        set => _nextWaterTime = value;
    }

    private int _waterCount;
    public int WaterCount
    {
        get => _waterCount;
        set => _waterCount = value;
    }

    private List<int> _fertilizerIdList = new();
    public List<int> FertilizerIdList
    {
        get => _fertilizerIdList;
        set => _fertilizerIdList = value;
    }

    private long _nextPestTime;
    public long NextPestTime
    {
        get => _nextPestTime;
        set => _nextPestTime = value;
    }

    private int _dePestCount;
    public int DePestCount
    {
        get => _dePestCount;
        set => _dePestCount = value;
    }
}

//通用的偷菜实体，记录 谁 什么时间 偷了什么 偷了多少
public class StealDataEntity : Fantasy.Entitas.Entity, ISupportedDataBase
{
    public long StealerPlayerId { get; set; } //小偷玩家ID
    public long TargetPlayerId { get; set; } //被偷玩家ID
    public long StealTime { get; set; } //偷取时间
    public int ItemId { get; set; } //偷取物品ID
    public int Count { get; set; } //偷取数量
}