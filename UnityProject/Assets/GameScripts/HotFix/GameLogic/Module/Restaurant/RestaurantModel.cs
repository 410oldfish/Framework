using System.Collections.Generic;
using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Restaurant
{
    public class RestaurantModel : AbstractModel
    {
        //家具摆放位置
        BindableProperty<Dictionary<GridXY, FornitureData>> restaurantDataDic = new BindableProperty<Dictionary<GridXY, FornitureData>>();
        protected override void OnInit()
        {
            
        }
    }
}