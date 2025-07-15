using System.Collections.Generic;
using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.KitchenRoom
{
    public class KitchenRoomModel : AbstractModel
    {
        //做菜人员
        BindableProperty<List<int>> cookingStaffList = new BindableProperty<List<int>>();
        //菜谱数据
        BindableProperty<List<int>> recipeDataList = new BindableProperty<List<int>>();
        protected override void OnInit()
        {
            
        }
    }
}