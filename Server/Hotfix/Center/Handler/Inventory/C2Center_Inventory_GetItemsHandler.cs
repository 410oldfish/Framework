using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;

namespace Hotfix;

public class C2Center_Inventory_GetItemsHandler : RouteRPC<CenterUnit, C2Center_Inventory_GetItems_Req, Center2C_Inventory_GetItems_Resp>
{
    protected override async FTask Run(CenterUnit centerUnit, C2Center_Inventory_GetItems_Req request, Center2C_Inventory_GetItems_Resp response,
        Action reply)
    {
        if (!centerUnit.HasComponent<InventoryDataEntity>())
        {
            response.ErrorCode = 1; // 库存数据不存在
            return;
        }

        var inventoryComponent = centerUnit.GetComponent<InventoryDataEntity>();
        var curItemCounts = inventoryComponent.AddItems(request.Items);

        response.CurItems = curItemCounts;
        await FTask.CompletedTask;
    }
}