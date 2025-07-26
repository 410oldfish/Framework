using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Fantasy.Platform.Net;

namespace Hotfix.Gate.Handler;

public class C2G_BuildCenterRouteHandler : MessageRPC<C2G_BuildCenterRoute_Req, G2C_BuildCenterRoute_Resp>
{
    enum C2G_BuildCenterRoute_ErrorCode
    {
        Success = 0,
        FailToBuildRouteInCenter = 1,
    }
    protected override async FTask Run(Session session, C2G_BuildCenterRoute_Req request, G2C_BuildCenterRoute_Resp response, Action reply)
    {
        var centerSceneCfg = SceneConfigData.Instance.GetSceneBySceneType(SceneType.Map)[0];
        var centerRouteId = centerSceneCfg.RouteId;
        var networkMessagingComponent = session.Scene.NetworkMessagingComponent;
        var routeResponse = (Center2G_BuildRoute_Resp) await networkMessagingComponent.CallInnerRoute(centerRouteId, new G2Center_BuildRoute_Req()
        {
            GateRouteId = session.RuntimeId,
            PlayerId = request.PlayerId
        });

        if (routeResponse.ErrorCode != 0)
        {
            Log.Error($"建立到中心服的路由失败: {session.RuntimeId} ErrorCode : {routeResponse.ErrorCode}");
            response.ErrorCode = (int)C2G_BuildCenterRoute_ErrorCode.FailToBuildRouteInCenter;
            reply?.Invoke();
            return;
        }

        var routeComponent = session.GetOrAddComponent<RouteComponent>();
        routeComponent.AddAddress((int)RouteType.CenterRoute, routeResponse.CenterRouteId);
        
        Log.Debug($"建立到中心服的路由成功: {session.RuntimeId} CenterRouteId : {routeResponse.CenterRouteId}");

        response.PlayerId = request.PlayerId;
        response.NewPlayer = routeResponse.IsNewPlayer;
    }
}