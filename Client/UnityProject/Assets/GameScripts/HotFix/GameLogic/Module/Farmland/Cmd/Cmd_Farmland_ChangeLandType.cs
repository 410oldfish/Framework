using Fantasy;
using GameConfig.farm;
using QFramework;
using TEngine;

namespace GameLogic
{
    /// <summary>
    /// 改变农田地块类型
    /// </summary>
    public class Cmd_Farmland_ChangeLandType : ICommand
    {
        public int LandId;
        public ELandType LandType;
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async void Execute()
        {
            var resp = await GameModule.Network.CallRoute<C2Center_Farmland_ChangeLandType_Req, Center2C_Farmland_ChangeLandType_Resp>(
                new C2Center_Farmland_ChangeLandType_Req(){LandId = this.LandId});

            if (resp.ErrorCode != 0)
            {
                Log.Error("改变地块类型失败, 错误码: " + resp.ErrorCode);
                return;
            }
            var farmlandModel = this.GetModel<FarmlandModel>();
            farmlandModel.UpdateLandType(LandId, LandType);
            var inventoryModel = this.GetModel<InventoryModel>();
            inventoryModel.UpdateItems(resp.CurItems);
        }
    }
}