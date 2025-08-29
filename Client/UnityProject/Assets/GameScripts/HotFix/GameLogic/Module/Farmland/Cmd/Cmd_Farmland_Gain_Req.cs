using System.Collections.Generic;
using Fantasy;
using QFramework;
using TEngine;

namespace GameLogic
{
    /// <summary>
    /// 收菜,支持批量收菜
    /// </summary>
    public class Cmd_Farmland_Gain_Req : ICommand
    {
        public List<int> LandId;
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async void Execute()
        {
            var resp = await GameModule.Network.CallRoute<C2Center_Farmland_GainYield_Req, Center2C_Farmland_GainYield_Resp>(
                new C2Center_Farmland_GainYield_Req(){LandId = this.LandId});

            if (resp.ErrorCode != 0)
            {
                Log.Error("收菜失败, 错误码: " + resp.ErrorCode);
                return;
            }
            
            //更新农场和仓库数据
            var farmlandModel = this.GetModel<FarmlandModel>();
            farmlandModel.UpdateLandDatas(resp.LandDatas);
            var inventoryModel = this.GetModel<InventoryModel>();
            inventoryModel.UpdateItems(resp.CurItems);
        }
    }
}