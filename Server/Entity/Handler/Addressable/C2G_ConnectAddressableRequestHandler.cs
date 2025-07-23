using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Fantasy.Network.Route;
using Fantasy.Platform.Net;

namespace Entity.Handler.Addressable;

public class C2G_ConnectAddressableRequestHandler : MessageRPC<C2G_ConnectAddressableRequest, G2C_ConnectAddressableResponse>
{
    protected override async FTask Run(Session session, C2G_ConnectAddressableRequest request, G2C_ConnectAddressableResponse response,
        Action reply)
    {
        var mapSceneList = SceneConfigData.Instance.GetSceneBySceneType(SceneType.Map);
        var mapSceneConfig = mapSceneList[0];

        var ret = (M2G_ConnectResponse) await session.Scene.NetworkMessagingComponent.CallInnerRoute(mapSceneConfig.RouteId, new G2M_ConnectRequest()
        {
            GateRouteId = session.RouteId,
        });
        var addreddableComponent = session.AddComponent<AddressableRouteComponent>();
        addreddableComponent.AddressableId = ret.AddressableId;
        Log.Debug("register addressable id: " + addreddableComponent.AddressableId);
    }
}