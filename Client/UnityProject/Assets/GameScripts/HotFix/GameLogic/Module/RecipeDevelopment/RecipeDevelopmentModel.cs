using System.Collections.Generic;
using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.RecipeDevelopment
{
    public class RecipeDevelopmentModel : AbstractModel
    {
        //研发中的菜谱
        BindableProperty<List<ItemDataBase>> recipeDevelopmentDataList = new BindableProperty<List<ItemDataBase>>();
        protected override void OnInit()
        {
            
        }
    }
}