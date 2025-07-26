using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;
using Hotfix.Common.Entity.Database;

namespace Hotfix.Center.Handler;

public class G2Center_BuildRouteHandler : RouteRPC<Scene, G2Center_BuildRoute_Req, Center2G_BuildRoute_Resp>
{
    protected override async FTask Run(Scene entity, G2Center_BuildRoute_Req request, Center2G_BuildRoute_Resp response, Action reply)
    {
        var centerUnit = Fantasy.Entitas.Entity.Create<CenterUnit>(entity, true, true);
        centerUnit.SetGateRouteId(request.GateRouteId);
        centerUnit.SetPlayerId(request.PlayerId);
        
        response.CenterRouteId = centerUnit.RuntimeId;
        // 获得操作DB的接口
        var worldDataBase = entity.World.DataBase;
        bool existPlayer = await worldDataBase.Exist<PlayerBaseData>(d => d.PlayerId == request.PlayerId);
        response.IsNewPlayer = !existPlayer;
        await FTask.CompletedTask;
    }
}