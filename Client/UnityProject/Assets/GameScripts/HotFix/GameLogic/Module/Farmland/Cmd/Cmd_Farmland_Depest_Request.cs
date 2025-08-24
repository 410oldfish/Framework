using Fantasy;
using QFramework;
using TEngine;

namespace GameLogic
{
    /// <summary>
    /// 除虫请求
    /// </summary>
    public class Cmd_Farmland_Depest_Request : ICommand
    {
        public int LandId;
        public int DePesterId;
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async void Execute()
        {
            var resp = await GameModule.Network.CallRoute<C2Center_Farmland_DePest_Req, Center2C_Farmland_DePest_Resp>(
                new C2Center_Farmland_DePest_Req(){LandId = this.LandId, DePesterId = this.DePesterId});

            if(resp.ErrorCode != 0)
            {
                Log.Error("除虫失败, 错误码: " + resp.ErrorCode);
                return;
            }
            
            //农场更新
            var farmlandModel = this.GetModel<FarmlandModel>();
            farmlandModel.UpdateLandData(resp.LandData);
            GameEvent.Send(EventID.FARMLAND_LAND_DEPEST_FINISH, LandId);
            
            //仓库道具更新
            var inventoryModel = this.GetModel<InventoryModel>();
            inventoryModel.UpdateItem(resp.CurItem);
        }
    }
}