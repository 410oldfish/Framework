using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Player
{
    public class InventoryCtrl : AbstractSystem
    {
        protected override void OnInit()
        {
            
        }
        
        // Check
        
        /// <summary>
        /// 获取道具数量
        /// <summary>
        public int GetItemCount(int itemId)
        {
            InventoryModel inventoryModel = this.GetModel<InventoryModel>();
            return inventoryModel.GetItemCount(itemId);
        }
    }
}