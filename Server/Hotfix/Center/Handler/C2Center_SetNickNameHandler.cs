using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;
using Hotfix.Common.Entity.Database;
using MongoDB.Driver;
namespace Hotfix.Center.Handler;
//设置玩家昵称，同时创建玩家基础数据
public class C2Center_SetNickNameHandler : RouteRPC<CenterUnit, C2Center_SetNickName_Req, Center2C_SetNickName_Resp>
{
    enum C2Center_SetNickName_ErrorCode
    {
        Success = 0, //成功
        PlayerDataExists = 1, //玩家数据已存在
    }
    protected override async FTask Run(CenterUnit entity, C2Center_SetNickName_Req request, Center2C_SetNickName_Resp response, Action reply)
    {
        long playerId = entity.PlayerId;
        string nickName = request.NickName;
        int lv = 1;
        int exp = 1;
        
        var dataBase = entity.Scene.World.DataBase;
        //创建唯一索引
        var exist = await dataBase.Exist<PlayerBaseData>(d => d.PlayerId == playerId);
        if (!exist) //不存在，创建新玩家数据
        {
            var playerEntity = Fantasy.Entitas.Entity.Create<PlayerBaseData>(entity.Scene, true, true);
            playerEntity.PlayerId = entity.PlayerId;
            playerEntity.NickName = nickName;
            playerEntity.Lv = lv;
            playerEntity.Exp = exp;
            
            await dataBase.Insert(playerEntity);
            playerEntity.Dispose();
        }
        else
        {
            response.ErrorCode = (int)C2Center_SetNickName_ErrorCode.PlayerDataExists; //玩家数据已存在
        }
    }
}