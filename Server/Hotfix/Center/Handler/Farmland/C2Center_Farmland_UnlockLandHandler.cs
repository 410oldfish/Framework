using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;
using Hotfix.Common.Entity.Database;
using Hotfix.Common.Entity.Database.Common;
using Hotfix.Config;

namespace Hotfix.Center.Handler.Farmland;

public class C2Center_Farmland_UnlockLandHandler : RouteRPC<CenterUnit, C2Center_Farmland_UnlockLand_Req, Center2C_Farmland_UnlockLand_Resp>
{
    protected override async FTask Run(CenterUnit centerUnit, C2Center_Farmland_UnlockLand_Req request, Center2C_Farmland_UnlockLand_Resp response,
        Action reply)
    {
        //检查模块是否解锁
        if (!centerUnit.HasComponent<FarmlandDataEntity>())
        {
            // 如果没有数据，则返回错误
            response.ErrorCode = 1; // 数据不存在
            return;
        }
        var farmlandData = centerUnit.GetComponent<FarmlandDataEntity>();

        // 检查地块是否已解锁
        if (farmlandData.IsLandUnlocked(request.LandId))
        {
            response.ErrorCode = 2; // 地块已解锁
            return;
        }
        
        // 检查道具
        var configHelper = centerUnit.Scene.GetComponent<ConfigHelper>();
        var costItems = configHelper.FarmlandUnlockConfig.Get(request.LandId).Cost;
        
        bool hasEnoughItems = true;
        var inventory = centerUnit.GetComponent<InventoryDataEntity>();
        if(costItems != null)
        {
            if (inventory == null)
            {
                response.ErrorCode = 3; // 库存数据不存在
                return;
            }
            foreach (var item in costItems)
            {
                if (!inventory.CheckItemCount(item.Id, item.Count))
                {
                    hasEnoughItems = false;
                    break;
                }
            }
        }
        if (!hasEnoughItems)
        {
            response.ErrorCode = 4; // 库存物品不足
            return;
        }
        
        //检查等级
        int unlockLevel = configHelper.FarmlandUnlockConfig.Get(request.LandId).NeedLv;
        var playerCoreData = centerUnit.GetComponent<PlayerCoreDataEntity>();
        if(playerCoreData.Lv < unlockLevel)
        {
            response.ErrorCode = 5; // 等级不足
            return;
        }
        
        // 扣除道具
        var curItems = inventory.RemoveItems(costItems);
        
        // 解锁地块
        long unlockStartTime = farmlandData.UnlockLand(request.LandId);

        //--------------同步客户端----------------------------
        response.UnlockStartTime = unlockStartTime;
        response.CurItems = curItems;
    }
}