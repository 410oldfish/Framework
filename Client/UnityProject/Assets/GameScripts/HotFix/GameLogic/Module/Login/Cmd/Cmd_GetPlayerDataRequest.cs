using Fantasy;
using Fantasy.Helper;
using GameLogic.GameScripts.HotFix.GameLogic.Module.Player;
using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Login.Cmd
{
    public class Cmd_GetPlayerDataRequest : ICommand
    {
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async void Execute()
        {
            var resp = (Center2C_GetPlayerData_Resp) await GameModule.Network.CallRoute<C2Center_GetPlayerData_Req, Center2C_GetPlayerData_Resp>(
                new C2Center_GetPlayerData_Req());
            
            if (resp.ErrorCode != 0)
            {
                Log.Error($"获取玩家数据失败: {resp.ErrorCode}");
                return;
            }
            Log.Debug(resp.PlayerData.ToJson());
            //同步玩家数据到本地模型
            this.GetModel<PlayerModel>().SyncPlayerData(resp.PlayerData);
        }
    }
}