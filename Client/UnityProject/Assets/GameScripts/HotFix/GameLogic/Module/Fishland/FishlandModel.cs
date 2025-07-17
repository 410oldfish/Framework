using System.Collections.Generic;
using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Fishland
{
    public class FishlandModel : AbstractModel
    {
        //收获的鱼
        BindableProperty<List<FishData>> fishDataList = new BindableProperty<List<FishData>>();
        protected override void OnInit()
        {
            
        }
    }
}