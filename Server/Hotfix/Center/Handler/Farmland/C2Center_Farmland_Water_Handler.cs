using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;

namespace Hotfix;

//浇水请求
public class C2Center_Farmland_Water_Handler : RouteRPC<CenterUnit, C2Center_Farmland_Water_Req, Center2C_Farmland_Water_Resp>
{
    enum ErrorCode
    {
        Success = 0,
        WaterFailed = 1, // 浇水失败
    }
    protected override async FTask Run(CenterUnit entity, C2Center_Farmland_Water_Req request, Center2C_Farmland_Water_Resp response, Action reply)
    {
        int landId = request.LandId;
        var farmlandData = entity.GetComponent<FarmlandDataEntity>();
        var landData = farmlandData.Water(landId);
        if (landData == null)
        {
            response.ErrorCode = (int)ErrorCode.WaterFailed; // 浇水失败
            return;
        }
        response.LandData = landData;
    }
}