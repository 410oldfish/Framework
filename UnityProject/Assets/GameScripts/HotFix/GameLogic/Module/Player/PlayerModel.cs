using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Player
{
    public class PlayerModel : AbstractModel
    {
        BindableProperty<int> playerId = new BindableProperty<int>();
        BindableProperty<string> playerName = new BindableProperty<string>();
        BindableProperty<int> playerLevel = new BindableProperty<int>();
        BindableProperty<int> playerExp = new BindableProperty<int>();
        protected override void OnInit()
        {
            
        }
    }
}