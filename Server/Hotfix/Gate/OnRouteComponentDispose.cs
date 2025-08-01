using Fantasy;
using Fantasy.Entitas.Interface;
using Fantasy.Event;
using Fantasy.Network;
using Fantasy.Platform.Net;

namespace Hotfix;

public sealed class OnRouteComponentDispose : DestroySystem<RouteLifeComponent>
{
    protected override async void Destroy(RouteLifeComponent self)
    {
        var centerSceneCfg = SceneConfigData.Instance.GetSceneBySceneType(SceneType.Map)[0];
        var centerRouteId = centerSceneCfg.RouteId;
        var networkMessagingComponent = self.Scene.NetworkMessagingComponent;
        networkMessagingComponent.SendInnerRoute(centerRouteId, new G2Center_DisposeCenterUnit_Msg()
        {
            CenterRouteId = self.centerRouteId
        });
    }
}