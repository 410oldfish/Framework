using QFramework;

namespace GameLogic
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