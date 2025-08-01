using TEngine;
using QFramework;

namespace GameLogic
{
    public class PlayerModel : AbstractModel
    {
        long playerId;
        public long PlayerId => playerId;
        public void SetPlayerId(long id)
        {
            playerId = id;
            Log.Debug("客户端数据更改 ： PlayerId = " + id);
        }
        
        string playerName;
        public string PlayerName => playerName;
        public void SetPlayerName(string name)
        {
            playerName = name;
            Log.Debug("客户端数据更改 ： PlayerName = " + name);
        }
        
        int playerLevel;
        public int PlayerLevel => playerLevel;
        public void SetPlayerLevel(int level)
        {
            playerLevel = level;
            Log.Debug("客户端数据更改 ： PlayerLevel = " + level);
        }
        
        int playerExp;
        public int PlayerExp => playerExp;
        public void SetPlayerExp(int exp)
        {
            playerExp = exp;
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