using Fantasy;
using QFramework;
using TEngine;

namespace GameLogic
{
    /// <summary>
    /// 请求解锁农田地块
    /// </summary>
    public class Cmd_Farmland_UnlockBlock_Req : ICommand
    {
        public int LandId;

        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async void Execute()
        {
            var resp = await GameModule.Network.CallRoute<C2Center_Farmland_UnlockLand_Req, Center2C_Farmland_UnlockLand_Resp>(
                new C2Center_Farmland_UnlockLand_Req(){LandId = this.LandId});

            if(resp.ErrorCode != 0)
            {
                Log.Error("解锁地块失败, 错误码: " + resp.ErrorCode);
                return;
            }
            var farmlandModel = this.GetModel<FarmlandModel>();
            farmlandModel.UpdateLandData(resp.LandData);
            var inventoryModel = this.GetModel<InventoryModel>();
            inventoryModel.UpdateItems(resp.CurItems);
        }
    }
}