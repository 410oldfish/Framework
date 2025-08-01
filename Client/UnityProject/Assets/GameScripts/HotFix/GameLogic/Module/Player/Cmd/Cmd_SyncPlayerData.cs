using QFramework;

namespace GameLogic
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