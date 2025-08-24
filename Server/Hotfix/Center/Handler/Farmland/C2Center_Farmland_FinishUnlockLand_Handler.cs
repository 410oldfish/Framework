using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using GameConfig.farm;
using Hotfix.Center.Entity;

namespace Hotfix;

public class C2Center_Farmland_FinishUnlockLand_Handler : RouteRPC<CenterUnit, C2Center_Farmland_FinishUnlockLand_Req, Center2C_Farmland_FinishUnlockLand_Resp>
{
    protected override async FTask Run(CenterUnit centerUnit, C2Center_Farmland_FinishUnlockLand_Req request,
        Center2C_Farmland_FinishUnlockLand_Resp response, Action reply)
    {
        //检查模块是否解锁
        if (!centerUnit.HasComponent<FarmlandDataEntity>())
        {
            // 如果没有数据，则返回错误
            response.ErrorCode = 1; // 数据不存在
            return;
        }
        var farmlandData = centerUnit.GetComponent<FarmlandDataEntity>();

        var landId = request.LandId;
        bool ret = farmlandData.FinishUnlockLand(landId);
        if (!ret)
        {
            // 如果解锁失败，返回错误
            response.ErrorCode = 2; // 解锁失败
            return;
        }
        ELandType targetLandType = farmlandData.GetLandType(landId);
        response.LandType = (int)targetLandType;
    }
}