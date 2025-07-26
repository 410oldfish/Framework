using GameLogic.GameScripts.HotFix.GameLogic.Module.Player;

namespace QFramework
{
    public class GameMgr : Architecture<GameMgr>
    {
        protected override void Init()
        {
            //Model
            this.RegisterModel(new PlayerModel());
            
            //System
        }
    }
}