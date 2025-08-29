using Fantasy;
using QFramework;
using TEngine;

namespace GameLogic
{
    public class Cmd_Farmland_Fertilize_Req : ICommand
    {
        public int landId;
        public int fertilizerId;
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async void Execute()
        {
            var resp = await GameModule.Network.CallRoute<C2Center_Farmland_Fertilizer_Req, Center2C_Farmland_Fertilizer_Resp>(
                new C2Center_Farmland_Fertilizer_Req(){LandId = this.landId, FertilizerId = this.fertilizerId});

            if (resp.ErrorCode != 0)
            {
                Log.Error("施肥失败, 错误码: " + resp.ErrorCode);
                return;
            }
            
            var farmlandModel = this.GetModel<FarmlandModel>();
            farmlandModel.UpdateLandData(resp.LandData);
        }
    }
}