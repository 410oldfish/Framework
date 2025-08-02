using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using GameConfig.farm;
using Hotfix.Center.Entity;
using Hotfix.Common.Entity.Database;

namespace Hotfix.Center.Handler.Farmland;

public class C2Center_Farmland_GetFarmlandDataHandler : RouteRPC<CenterUnit, C2Center_Farmland_GetFarmlandData_Req, Center2C_Farmland_GetFarmlandData_Resp>
{
    enum ErrorCode
    {
        Success = 0,
        NoFoundData = 1,
    }
    protected override async FTask Run(CenterUnit centerUnit, C2Center_Farmland_GetFarmlandData_Req request,
        Center2C_Farmland_GetFarmlandData_Resp response, Action reply)
    {
        if (!centerUnit.HasComponent<FarmlandDataEntity>())
        {
            var worldDataBase = centerUnit.Scene.World.DataBase;
            var farmlandDataList = await worldDataBase.Query<FarmlandDataEntity>(d => d.PlayerId == centerUnit.PlayerId);
            if (farmlandDataList.Count == 0)
            {
                response.ErrorCode = (int)ErrorCode.NoFoundData; // 数据不存在
                return;
            }
            var farmlandData = farmlandDataList[0];
            farmlandData.Deserialize(centerUnit.Scene);
            centerUnit.AddComponent(farmlandData);
        }

        var farmlandDataComponent = centerUnit.GetComponent<FarmlandDataEntity>();

        var landProtoData = farmlandDataComponent.GetLandDataList();

        response.FarmlandData = landProtoData;
    }
}