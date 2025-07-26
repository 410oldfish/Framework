
using Fantasy;
using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Player.Cmd
{
    public class Cmd_SyncPlayerData : ICommand
    {
        public Data_SyncPlayerData playerData;

        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public void Execute()
        {
            this.GetModel<PlayerModel>().SyncPlayerData(playerData);
        }
    }
}