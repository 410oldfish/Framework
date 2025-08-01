using Cysharp.Threading.Tasks;
using Fantasy;
using QFramework;

namespace GameLogic
{
    public class Cmd_Player_GetPlayerDataRequest : ICommand<UniTask<bool>>
    {
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async UniTask<bool> Execute()
        {
            var resp = (Center2C_GetPlayerData_Resp) await GameModule.Network.CallRoute<C2Center_GetPlayerData_Req, Center2C_GetPlayerData_Resp>(
                new C2Center_GetPlayerData_Req());
            
            if (resp.ErrorCode != 0)
            {
                FLog.Error($"获取玩家数据失败: {resp.ErrorCode}");
                return false;
            }

            //同步玩家数据到本地模型
            this.GetModel<PlayerModel>().SyncPlayerData(new Data_SyncPlayerData()
            {
                PlayerId = resp.PlayerId,
                Name = resp.Name,
                Lv = resp.Lv,
                Exp = resp.Exp
            });
            return true;
        }
    }
}