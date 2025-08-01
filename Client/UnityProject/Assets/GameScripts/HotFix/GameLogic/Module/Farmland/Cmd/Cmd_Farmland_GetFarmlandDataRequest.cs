using Fantasy;
using QFramework;

namespace GameLogic
{
    public class Cmd_Farmland_GetFarmlandDataRequest : ICommand
    {
        enum ErrorCode
        {
            Success = 0,
            NoFoundData = 1,
        }
        
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async void Execute()
        {
            var res = await GameModule.Network.CallRoute<C2Center_Farmland_GetFarmlandData_Req, Center2C_Farmland_GetFarmlandData_Resp>(
                new C2Center_Farmland_GetFarmlandData_Req());

            if (res.ErrorCode == (int)ErrorCode.NoFoundData)
            {
                
            }
        }
    }
}