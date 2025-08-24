using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;

namespace Hotfix;

//请求收获
public class C2Center_Farmland_GainYieldHandler : RouteRPC<CenterUnit, C2Center_Farmland_GainYield_Req, Center2C_Farmland_GainYield_Resp>
{
    enum ErrorCode
    {
        None = 0,
        LandNotUnlocked = 1, //土地未解锁
        LandNotReady = 2, //土地未成熟
        UnknownError = 3, //未知错误
    }
    protected override async FTask Run(CenterUnit entity, C2Center_Farmland_GainYield_Req request, Center2C_Farmland_GainYield_Resp response,
        Action reply)
    {
        var landIds = request.LandId;
        var farmlandComponent = entity.GetComponent<FarmlandDataEntity>();
        
        //土地检查
        foreach (var landId in landIds)
        {
            if (!farmlandComponent.IsLandExist(landId))
            {
                response.ErrorCode = (int)ErrorCode.LandNotUnlocked;
                return;
            }
        }

        var gainRet = farmlandComponent.Gain(landIds);
        var gainItems = gainRet.Item1;
        var landProtos = gainRet.Item2;
        if (gainItems.Count == 0 || landProtos.Count == 0)
        {
            response.ErrorCode = (int)ErrorCode.LandNotReady; // 所有土地均不可收获
            return;
        }
        
        //添加到库存
        var inventoryComponent = entity.GetComponent<InventoryDataEntity>();
        var curItems = inventoryComponent.AddItems(gainItems);
        
        response.LandDatas = landProtos;
        response.CurItems = curItems;
        response.GainItems = gainItems;
    }
}