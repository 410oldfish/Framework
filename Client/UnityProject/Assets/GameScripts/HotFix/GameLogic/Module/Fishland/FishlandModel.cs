using System.Collections.Generic;
using QFramework;

namespace GameLogic
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