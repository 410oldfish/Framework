using Cysharp.Threading.Tasks;
using Fantasy;
using QFramework;

namespace GameLogic
{
    public class Cmd_Inventory_GetInventoryData_Request : ICommand<UniTask<bool>>
    {
        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
        }

        public async UniTask<bool> Execute()
        {
            var res = await GameModule.Network.CallRoute<C2Center_Inventory_GetInventoryData_Req, Center2C_Inventory_GetInventoryData_Resp>(
                new C2Center_Inventory_GetInventoryData_Req());
            
            if (res.ErrorCode != 0)
            {
                FLog.Error("获取库存数据失败，错误码：" + res.ErrorCode);
                return false;
            }
            
            this.GetModel<InventoryModel>().UpdateItems(res.Items);
            return true;
        }
    }
}