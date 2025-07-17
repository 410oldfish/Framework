using System.Collections.Generic;
using QFramework;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Ranchland
{
    public class RanchlandModel : AbstractModel
    {
        BindableProperty<List<LivestockData>> livestockDataList = new BindableProperty<List<LivestockData>>();
        protected override void OnInit()
        {
            
        }
    }
}