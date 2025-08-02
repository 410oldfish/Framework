using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;

namespace Hotfix;

//铲除作物
public class C2Center_Farmland_RemoveCrop_Handler : RouteRPC<CenterUnit, C2Center_Farmland_RemoveCrop_Req ,Center2C_Farmland_RemoveCrop_Resp>
{
    enum ErrorCode
    {
        Success = 0,
        RemoveFailed = 1, // 铲除失败
    }
    
    protected override async FTask Run(CenterUnit entity, C2Center_Farmland_RemoveCrop_Req request, Center2C_Farmland_RemoveCrop_Resp response,
        Action reply)
    {
        int landId = request.LandId;
        var farmlandComponent = entity.GetComponent<FarmlandDataEntity>();
        var landData = farmlandComponent.ClearSeed(landId);
        if (landData == null)
        {
            response.ErrorCode = (int)ErrorCode.RemoveFailed; // 种植失败
            return;
        }
        response.LandData = landData;
    }
}