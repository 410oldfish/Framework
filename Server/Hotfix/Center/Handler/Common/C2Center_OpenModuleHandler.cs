using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using GameConfig.common;
using Hotfix.Center.Entity;
using Hotfix.Common.Entity.Database;

namespace Hotfix.Center.Handler;

public class C2Center_OpenModuleHandler : RouteRPC< CenterUnit,C2Center_OpenModule_Req, Center2C_OpenModule_Resp>
{
    enum ErrorCode  
    {
        ExistData = 1, //数据已存在
    }
    protected override async FTask Run(CenterUnit centerUnit, C2Center_OpenModule_Req request, Center2C_OpenModule_Resp response, Action reply)
    {
        int moduleId = request.ModuleId;
        if (moduleId == (int)EModule.Farmland)
        {
            if (centerUnit.HasComponent<FarmlandDataEntity>())
            {
                response.ErrorCode = (int)ErrorCode.ExistData;
                return;
            }

            var farmlandData = centerUnit.AddComponent<FarmlandDataEntity>();
            farmlandData.SetPlayerId(centerUnit.PlayerId);
        }
    }
}