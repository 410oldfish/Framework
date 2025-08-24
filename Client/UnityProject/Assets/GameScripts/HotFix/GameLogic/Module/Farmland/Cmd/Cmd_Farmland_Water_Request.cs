using Fantasy;
using QFramework;
using TEngine;

namespace GameLogic
{
    public class Cmd_Farmland_Water_Request : ICommand
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
            var resp = await GameModule.Network.CallRoute<C2Center_Farmland_Water_Req, Center2C_Farmland_Water_Resp>(
                new C2Center_Farmland_Water_Req(){LandId = this.LandId});

            if (resp.ErrorCode != 0)
            {
                Log.Error("浇水失败, 错误码: " + resp.ErrorCode);
                return;
            }

            //农场更新
            var farmlandModel = this.GetModel<FarmlandModel>();
            farmlandModel.UpdateLandData(resp.LandData);
            GameEvent.Send<int>(EventID.FARMLAND_LAND_WATER_FINISH, LandId);
        }
    }
}