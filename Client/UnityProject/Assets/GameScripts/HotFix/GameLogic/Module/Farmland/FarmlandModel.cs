using System.Collections.Generic;
using GameConfig.farm;
using QFramework;

namespace GameLogic
{
    //农作物种植数据
    public class CropsData
    {
        //get set
        public int CfgId { get; set; } //种子ID

        public long StartTime { get; set; } //开始种植时间
        
        public int GainCount; //收获次数

        public long LastGainTime { get; set; } //上次收获时间

        public string StealPlayerListStr { get; set; } //偷取玩家列表,到客户端解析成List<long>

        public int YieldCount { get; set; } //产量
    }
    public class FarmlandModel : AbstractModel
    {
        //田地信息 0未解锁 1~ 土地种类
        Dictionary<int, ELandType> farmlandDic = new Dictionary<int, ELandType>();
        //田地种植数据
        Dictionary<int, CropsData> farmlandDataDic = new Dictionary<int, CropsData>();
        
        protected override void OnInit()
        {
            
        }
    }
}