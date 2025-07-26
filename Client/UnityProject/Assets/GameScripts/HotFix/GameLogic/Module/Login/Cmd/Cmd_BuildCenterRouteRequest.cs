
using GameLogic.GameScripts.HotFix.GameLogic.Common;
using QFramework;
using TEngine;
using Log = TEngine.Log;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Login.Cmd
{
    public class Cmd_BuildCenterRouteRequest : ICommand
    {
        public long playerId;
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {

        }

        public async void Execute()
        {
            var res = await GameModule.Network.BuildCenterRouteRequest(playerId);
            if (res.ErrorCode != 0)
            {
                Log.Error(ErrorCodeDesc.GetErrorDesc(res.GetType().Name, res.ErrorCode));
                return;
            }

            if (res.NewPlayer) //如果是新玩家，弹窗设定昵称
            {
                GameEvent.Send(EventID.LOGIN_SHOWSETNICKNAME);
            }
            else //如果是老玩家，请求完整玩家数据
            {
                this.SendCommand(new Cmd_GetPlayerDataRequest());
            }
        }
    }
}