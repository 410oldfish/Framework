using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;

namespace Hotfix.Center.Handler.Inventory;

public class C2Center_Inventory_GetInventoryDataHandler : RouteRPC<CenterUnit, C2Center_Inventory_GetInventoryData_Req, Center2C_Inventory_GetInventoryData_Resp>
{
    enum ErrorCode
    {
        Success = 0,
        NoFoundData = 1,
    }
    protected override async FTask Run(CenterUnit centerUnit, C2Center_Inventory_GetInventoryData_Req request,
        Center2C_Inventory_GetInventoryData_Resp response, Action reply)
    {
        if (!centerUnit.HasComponent<InventoryDataEntity>())
        {
            //尝试从数据库中查询玩家数据
            var worldDataBase = centerUnit.Scene.World.DataBase;
            var inventoryDataList = await worldDataBase.Query<InventoryDataEntity>(d => d.PlayerId == centerUnit.PlayerId);
            if (inventoryDataList.Count == 0) //玩家数据不存在
            {
                response.ErrorCode = (int)ErrorCode.NoFoundData;
                return;
            }
            var inventoryData = inventoryDataList[0];
            inventoryData.Deserialize(centerUnit.Scene);
            centerUnit.AddComponent(inventoryData);
        }
        
        var inventoryDataComponent = centerUnit.GetComponent<InventoryDataEntity>();
        var itemData = inventoryDataComponent.GetInventoryItems();
        response.Items = itemData;
        
        await FTask.CompletedTask;
    }
}