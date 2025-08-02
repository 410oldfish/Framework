using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;

namespace Hotfix;

public class
    C2Center_Farmland_DePest_Handler : RouteRPC<CenterUnit, C2Center_Farmland_DePest_Req, Center2C_Farmland_DePest_Resp>
{
    enum ErrorCode
    {
        Success = 0,
        NoEnoughDePester = 1, // 库存中没有足够的除虫剂
    }
    
    protected override async FTask Run(CenterUnit entity, C2Center_Farmland_DePest_Req request,
        Center2C_Farmland_DePest_Resp response,
        Action reply)
    {
        int landId = request.LandId;
        int depesterId = request.DePesterId;
        
        var inventoryComponent = entity.GetComponent<InventoryDataEntity>();
        if (!inventoryComponent.CheckItemCount(depesterId, 1))
        {
            response.ErrorCode = (int)ErrorCode.NoEnoughDePester; // 库存中没有足够的除虫剂
            return;
        }
        
        
        
    }
}