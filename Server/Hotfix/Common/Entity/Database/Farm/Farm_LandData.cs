using Fantasy.Entitas.Interface;
using GameConfig.farm;
using Hotfix.Common.Misc.CustomClass;

namespace Hotfix;


/// <summary>
/// landId 唯一标识一个土地
/// landType 土地类型
/// landUnlockTime 土地解锁时间
/// seedId 种子ID
/// startTime 种植开始时间
/// harvestTime 收获时间
/// yieldCount 产量
/// rareRate 稀有率
/// yieldRate 产量倍率
/// gainTimeList 收获时间列表
/// stealPlayerList 偷菜玩家列表
/// stealYieldCount 偷菜产量
/// nextWaterTime 下次浇水时间
/// waterCount 浇水次数
/// maxWaterCount 最大浇水次数
/// fertilizerCount 施肥次数
/// maxFertilizerCount 最大施肥次数
/// nextPestTime 下次除虫时间
/// pestCount 除虫次数
/// maxPestCount 最大除虫次数
/// </summary>
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
    
    private long _harvestTime;
    public long HarvestTime
    {
        get => _harvestTime;
        set => _harvestTime = value;
    }

    private int _yieldCount;
    public int YieldCount
    {
        get => _yieldCount;
        set => _yieldCount = value;
    }

    private float _rareRate;
    public float RareRate
    {
        get => _rareRate;
        set => _rareRate = value;
    }
    
    /// <summary>
    /// 产量倍率，只给稀有收获使用
    /// </summary>
    private float _yieldRate;
    public float YieldRate
    {
        get => _yieldRate;
        set => _yieldRate = value;
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

    private int _stealYieldCount;
    public int StealYieldCount
    {
        get => _stealYieldCount;
        set => _stealYieldCount = value;
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
    
    private int _maxWaterCount;
    public int MaxWaterCount
    {
        get => _maxWaterCount;
        set => _maxWaterCount = value;
    }

    private int _fertilizerCount;
    public int FertilizerCount
    {
        get => _fertilizerCount;
        set => _fertilizerCount = value;
    }
    
    private int _maxFertilizerCount;
    public int MaxFertilizerCount
    {
        get => _maxFertilizerCount;
        set => _maxFertilizerCount = value;
    }

    private long _nextPestTime;
    public long NextPestTime
    {
        get => _nextPestTime;
        set => _nextPestTime = value;
    }

    private int _pestCount;
    public int PestCount
    {
        get => _pestCount;
        set => _pestCount = value;
    }
    
    private int _maxPestCount;
    public int MaxPestCount
    {
        get => _maxPestCount;
        set => _maxPestCount = value;
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