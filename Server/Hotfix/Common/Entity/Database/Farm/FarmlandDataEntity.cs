using Fantasy;
using Fantasy.Entitas.Interface;
using Fantasy.Helper;
using GameConfig.farm;
using Hotfix.Common.Entity.Base;
using Hotfix.Common.Misc;
using Hotfix.Config;

namespace Hotfix;

public sealed class On_FarmlandDataEntity_AwakeSystem : AwakeSystem<FarmlandDataEntity>
{
    protected override void Awake(FarmlandDataEntity self)
    {
        
    }
}

public sealed class On_FarmlandDataEntity_Deserialize : DeserializeSystem<FarmlandDataEntity>
{
    protected override void Deserialize(FarmlandDataEntity self)
    {
        
    }
}

public class FarmlandDataEntity : PlayerDataBase
{
    enum ELandFriend
    {
        General = 0,
        Friendly = 1, // 土地友好
        Hostile = 2, // 土地不友好
    }
    
    private readonly Dictionary<string, Farm_LandData> _landDataDic = new Dictionary<string, Farm_LandData>();
    
    //-----------业务逻辑方法-----------
    private Farm_LandData? GetLandData(int landId)
    {
        string landIdStr = landId.ToString();
        if (!_landDataDic.ContainsKey(landIdStr)) return null;
        return _landDataDic[landIdStr];
    }

    public List<LandProto> GetLandDataList()
    {
        List<LandProto> landDataList = new List<LandProto>();
        foreach (var data in _landDataDic)
        {
            Farm_LandData landData = data.Value;
            LandProto landProto = GetLandProtoFromLandData(landData);
            landDataList.Add(landProto);
        }
        return landDataList;
    }
    
    public List<LandProto> GetLandDataList(List<int> landIds)
    {
        List<LandProto> landDataList = new List<LandProto>();
        foreach (var landId in landIds)
        {
            var landData = GetLandData(landId);
            if (landData != null)
            {
                LandProto landProto = GetLandProtoFromLandData(landData);
                landDataList.Add(landProto);
            }
        }
        return landDataList;
    }
    
    private LandProto GetLandProtoFromLandData(Farm_LandData landData)
    {
        return new LandProto
        {
            landId = landData.LandId,
            landType = (int)landData.LandType,
            seedId = landData.SeedId,
            startTime = landData.StartTime,
            harvestTime = landData.HarvestTime,
            yieldCount = landData.YieldCount,
            gainCount = landData.GainTimeList.Count,
            nextWaterTime = landData.NextWaterTime,
            fertilizerCount = landData.FertilizerCount,
            maxFertilizerCount = landData.MaxFertilizerCount,
            nextPestTime = landData.NextPestTime,
        };
    }

    //是否解锁，包括解锁中状态也算解锁
    public bool IsLandUnlocked(int landId)
    {
        string landIdStr = landId.ToString();
        return _landDataDic.ContainsKey(landIdStr);
    }
    //解锁
    public long UnlockLand(int landId)
    {
        string landIdStr = landId.ToString();
        if (IsLandUnlocked(landId))
        {
            return -1; // 地块已解锁
        }
        
        long currentTime = TimeHelper.Now;
        _landDataDic.Add(landIdStr, new Farm_LandData()
        {
            LandId = landId,
            LandType = ELandType.Unlocking, // 解锁中状态
            LandUnlockTime = currentTime,
        });

        SetDirty();
        return currentTime; // 成功解锁地块
    }
    //修改土地类型
    public bool ChangeLandType(int landId, ELandType landType)
    {
        if(!IsLandUnlocked(landId))
        {
            return false; // 地块未解锁
        }

        var landData = GetLandData(landId);
        if(landData == null || landData.LandType <= ELandType.Unlocking)
        {
            return false; // 地块不可用，可能是解锁中或未解锁
        }
        
        landData.LandType = landType;
        
        SetDirty();
        return true;
    }
    
    //土地是否空闲(已完成解锁，并且没有种植作物)
    public bool IsLandFree(int landId)
    {
        if (!IsLandUnlocked(landId)) return false;
        
        Farm_LandData landData = GetLandData(landId);
        if(landData == null || landData.LandType <= ELandType.Unlocking)
        {
            return false; // 地块不可用，可能是解锁中或未解锁
        }
        
        return landData.SeedId <= 0;
    }

    /// <summary>
    /// 获取最大浇水次数,只在种植时计算
    /// </summary>
    /// <param name="seedId"></param>
    /// <param name="landType"></param>
    /// <returns></returns>
    private int GetMaxWaterCount(int seedId, ELandType landType)
    {
        var configHelper = this.Scene.GetComponent<ConfigHelper>();
        var seedConfig = configHelper.SeedConfig.Get(seedId);
        if (seedConfig == null) return 0;
        var landConfig = configHelper.FarmlandTypeConfig.Get(landType);
        if (landConfig == null) return 0;
        int baseMaxWaterCount = seedConfig.MaxWaterCount;
        int landCountBuff = landConfig.WaterCountBuff;
        int finalMaxWaterCount = Math.Max(baseMaxWaterCount + landCountBuff, 0 );
        return finalMaxWaterCount;
    }
    
    /// <summary>
    /// 获取最大施肥次数,只在种植时计算
    /// </summary>
    /// <param name="seedId"></param>
    /// <param name="landType"></param>
    /// <returns></returns>
    private int GetMaxFertilizerCount(int seedId, ELandType landType)
    {
        var configHelper = this.Scene.GetComponent<ConfigHelper>();
        var seedConfig = configHelper.SeedConfig.Get(seedId);
        if (seedConfig == null) return 0;
        var landConfig = configHelper.FarmlandTypeConfig.Get(landType);
        if (landConfig == null) return 0;
        int baseMaxFertilizerCount = seedConfig.MaxFertilizerCount;
        int landCountBuff = landConfig.FertilizerCountBuff;
        int finalMaxFertilizerCount = Math.Max(baseMaxFertilizerCount + landCountBuff, 0 );
        return finalMaxFertilizerCount;
    }
    
    /// <summary>
    /// 种植
    /// </summary>
    /// <param name="landId"></param>
    /// <param name="seedId"></param>
    /// <returns></returns>
    public LandProto? Seed(int landId, int seedId)
    {
        if (!IsLandFree(landId))
        {
            return null; // 地块不可用或已种植作物
        }
        
        var landData = GetLandData(landId);
        if (landData == null) return null;
        landData.SeedId = seedId;
        landData.StartTime = TimeHelper.Now;

        var seedConfig = this.Scene.GetComponent<ConfigHelper>().SeedConfig;
        var cfg = seedConfig.Get(seedId);
        if (cfg == null) return null;
        
        //计算收获时间
        landData.HarvestTime = GetInitHarvestTime(seedId, landId);
        
        //获取初始产量
        landData.YieldCount = GetInitYieldCount(seedId, landId);
        
        //计算稀有收获概率
        landData.RareRate = cfg.RareRate;
        
        //计算最大浇水次数
        landData.MaxWaterCount = GetMaxWaterCount(seedId, landData.LandType);
        
        //计算最大施肥次数
        landData.MaxFertilizerCount = GetMaxFertilizerCount(seedId, landData.LandType);
        
        //计算最大除虫次数
        landData.MaxFertilizerCount = cfg.MaxFertilizerCount;
        
        //随机出下次需要浇水的时间
        if (cfg.MaxWaterCount > 0)
        {
            landData.NextWaterTime = GetNextWaterTime(seedId);
        }

        //下次虫害时间
        if (cfg.MaxPestCount > 0)
        {
            if (IsSpawnPest(cfg.PestRate))
            {
                long growTime = landData.HarvestTime - landData.StartTime;
                landData.NextPestTime = GetNextPestTime(seedId, growTime);
            }
        }
        var landProto = GetLandProtoFromLandData(landData);
        return landProto;
    }
    
    /// <summary>
    /// 获取初始产量
    /// </summary>
    /// <param name="seedId"></param>
    /// <param name="landId"></param>
    /// <returns></returns>
    /// 影响因素：基础产量，雇员加成，土地效果加成，是否土地友好加成，全局加成
    private int GetInitYieldCount(int seedId, int landId, int gainCount = 0)
    {
        var configHelper = this.Scene.GetComponent<ConfigHelper>();
        var seedConfig = configHelper.SeedConfig.Get(seedId);
        if (seedConfig == null) return -1;
        
        //随机出初始产量
        var yieldCountRange = seedConfig.YieldCount;
        int baseYieldCount = RandomHelper.RandomNumber(yieldCountRange.MinCount, yieldCountRange.MaxCount + 1);
        if (gainCount != 0)
        {
            if (gainCount >= seedConfig.MutiHarvestCount ||
                seedConfig.MutiHarvestYieldCountChangeRate.Contains(gainCount - 1))
            {
                Log.Error("GetInitYieldCount gainCount error: seedId={0}, gainCount={1}", seedId, gainCount);
                return 0;
            }

            int yieldCountChangeRate = seedConfig.MutiHarvestYieldCountChangeRate[gainCount - 1];
            float changeRate = GameHelper.IntRateToFloat(yieldCountChangeRate);
            baseYieldCount = (int)Math.Floor(baseYieldCount * (1 + changeRate));
        }
        
        //TODO 雇员加成
        float roleBuff = 1.0f;
        
        //土地效果加成
        var landconfig = configHelper.FarmlandTypeConfig.Get((ELandType)landId);
        if (landconfig == null) return -1;
        float landBuff = 1 + GameHelper.IntRateToFloat(landconfig.YieldCountBuff);
        
        //土地效果加成
        var landFriendly = GetLandFriendly(seedId, landId);
        var friendlyBuffCfg = configHelper.GlobalConfig.FriendlyLandYieldCountBuff;
        var hostileBuffCfg = configHelper.GlobalConfig.HostileLandYieldCountBuff;
        int friendlyBuffValue = landFriendly == ELandFriend.Friendly ? friendlyBuffCfg : 
            landFriendly == ELandFriend.Hostile ? hostileBuffCfg : 0;
        float friendlyBuff = 1 + GameHelper.IntRateToFloat(friendlyBuffValue);
        
        //全局加成
        float globalBuff = 1.0f;
        
        //计算最终产量
        int finalYieldCount = (int)Math.Floor(baseYieldCount * roleBuff *
            landBuff * friendlyBuff * globalBuff);
        
        return finalYieldCount;
    }

    private ELandFriend GetLandFriendly(int seedId, int landId)
    {
        var configHelper = this.Scene.GetComponent<ConfigHelper>();
        var seedConfig = configHelper.SeedConfig.Get(seedId);
        if (seedConfig == null) return ELandFriend.General;
        var landConfig = configHelper.FarmlandTypeConfig.Get((ELandType)landId);
        if (landConfig == null) return ELandFriend.General;
        var seedType = seedConfig.SeedType;
        if (landConfig.Friendly.Contains(seedType))
        {
            return ELandFriend.Friendly; // 相性友好
        }
        
        if (landConfig.Hostile.Contains(seedType))
        {
            return ELandFriend.Hostile; // 相性不友好
        }
        
        return ELandFriend.General; // 相性一般
    }

    /// <summary>
    /// 下次需要浇水的时间戳
    /// </summary>
    /// <param name="seedId"></param>
    /// <returns></returns>
    private long GetNextWaterTime(int seedId)
    {
        var seedConfig = this.Scene.GetComponent<ConfigHelper>().SeedConfig;
        var cfg = seedConfig.Get(seedId);
        if (cfg == null) return -1;
        var needWaterTime = cfg.NeedWaterTime;
        if(needWaterTime == null || needWaterTime.Count != 2)
        {
            return -1;
        }
        
        return TimeHelper.Now + RandomHelper.RandomNumber(needWaterTime[0], needWaterTime[1]+1);
    }

    /// <summary>
    /// 是否会出现虫害
    /// </summary>
    /// <returns></returns>
    private bool IsSpawnPest(int pestRate)
    {
        if (pestRate <= 0) return false; // 不会出现虫害
        if (pestRate >= 100) return true; // 一定会出现虫害
        
        return RandomHelper.RandomNumber(0, 100) < pestRate; // 根据概率判断是否出现虫害
    }

    /// <summary>
    /// 下次需要除虫的时间戳
    /// </summary>
    /// <param name="seedId"></param>
    /// <returns></returns>
    /// 除虫时间是随机的，范围在0到基础成熟时间之间
    private long GetNextPestTime(int seedId, long growTime)
    {
        var needPestTime = (long)(RandomHelper.RandFloat01() * growTime);
        
        return TimeHelper.Now + needPestTime;
    }


    /// <summary>
    /// 获取收获时间
    /// </summary>
    /// <param name="seedId">种子ID</param>
    /// <param name="landId">土地ID</param>
    /// <param name="gainCount">已收获次数</param>
    /// <returns></returns>
    /// 考虑因素：基础成熟时间, 土地类型增益
    /// TODO:雇员增益,全局增益
    private long GetInitHarvestTime(int seedId, int landId, int gainCount = 0)
    {
        var configHelper = this.Scene.GetComponent<ConfigHelper>();
        var seedConfig = configHelper.SeedConfig.Get(seedId);
        if (seedConfig == null) return -1;
        var landConfig = configHelper.FarmlandTypeConfig.Get((ELandType)landId);
        if (landConfig == null) return -1;

        int baseHarvestTime = -1;
        if (gainCount == 0)
        {
            baseHarvestTime = seedConfig.GrowTime;
        }
        else
        {
            if (!seedConfig.MutiHarvestTime.Contains(gainCount - 1) ) return -1;
            baseHarvestTime = seedConfig.MutiHarvestTime[gainCount - 1];
        }
        
        //土地类型增益
        float landBuff = 1 + GameHelper.IntRateToFloat(landConfig.GrowTimeBuff);
        long finalGrowTime = (long) Math.Max( baseHarvestTime * landBuff, 0);
        
        return finalGrowTime + TimeHelper.Now;
    }

    /// <summary>
    /// 是否可以收获
    /// </summary>
    /// <param name="landId"></param>
    /// <returns></returns>
    private bool CanHarvest(int landId)
    {
        var landData = GetLandData(landId);
        if (landData == null) return false; // 地块未解锁或不存在
        if (landData.LandType <= ELandType.Unlocking) return false; // 地块不可用，可能是解锁中或未解锁
        if (landData.SeedId <= 0) return false; // 地块没有种植作物
        return landData.HarvestTime <= TimeHelper.Now; // 检查是否到达收获时间
    }

    
    /// <summary>
    /// 清除作物
    /// </summary>
    /// <param name="landId"></param>
    /// <returns></returns>
    public LandProto? ClearSeed(int landId)
    {
        if(!IsLandUnlocked(landId)) return null;
        if(IsLandFree(landId)) return null; // 地块未种植作物
        var landData = GetLandData(landId);
        if(landData == null) return null;
        landData.SeedId = 0; // 清除种子ID
        landData.StartTime = 0; // 清除种植开始时间
        landData.HarvestTime = 0; // 清除收获时间
        landData.YieldCount = 0; // 清除产量
        landData.YieldRate = 0; // 清除产量倍率
        landData.GainTimeList.Clear(); // 清除收获时间列表
        landData.RareRate = 0; // 清除稀有收获概率
        landData.NextWaterTime = 0; // 清除下次浇水时间
        landData.WaterCount = 0; // 清除浇水次数
        landData.NextPestTime = 0; // 清除下次除虫时间
        landData.FertilizerCount = 0; // 清除肥料列表
        landData.StealPlayerList.Clear(); // 清除偷窃玩家列表
        landData.StealYieldCount = 0; // 清除偷窃产量
        landData.PestCount = 0; // 清除除虫次数
        landData.MaxWaterCount = 0;
        landData.MaxFertilizerCount = 0; // 清除最大施肥次数
        landData.MaxPestCount = 0; // 清除最大虫害次数
        
        SetDirty();
        return GetLandProtoFromLandData(landData);
    }

    /// <summary>
    /// 多季作物收获后重置数据并添加新数据
    /// </summary>
    /// <param name="landId"></param>
    /// <returns></returns>
    private bool ResetMutiHarvest(int landId)
    {
        if(!IsLandUnlocked(landId)) return false;
        if(IsLandFree(landId)) return false; // 地块未种植作物
        var landData = GetLandData(landId);
        if(landData == null) return false;
        
        //清除操作记录
        landData.WaterCount = 0; // 清除浇水次数
        landData.FertilizerCount = 0; // 清除肥料列表
        landData.PestCount = 0; // 清除除虫次数
        
        //重新计算成熟数据
        int seedId = landData.SeedId;
        int gainCount = landData.GainTimeList.Count;
        landData.HarvestTime = GetInitHarvestTime(seedId, landId, gainCount);
        landData.YieldCount = GetInitYieldCount(seedId, landId, gainCount);
        landData.YieldRate = 0; // 清除产量倍率
        var configHelper = this.Scene.GetComponent<ConfigHelper>();
        var seedConfig = configHelper.SeedConfig.Get(seedId);
        landData.RareRate = seedConfig.RareRate; // 重新计算稀有收获概率
        long growTime = landData.HarvestTime - landData.StartTime;
        
        //重新计算下次浇水和除虫时间
        if (landData.MaxWaterCount > 0)
        {
            landData.NextWaterTime = GetNextWaterTime(seedId); // 下次浇水时间
        }

        if (landData.MaxPestCount > 0)
        {
            if (IsSpawnPest(seedConfig.PestRate))
            {
                landData.NextPestTime = GetNextPestTime(seedId, growTime); // 下次除虫时间
            }
        }
        
        SetDirty(); // 标记数据为脏数据，需保存
        return true;
    }

    /// <summary>
    /// 收获完成后处理,单季作物清除，多季作物重置数据
    /// </summary>
    /// <param name="landId"></param>
    private void ResetAfterGain(int landId)
    {
        var landData = GetLandData(landId);
        if(landData == null) return;
        int seedId = landData.SeedId;
        var configHelper = this.Scene.GetComponent<ConfigHelper>();
        var seedCfg = configHelper.SeedConfig.Get(seedId);
        if (seedCfg == null) return;
        int curGrowCount = landData.GainTimeList.Count;
        int cfgMaxGrowCount = seedCfg.MutiHarvestCount;
        if (curGrowCount >= cfgMaxGrowCount) //已达到最大收获次数，清除作物
        {
            ClearSeed(landId);
        }
        else //多季作物未达到最大收获次数，可继续成长
        {
            ResetMutiHarvest(landId);
        }
    }

    private bool IsRareGain(int landId)
    {
        var landData = GetLandData(landId);
        if (landData == null) return false;
        //判断是否为稀有收获
        var rareRate = landData.RareRate;
        if (rareRate <= 0) return false;
        
        return RandomHelper.RandomNumber(0, 100) < rareRate * 100; // 根据概率判断是否为稀有收获
    }
    
    /// <summary>
    /// 收获 单个地块
    /// </summary>
    /// <param name="landId"></param>
    /// <returns></returns>
    private GainProto? Gain(int landId)
    {
        if (!IsLandUnlocked(landId)) return null;
        if(!CanHarvest(landId)) return null;
        var landData = GetLandData(landId);
        if(landData == null) return null;
        int seedId = landData.SeedId;
        var configHelper = this.Scene.GetComponent<ConfigHelper>();
        var seedCfg = configHelper.SeedConfig.Get(seedId);
        if (seedCfg == null) return null;

        List<ItemProto> gainItems = new List<ItemProto>();
        int gainItemId = seedCfg.YieldCount.Id;
        int gainItemCount = landData.YieldCount;
        gainItems.Add(new ItemProto
        {
            Id = gainItemId,
            Count = gainItemCount
        });

        if (IsRareGain(landId)) //如果触发稀有收获
        {
            int rareItemId = seedCfg.RareYieldCount.Id;
            int rareItemCount = RandomHelper.RandomNumber(seedCfg.RareYieldCount.MinCount, seedCfg.RareYieldCount.MaxCount +1);
            float yieldCountBuff = landData.YieldRate;//施肥带来的产量buff
            if (yieldCountBuff > 0)
            {
                rareItemCount = (int)Math.Floor(rareItemCount * (1 + yieldCountBuff));
            }
            
            gainItems.Add(new ItemProto
            {
                Id = rareItemId,
                Count = rareItemCount
            });
        }
        
        GainProto gainProto = new GainProto
        {
            fromId = landId,
            items = gainItems
        };

        landData.GainTimeList.Add(TimeHelper.Now);
        ResetAfterGain(landId); // 收获完成后处理
        SetDirty(); // 标记数据为脏数据，需保存
        return gainProto; // 返回收获的物品
    }

    /// <summary>
    /// 收获
    /// </summary>
    /// <param name="landIds"></param>
    /// <returns></returns>
    public (List<GainProto>, List<LandProto>) Gain(List<int> landIds)
    {
        List<GainProto> gainItemsList = new List<GainProto>();
        List<int> successGainLandIds = new List<int>();
        foreach (var landId in landIds)
        {
            var gainItem = Gain(landId);
            if (gainItem != null)
            {
                gainItemsList.Add(gainItem);
                successGainLandIds.Add(landId);
            }
        }
        if (gainItemsList.Count > 0)
        {
            SetDirty(); // 标记数据为脏数据，需保存
        }

        var newLandDatas = GetLandDataList(successGainLandIds);
        return (gainItemsList, newLandDatas);
    }


    /// <summary>
    /// 土地是否可以操作(浇水，施肥，除虫等)
    /// </summary>
    /// <param name="landId"></param>
    /// <returns></returns>
    private bool CanLandOperate(int landId)
    {
        if(!IsLandUnlocked(landId)) return false; // 土地未解锁
        var landData = GetLandData(landId);
        if(landData == null) return false; // 土地数据不存在
        if(landData.LandType <= ELandType.Unlocking) return false; // 土地未解锁或解锁中
        if(landData.SeedId <= 0) return false; // 土地未种植作物
        if(landData.HarvestTime <= TimeHelper.Now) return false; // 土地已成熟，不能操作
        return true; // 土地可以操作
    }

    /// <summary>
    /// 浇水
    /// </summary>
    /// <param name="landId"></param>
    /// <returns></returns>
    public LandProto? Water(int landId)
    {
        if (!CanLandOperate(landId)) return null;
        var landData = GetLandData(landId);
        int curWaterCount = landData.WaterCount;
        int seedId = landData.SeedId;
        var configHelper = this.Scene.GetComponent<ConfigHelper>();
        var seedConfig = configHelper.SeedConfig.Get(seedId);
        if (seedConfig == null) return null;
        if (curWaterCount >= seedConfig.MaxWaterCount)
        {
            landData.NextWaterTime = 0;
            Log.Error( "FarmlandDataEntity: Water failed, already reached max water count: landId={0}, seedId={1}, curWaterCount={2}",
                landId, seedId, curWaterCount);
            return GetLandProtoFromLandData(landData); // 已经浇水次数已达上限
        }
        //检查浇水时间
        if (TimeHelper.Now < landData.NextWaterTime)
        {
            return null; // 还未到下次浇水时间
        }

        //浇水成功
        landData.WaterCount++;
        if(landData.WaterCount >= landData.MaxWaterCount)
        {
            landData.NextWaterTime = 0; // 已经浇水次数已达上限
        }
        else
        {
            landData.NextWaterTime = GetNextWaterTime(seedId); // 重新计算下次浇水时间
        }
        
        //计算浇水增益 基础 土地效果 全局
        int baseSpeedUpTime = configHelper.GlobalConfig.WaterSpeedUpGrowTime;
        var landType = landData.LandType;
        var landConfig = configHelper.FarmlandTypeConfig.Get(landType);
        int landBuff = landConfig.WaterBuff;
        float landBuffRate = GameHelper.IntRateToFloat(landBuff);
        int finalSpeedUpTime = (int)Math.Floor(baseSpeedUpTime * (1 + landBuffRate));
        ChangeHarvestTime(landData, -finalSpeedUpTime); // 缩短成熟时间
        return GetLandProtoFromLandData(landData);
    }

    /// <summary>
    /// 调整成熟时间 (浇水，施肥等操作后)
    /// </summary>
    /// <param name="landData"></param>
    /// <param name="changeTime"></param>
    private void ChangeHarvestTime(Farm_LandData landData, int changeTime)
    {
        landData.HarvestTime += changeTime; // 调整成熟时间
    }
    
    /// <summary>
    /// 调整产量倍率 (施肥等操作后)
    /// </summary>
    /// <param name="landData"></param>
    /// <param name="changeRate"></param>
    private void ChangeYieldRate(Farm_LandData landData, float changeRate)
    {
        landData.YieldRate += changeRate; // 调整产量倍率
    }
    
    /// <summary>
    ///调整产量
    /// </summary>
    /// <param name="landData"></param>
    /// <param name="changeCount"></param>
    private void ChanegYieldCount(Farm_LandData landData, int changeCount)
    {
        landData.YieldCount += changeCount; // 调整产量
        if (landData.YieldCount < 0) landData.YieldCount = 0; // 确保产量不为负数
    }
    
    private void ChangeRareRate(Farm_LandData landData, float changeRate)
    {
        landData.RareRate += changeRate; // 调整稀有率
        if (landData.RareRate < 0) landData.RareRate = 0; // 确保稀有率不为负数
        if (landData.RareRate > 1) landData.RareRate = 1; // 确保稀有率不超过1
    }

    /// <summary>
    /// 请求施肥
    /// </summary>
    /// <param name="landId"></param>
    /// <param name="fertilizerId"></param>
    /// <returns></returns>
    public LandProto? Fertilizer(int landId, int fertilizerId)
    {
        if(!CanLandOperate(landId)) return null; // 土地不可操作
        var landData = GetLandData(landId);
        if (landData.FertilizerCount >= landData.MaxFertilizerCount)
        {
            return null; // 已经施肥次数已达上限
        }
        //成功施肥
        landData.FertilizerCount++;
        //计算施肥增益 基础 土地效果
        var configHelper = this.Scene.GetComponent<ConfigHelper>();
        var fertilizerCfg = configHelper.FertilizerConfig.Get(fertilizerId);
        if(fertilizerCfg == null)
        {
            return null; // 肥料不存在
        }
        var landConfig = configHelper.FarmlandTypeConfig.Get(landData.LandType);
        if (landConfig == null)
        {
            return null; // 土地类型配置不存在
        }

        int baseGrowTimeBuff = fertilizerCfg.GrowTimeBuff;
        float baseYieldCountBuff = GameHelper.IntRateToFloat(fertilizerCfg.YieldCountBuff);
        float baseRareBuff = GameHelper.IntRateToFloat(fertilizerCfg.RareBuff);

        int landFertilizerBuff = landConfig.FertilizerBuff;
        //生长时间变化
        if (baseGrowTimeBuff != 0)
        {
            baseGrowTimeBuff *= (int)(1 + GameHelper.IntRateToFloat(landFertilizerBuff));
            ChangeHarvestTime(landData, -baseGrowTimeBuff);
        }
        // 产量变化
        if (baseYieldCountBuff != 0)
        {
            baseYieldCountBuff *= (1 + GameHelper.IntRateToFloat(landFertilizerBuff));
            ChangeYieldRate(landData, baseYieldCountBuff);
            int changeYieldCount = (int)Math.Floor(landData.YieldCount * baseYieldCountBuff);
            ChanegYieldCount(landData, changeYieldCount);
        }
        // 稀有率变化
        if (baseRareBuff != 0)
        {
            baseRareBuff *= (1 + GameHelper.IntRateToFloat(landFertilizerBuff));
            ChangeRareRate(landData, baseRareBuff);
        }
        SetDirty(); // 标记数据为脏数据，需保存
        return GetLandProtoFromLandData(landData); // 返回更新后的土地数据
    }

    /// <summary>
    /// 杀虫
    /// </summary>
    /// <param name="landId"></param>
    /// <param name="depesterId"></param>
    /// <returns></returns>
    public LandProto? DePest(int landId, int depesterId)
    {
        if (!CanLandOperate(landId)) return null;
        var landData = GetLandData(landId);
        var configHelper = this.Scene.GetComponent<ConfigHelper>();
        var depesterCfg = configHelper.DepesterConfig.Get(depesterId);
        var rareRateBuff = depesterCfg.RareRateBuff;
        var pestRateBuff = depesterCfg.PestRateBuff;
        ChangeRareRate(landData, rareRateBuff);
        landData.PestCount++;
        
        int seedId = landData.SeedId;
        var seedCfg = configHelper.SeedConfig.Get(seedId);
        var basePestRate = seedCfg.PestRate;
        basePestRate += pestRateBuff;
        //是否出现新虫害
        if (landData.PestCount >= landData.MaxPestCount)
        {
            if (IsSpawnPest(basePestRate))
            {
                landData.NextPestTime = GetNextPestTime(seedId, landData.HarvestTime - TimeHelper.Now);
            }
        }

        return GetLandProtoFromLandData(landData);
    }
}