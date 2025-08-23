using System.Collections.Generic;
using Fantasy;
using Fantasy.Entitas.Interface;
using GameConfig.farm;

namespace GameLogic
{
    
    public class Farm_LandData
    {
        public Farm_LandData(LandProto proto)
        {
            LandId = proto.landId;
            LandType = (ELandType)proto.landType;
            LandUnlockTime = proto.unlockTime;
            SeedId = proto.seedId;
            StartTime = proto.startTime;
            HarvestTime = proto.harvestTime;
            YieldCount = proto.yieldCount;
            GainCount = proto.gainCount;
            NextWaterTime = proto.nextWaterTime;
            FertilizerCount = proto.fertilizerCount;
            MaxFertilizerCount = proto.maxFertilizerCount;
            NextPestTime = proto.nextPestTime;
        }
        
        /// <summary>
        /// 土地ID
        /// </summary>
        private int _landId;
        public int LandId
        {
            get => _landId;
            set => _landId = value;
        }

        /// <summary>
        /// 土地类型
        /// </summary>
        private ELandType _landType;
        public ELandType LandType
        {
            get => _landType;
            set => _landType = value;
        }

        /// <summary>
        /// 土地开始执行解锁的时间戳
        /// </summary>
        private long _landUnlockTime;
        public long LandUnlockTime
        {
            get => _landUnlockTime;
            set => _landUnlockTime = value;
        }

        /// <summary>
        /// 种子ID
        /// </summary>
        private int _seedId;
        public int SeedId
        {
            get => _seedId;
            set => _seedId = value;
        }

        /// <summary>
        /// 开始种植的时间戳
        /// </summary>
        private long _startTime;
        public long StartTime
        {
            get => _startTime;
            set => _startTime = value;
        }
        
        /// <summary>
        /// 预计收获的时间戳
        /// </summary>
        private long _harvestTime;
        public long HarvestTime
        {
            get => _harvestTime;
            set => _harvestTime = value;
        }

        /// <summary>
        /// 预计产量
        /// </summary>
        private int _yieldCount;
        public int YieldCount
        {
            get => _yieldCount;
            set => _yieldCount = value;
        }

        // /// <summary>
        // /// 稀有收获概率
        // /// </summary>
        // private float _rareRate;
        // public float RareRate
        // {
        //     get => _rareRate;
        //     set => _rareRate = value;
        // }
        //
        // /// <summary>
        // /// 产量倍率，只给稀有收获使用
        // /// </summary>
        // private float _yieldRate;
        // public float YieldRate
        // {
        //     get => _yieldRate;
        //     set => _yieldRate = value;
        // }

        /// <summary>
        /// 最近一次收获的时间戳
        /// </summary>
        private long _lastGainTime;
        public long LastGainTime
        {
            get => _lastGainTime;
            set => _lastGainTime = value;
        }

        /// <summary>
        /// 已收获次数
        /// </summary>
        private int _gainCount;
        public int GainCount
        {
            get => _gainCount;
            set => _gainCount = value;
        }

        // private readonly List<long> _stealPlayerList = new();
        // public List<long> StealPlayerList
        // {
        //     get => _stealPlayerList;
        // }
        //
        // private int _stealYieldCount;
        // public int StealYieldCount
        // {
        //     get => _stealYieldCount;
        //     set => _stealYieldCount = value;
        // }

        private long _nextWaterTime;
        public long NextWaterTime
        {
            get => _nextWaterTime;
            set => _nextWaterTime = value;
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

    }
    
}
