using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;
using Hotfix.Common.Entity.Database.Common;

namespace Hotfix.Center.Handler;

//客户端请求玩家数据
public class C2Center_GetPlayerDataHandler : RouteRPC<CenterUnit, C2Center_GetPlayerData_Req, Center2C_GetPlayerData_Resp>
{
    enum ErrorCode
    {
        Success = 0,
        NoFoundData = 1,
    }
    protected override async FTask Run(CenterUnit centerUnit, C2Center_GetPlayerData_Req request, Center2C_GetPlayerData_Resp response,
        Action reply)
    {
        if (!centerUnit.HasComponent<GameCoreDataEntity>())
        {
            //尝试从数据库中查询玩家数据
            var worldDataBase = centerUnit.Scene.World.DataBase;
            var playerDataList = await worldDataBase.Query<GameCoreDataEntity>(d => d.PlayerId == centerUnit.PlayerId);
            if (playerDataList.Count == 0) //玩家数据不存在
            {
                response.ErrorCode = (int)ErrorCode.NoFoundData;
                return;
            }
            var playerData = playerDataList[0];
            playerData.Deserialize(centerUnit.Scene);
            centerUnit.AddComponent(playerData);
        }
        
        var playerCoreData = centerUnit.GetComponent<GameCoreDataEntity>();
        response.PlayerId = playerCoreData.PlayerId;
        response.Name = playerCoreData.NickName;
        response.Lv = playerCoreData.Lv;
        response.Exp = playerCoreData.Exp;
    }
}