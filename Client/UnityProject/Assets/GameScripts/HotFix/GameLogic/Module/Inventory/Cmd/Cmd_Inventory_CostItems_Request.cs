using System.Collections.Generic;
using Fantasy;
using QFramework;

namespace GameLogic
{
    public class Cmd_Inventory_CostItems_Request : ICommand
    {
        public int itemId;
        public int itemCount;
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async void Execute()
        {
            var res = await GameModule.Network.CallRoute<C2Center_Inventory_CostItems_Req, Center2C_Inventory_CostItems_Resp>(
                new C2Center_Inventory_CostItems_Req()
                {
                    Items = new List<ItemProto>(){new ItemProto()
                    {
                        Id = itemId,
                        Count = itemCount
                    }}
                });
            
            this.GetModel<InventoryModel>().UpdateItems(res.CurItems);
        }
    }
}