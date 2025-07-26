
using Fantasy;
using QFramework;
using Log = TEngine.Log;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Player
{
    public class PlayerModel : AbstractModel
    {
        BindableProperty<long> playerId = new BindableProperty<long>();
        public long PlayerId => playerId.Value;
        public void SetPlayerId(long id)
        {
            playerId.Value = id;
            Log.Debug("客户端数据更改 ： PlayerId = " + id);
        }
        
        BindableProperty<string> playerName = new BindableProperty<string>();
        public string PlayerName => playerName.Value;
        public void SetPlayerName(string name)
        {
            playerName.Value = name;
            Log.Debug("客户端数据更改 ： PlayerName = " + name);
        }
        
        BindableProperty<int> playerLevel = new BindableProperty<int>();
        public int PlayerLevel => playerLevel.Value;
        public void SetPlayerLevel(int level)
        {
            playerLevel.Value = level;
            Log.Debug("客户端数据更改 ： PlayerLevel = " + level);
        }
        
        BindableProperty<int> playerExp = new BindableProperty<int>();
        public int PlayerExp => playerExp.Value;
        public void SetPlayerExp(int exp)
        {
            playerExp.Value = exp;
            Log.Debug("客户端数据更改 ： PlayerExp = " + exp);
        }
        
        protected override void OnInit()
        {
            
        }

        public void SyncPlayerData(Data_SyncPlayerData playerData)
        {
            this.SetPlayerId(playerData.PlayerId);
            this.SetPlayerName(playerData.Name);
            this.SetPlayerLevel(playerData.Lv);
            this.SetPlayerExp(playerData.Exp);
        }
    }
}