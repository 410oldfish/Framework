using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using GameConfig.farm;
using Hotfix.Center.Entity;
using Hotfix.Common.Entity.Database;
using Hotfix.Config;

namespace Hotfix;

public class C2Center_Farmland_ChangeLandTypeHandler : RouteRPC<CenterUnit, C2Center_Farmland_ChangeLandType_Req, Center2C_Farmland_ChangeLandType_Resp>
{
    enum ErrorCode
    {
        InvalidConfig = 1001, // 无效的配置
        InsufficientResources = 1002, // 资源不足
        InvalidLandId = 1003, // 无效的土地ID
        UnknownError = 9999 // 未知错误
    }
    
    protected override async FTask Run(CenterUnit entity, C2Center_Farmland_ChangeLandType_Req request,
        Center2C_Farmland_ChangeLandType_Resp response, Action reply)
    {
        var landId = request.LandId;
        var changeLandType = request.LandType;
        ConfigHelper configHelper = entity.Scene.GetComponent<ConfigHelper>();
        var costConfig = configHelper.FarmlandTypeConfig.Get((ELandType)changeLandType);
        if(costConfig == null)
        {
            response.ErrorCode = (int)ErrorCode.InvalidConfig;
            return;
        }
        
        var farmlandComponent = entity.GetComponent<FarmlandDataEntity>();
        if (!farmlandComponent.IsLandExist(landId))
        {
            response.ErrorCode = (int)ErrorCode.InvalidLandId;
            return;
        }
        
        // 检查资源是否足够
        var inventoryComponent = entity.GetComponent<InventoryDataEntity>();
        var costItems = costConfig.Cost;
        if (!inventoryComponent.CheckItemCount(costItems))
        {
            response.ErrorCode = (int)ErrorCode.InsufficientResources;
            return;
        }
        
        // 扣除资源
        var curItems = inventoryComponent.RemoveItems(costItems);
        
        // 更新土地类型
        farmlandComponent.ChangeLandType(landId, (ELandType)changeLandType);
        
        // 返回结果
        response.CurItems = curItems;
    }
}