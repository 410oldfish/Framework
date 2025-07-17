using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Player
{
    public class PlayerModel : AbstractModel
    {
        BindableProperty<int> playerId = new BindableProperty<int>();
        public int PlayerId => playerId.Value;
        public void SetPlayerId(int id)
        {
            playerId.Value = id;
        }
        
        BindableProperty<string> playerName = new BindableProperty<string>();
        public string PlayerName => playerName.Value;
        public void SetPlayerName(string name)
        {
            playerName.Value = name;
        }
        
        BindableProperty<int> playerLevel = new BindableProperty<int>();
        public int PlayerLevel => playerLevel.Value;
        public void SetPlayerLevel(int level)
        {
            playerLevel.Value = level;
        }
        
        BindableProperty<int> playerExp = new BindableProperty<int>();
        public int PlayerExp => playerExp.Value;
        public void SetPlayerExp(int exp)
        {
            playerExp.Value = exp;
        }
        
        protected override void OnInit()
        {
            
        }
    }
}