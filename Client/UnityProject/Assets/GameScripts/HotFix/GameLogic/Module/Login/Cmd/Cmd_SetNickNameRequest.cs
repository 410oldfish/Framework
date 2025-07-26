using Fantasy;
using QFramework;
using Log = TEngine.Log;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Login.Cmd
{
    public class Cmd_SetNickNameRequest : ICommand
    {
        public string nickName;
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async void Execute()
        {
            var resp = await GameModule.Network.CallRoute<C2Center_SetNickName_Req, Center2C_SetNickName_Resp>(
                new C2Center_SetNickName_Req()
                {
                    NickName = nickName
                });

            if (resp.ErrorCode != 0)
            {
                Log.Error("设置昵称失败: " + resp.ErrorCode);
                return;
            }
            
            //设置昵称成功，请求玩家基础数据
            this.SendCommand(new Cmd_GetPlayerDataRequest());
        }
    }
}