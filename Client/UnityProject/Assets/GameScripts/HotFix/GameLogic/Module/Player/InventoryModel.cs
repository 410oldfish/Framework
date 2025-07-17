using System.Collections.Generic;
using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Player
{
    //仓库数据
    public class InventoryModel : AbstractModel
    {
        BindableProperty<Dictionary<int, int>> inventoryData = new BindableProperty<Dictionary<int, int>>();
        
        public int GetItemCount(int itemId)
        {
            if (inventoryData.Value == null || !inventoryData.Value.ContainsKey(itemId))
            {
                return 0;
            }
            return inventoryData.Value[itemId];
        }
        
        
        protected override void OnInit()
        {
            
        }
    }
}