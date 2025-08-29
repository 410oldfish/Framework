using Fantasy;
using QFramework;
using TEngine;

namespace GameLogic
{
    public class Cmd_Farmland_RemoveCrop_Req : ICommand
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
            var resp = await GameModule.Network.CallRoute<C2Center_Farmland_RemoveCrop_Req, Center2C_Farmland_RemoveCrop_Resp>(
                new C2Center_Farmland_RemoveCrop_Req(){LandId = this.LandId});

            if (resp.ErrorCode != 0)
            { 
                Log.Error("移除作物失败, 错误码: " + resp.ErrorCode);
                return;
            }
            var farmlandModel = this.GetModel<FarmlandModel>();
            farmlandModel.UpdateLandData(resp.LandData);
        }
    }
}