using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;
using Hotfix.Common.Entity.Database.Common;
using Hotfix.Config;
using MongoDB.Driver;
namespace Hotfix.Center.Handler;
//设置玩家昵称，同时创建玩家基础数据
public class C2Center_CreatePlayerHandler : RouteRPC<CenterUnit, C2Center_CreatePlayer_Req, Center2C_CreatePlayer_Resp>
{
    enum ErrorCode
    {
        Success = 0, //成功
        PlayerDataExists = 1, //玩家数据已存在
    }
    protected override async FTask Run(CenterUnit centerUnit, C2Center_CreatePlayer_Req request, Center2C_CreatePlayer_Resp response, Action reply)
    {
        if (centerUnit.HasComponent<GameCoreDataEntity>())
        {
            response.ErrorCode = (int)ErrorCode.PlayerDataExists; //玩家数据已存在
            return;
        }
        
        //Player Core Data
        var configHelper = centerUnit.Scene.GetComponent<ConfigHelper>();
        long playerId = centerUnit.PlayerId;
        string nickName = request.NickName;
        int lv = configHelper.GlobalConfig.PlayerInitLv;
        int exp = configHelper.GlobalConfig.PlayerInitExp;
        
        var playerCoreData = centerUnit.AddComponent<GameCoreDataEntity>();
        playerCoreData.SetPlayerData(playerId, nickName, lv, exp);
        
        //Inventory
        var inventoryData = centerUnit.AddComponent<InventoryDataEntity>();
        inventoryData.SetPlayerId(playerId);
        
        //Farmland
        var farmlandData = centerUnit.AddComponent<FarmlandDataEntity>();
        farmlandData.SetPlayerId(playerId);
    }
}