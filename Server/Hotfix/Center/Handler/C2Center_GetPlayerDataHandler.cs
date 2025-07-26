using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;
using Hotfix.Common.Data;
using Hotfix.Common.Entity.Database;

namespace Hotfix.Center.Handler;

//客户端请求玩家数据
public class C2Center_GetPlayerDataHandler : RouteRPC<CenterUnit, C2Center_GetPlayerData_Req, Center2C_GetPlayerData_Resp>
{
    protected override async FTask Run(CenterUnit centerUnit, C2Center_GetPlayerData_Req request, Center2C_GetPlayerData_Resp response,
        Action reply)
    {
        var worldDataBase = centerUnit.Scene.World.DataBase;
        var playerData = (await worldDataBase.Query<PlayerBaseData>(d => d.PlayerId == centerUnit.PlayerId))[0];
        playerData.Deserialize(centerUnit.Scene);
        
        response.PlayerData = new Data_SyncPlayerData()
        {
            PlayerId = playerData.PlayerId,
            Name = playerData.NickName,
            Lv = playerData.Lv,
            Exp = playerData.Exp
        };
        Log.Debug(response.PlayerData.Name + " " + response.PlayerData.Lv + " " + response.PlayerData.Exp);
        playerData.Dispose();
    }
}