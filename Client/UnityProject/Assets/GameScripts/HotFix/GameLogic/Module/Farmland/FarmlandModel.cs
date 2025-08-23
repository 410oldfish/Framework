using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fantasy;
using GameConfig.farm;
using QFramework;

namespace GameLogic
{

    public class FarmlandModel : AbstractModel
    {
        private readonly Dictionary<int, Farm_LandData> _landDataDic = new Dictionary<int, Farm_LandData>();

        public IReadOnlyDictionary<int, Farm_LandData> LandDataDic => _landDataDic;
        
        protected override void OnInit()
        {
            
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
        }
    }
}