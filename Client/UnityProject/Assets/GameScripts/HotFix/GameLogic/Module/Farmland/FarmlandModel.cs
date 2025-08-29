using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fantasy;
using Fantasy.Helper;
using GameConfig.farm;
using QFramework;
using TEngine;

namespace GameLogic
{

    public class FarmlandModel : AbstractModel
    {
        private readonly Dictionary<int, Farm_LandData> _landDataDic = new Dictionary<int, Farm_LandData>();

        public IReadOnlyDictionary<int, Farm_LandData> LandDataDic => _landDataDic;
        
        protected override void OnInit()
        {
            
        }

        public List<int> GetAllLandIds()
        {
            return new List<int>(_landDataDic.Keys);
        }
        
        public Farm_LandData GetLandData(int landId)
        {
            if (_landDataDic.TryGetValue(landId, out var landData))
            {
                return landData;
            }

            return null;
        }
        
        /// <summary>
        /// 用服务器数据初始化农田数据。
        /// </summary>
        /// <param name="landProtos"></param>
        public void SetFarmlandDatas(List<LandProto> landProtos)
        {
            foreach (var proto in landProtos)
            {
                var landData = new Farm_LandData(proto);
                _landDataDic[landData.LandId] = landData;
            }
            GameEvent.Send(EventID.FARMLAND_LAND_UPDATE_DATA_ALL);
        }

        /// <summary>
        /// 更新单个农田数据
        /// </summary>
        /// <param name="proto"></param>
        public void UpdateLandData(LandProto proto)
        {
            var landData = new Farm_LandData(proto);
            _landDataDic[landData.LandId] = landData;
            GameEvent.Send(EventID.FARMLAND_LAND_UPDATE_DATA, proto.landId);
        }
        
        /// <summary>
        /// 批量更新农田数据
        /// </summary>
        /// <param name="landProtos"></param>
        public void UpdateLandDatas(List<LandProto> landProtos)
        {
            foreach (var proto in landProtos)
            {
                UpdateLandData(proto);
            }
        }

        public void UpdateLandType(int landId, ELandType landType)
        {
            if (_landDataDic.TryGetValue(landId, out var landData))
            {
                landData.LandType = landType;
            }
        }
        
        /// <summary>
        /// 农田是否可以操作（修改土地类型，移除作物，种植、收获、施肥、浇水等）
        /// </summary>
        /// <param name="landId"></param>
        /// <returns></returns>
        public bool CanLandOperate(int landId)
        {
            if (_landDataDic.TryGetValue(landId, out var landData))
            {
                return landData.LandType != ELandType.Lock && landData.LandType != ELandType.Unlocking;
            }

            return false;
        }

        
        /// <summary>
        /// 是否可以收获
        /// </summary>
        /// <returns></returns>
        public bool CanGain(int landId)
        {
            if (!CanLandOperate(landId)) return false;
            var landData = _landDataDic[landId];
            if(landData.SeedId <= 0) return false;
            return TimeHelper.Now >= landData.HarvestTime;
        }
        
        /// <summary>
        /// 是否可以浇水
        /// </summary>
        /// <param name="landId"></param>
        /// <returns></returns>
        public bool CanWater(int landId)
        {
            if (!CanLandOperate(landId)) return false;
            var landData = _landDataDic[landId];
            if(landData.SeedId <= 0) return false;
            return TimeHelper.Now >= landData.NextWaterTime;
        }
        
        /// <summary>
        /// 是否可以施肥
        /// </summary>
        /// <param name="landId"></param>
        /// <returns></returns>
        public bool CanFertilize(int landId)
        {
            if (!CanLandOperate(landId)) return false;
            var landData = _landDataDic[landId];
            if(landData.SeedId <= 0) return false;
            return landData.MaxFertilizerCount > landData.FertilizerCount;
        }
        
        /// <summary>
        /// 是否可以除虫
        /// </summary>
        /// <param name="landId"></param>
        /// <returns></returns>
        public bool CanDepest(int landId)
        {
            if (!CanLandOperate(landId)) return false;
            var landData = _landDataDic[landId];
            if(landData.SeedId <= 0) return false;
            return TimeHelper.Now >= landData.NextPestTime;
        }

        /// <summary>
        /// 是否可以移除作物
        /// </summary>
        /// <param name="landId"></param>
        /// <returns></returns>
        public bool CanRemoveCrop(int landId)
        {
            if (!CanLandOperate(landId)) return false;
            var landData = _landDataDic[landId];
            return landData.SeedId > 0;
        }
    }
}