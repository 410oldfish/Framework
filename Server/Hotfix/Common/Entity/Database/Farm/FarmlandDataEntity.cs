using Fantasy;
using Fantasy.Entitas.Interface;
using Fantasy.Helper;
using GameConfig.farm;
using Hotfix.Common.Entity.Base;
using Hotfix.Common.Misc;
using Hotfix.Common.Misc.CustomClass;

namespace Hotfix.Common.Entity.Database;

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
    private readonly Dictionary<string, Farm_LandData> _landDataDic = new Dictionary<string, Farm_LandData>();
    
    //-----------业务逻辑方法-----------
    public List<LandProto> GetLandData()
    {
        List<LandProto> landDataList = new List<LandProto>();
        foreach (var data in _landDataDic)
        {
            Farm_LandData landData = data.Value;
            LandProto landProto = new LandProto
            {
                landId = landData.LandId,
                landType = (int)landData.LandType,
                seedId = landData.SeedId,
                startTime = landData.StartTime,
                gainCount = landData.GainTimeList.Count,
                lastGainTime = landData.GainTimeList.Count > 0 ? landData.GainTimeList[^1] : 0,
                nextWaterTime = landData.NextWaterTime,
                waterCount = landData.WaterCount,
                fertilizerId = landData.FertilizerIdList,
                nextPestTime = landData.NextPestTime,
                dePestCount = landData.DePestCount,
            };
            landDataList.Add(landProto);
        }
        return landDataList;
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
        
        long currentTime = GameHelper.GetCurrentTimeStamp();
        _landDataDic[landIdStr] = new Farm_LandData()
        {
            LandId = landId,
            LandType = ELandType.UNLOCKING, // 解锁中状态
            LandUnlockTime = currentTime,
        };
        
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
        
        string landIdStr = landId.ToString();
        _landDataDic[landIdStr].LandType = landType;
        SetDirty();
        return true;
    }
    
    //土地是否空闲(已完成解锁，并且没有种植作物)
    public bool IsLandFree(int landId)
    {
        string landIdStr = landId.ToString();
        if (!_landDataDic.ContainsKey(landIdStr))
        {
            return false; // 地块未解锁
        }
        
        Farm_LandData landData = _landDataDic[landIdStr];
        if(landData.LandType <= ELandType.UNLOCKING)
        {
            return false; // 地块不可用，可能是解锁中或未解锁
        }
        
        return landData.SeedId == -1;
    }

    /// <summary>
    /// 种植
    /// </summary>
    /// <param name="landId"></param>
    /// <param name="seedId"></param>
    /// <returns></returns>
    public LandProto Seed(int landId, int seedId)
    {
        if (!IsLandFree(landId))
        {
            return null; // 地块不可用或已种植作物
        }
        
        string landIdStr = landId.ToString();
        var landData = _landDataDic[landIdStr];
        landData.SeedId = seedId;
        landData.StartTime = TimeHelper.Now;
    }
}