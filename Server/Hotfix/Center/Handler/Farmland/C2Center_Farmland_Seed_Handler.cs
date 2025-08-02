using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;
using Hotfix.Common.Entity.Database;

namespace Hotfix;

//种植
public class C2Center_Farmland_Seed_Handler : RouteRPC<CenterUnit, C2Center_Farmland_Seed_Req, Center2C_Farmland_Seed_Resp>
{
    enum ErrorCode
    {
        None = 0,
        LandNotUnlocked = 1, //土地未解锁
        SeedNotEnough = 2, //种子不存在
        LandAlreadyHasCrop = 3, //土地上已有作物
        UnknownError = 4, //未知错误
    }
    
    protected override async FTask Run(CenterUnit entity, C2Center_Farmland_Seed_Req request, Center2C_Farmland_Seed_Resp response, Action reply)
    {
        int landId = request.LandId;
        int seedId = request.SeedId;
        
        var farmland = entity.GetComponent<FarmlandDataEntity>();
        if (!farmland.IsLandUnlocked(landId))
        {
            response.ErrorCode = (int)ErrorCode.LandNotUnlocked;
            return;
        }
        
        var inventory = entity.GetComponent<InventoryDataEntity>();
        if (!inventory.CheckItemCount(seedId, 1))
        {
            response.ErrorCode = (int)ErrorCode.SeedNotEnough;
            return;
        }
        
        if(!farmland.IsLandFree(landId))
        {
            response.ErrorCode = (int)ErrorCode.LandAlreadyHasCrop;
            return;
        }
        
        // 扣除种子
         var curItem = inventory.RemoveItem(seedId, 1);
        
         // 种植作物
         var landProto = farmland.Seed(landId, seedId);
         if (landProto == null)
         {
             response.ErrorCode = (int)ErrorCode.UnknownError;
             return;
         }

         response.LandData = landProto;
         response.CurItem = curItem;
    }
}