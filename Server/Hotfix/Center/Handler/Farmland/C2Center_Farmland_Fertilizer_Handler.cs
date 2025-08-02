using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;

namespace Hotfix;

//请求施肥
public class C2Center_Farmland_Fertilizer_Handler : RouteRPC<CenterUnit, C2Center_Farmland_Fertilizer_Req, Center2C_Farmland_Fertilizer_Resp>
{
    enum ErrorCode
    {
        Success = 0,
        NoEnoughFertilizer = 1, // 库存中没有足够的肥料
    }
    protected override async FTask Run(CenterUnit entity, C2Center_Farmland_Fertilizer_Req request, Center2C_Farmland_Fertilizer_Resp response,
        Action reply)
    {
        int landId = request.LandId;
        int fertilizerId = request.FertilizerId;
        
        var inventoryComponent = entity.GetComponent<InventoryDataEntity>();
        if (!inventoryComponent.CheckItemCount(fertilizerId, 1))
        {
            response.ErrorCode = (int)ErrorCode.NoEnoughFertilizer; // 库存中没有足够的肥料
            return;
        }

        var farmlandData = entity.GetComponent<FarmlandDataEntity>();
        var landData = farmlandData.Fertilizer(landId, fertilizerId);
    }
}