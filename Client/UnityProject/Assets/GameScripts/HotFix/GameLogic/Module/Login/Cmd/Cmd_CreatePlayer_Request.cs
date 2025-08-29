using Fantasy;
using QFramework;
using TEngine;

namespace GameLogic
{
    public class Cmd_CreatePlayer_Request : ICommand
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
            var resp = await GameModule.Network.CallRoute<C2Center_CreatePlayer_Req, Center2C_CreatePlayer_Resp>(
                new C2Center_CreatePlayer_Req()
                {
                    NickName = nickName
                });

            if (resp.ErrorCode != 0)
            {
                Log.Error("创建玩家失败: " + resp.ErrorCode);
                return;
            }
            Log.Debug("创建玩家成功 : " + nickName);
            //设置昵称成功，请求玩家数据
            
            this.GetSystem<PlayerSystem>().RequestAllModuleData();
        }
    }
}