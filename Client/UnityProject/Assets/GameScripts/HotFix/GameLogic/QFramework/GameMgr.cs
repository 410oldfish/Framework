
using GameLogic;
using GameLogic.GameScripts.HotFix.GameLogic.Module.Ranchland;

namespace QFramework
{
    public class GameMgr : Architecture<GameMgr>
    {
        protected override void Init()
        {
            //Model
            this.RegisterModel(new PlayerModel());
            this.RegisterModel(new InventoryModel());
            this.RegisterModel(new FarmlandModel());
            this.RegisterModel(new RanchlandModel());
            this.RegisterModel(new FishlandModel());
            
            //System
            this.RegisterSystem(new PlayerCtrl());
        }
    }
}