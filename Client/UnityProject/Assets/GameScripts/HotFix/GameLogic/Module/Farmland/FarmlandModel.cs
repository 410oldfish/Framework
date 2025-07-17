using System.Collections.Generic;
using QFramework;
using WeChatWASM;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Farmland
{
    public class FarmlandModel : AbstractModel
    {
        BindableProperty<Dictionary<GridXY, CropsData>> farmlandDataDic = new BindableProperty<Dictionary<GridXY, CropsData>>();
        
        
        protected override void OnInit()
        {
            
        }
    }
}