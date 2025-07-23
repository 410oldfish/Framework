using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Fantasy.Platform.Net;

namespace Entity.Handler;

public class C2G_ConnectChatRequestHandler : MessageRPC<C2G_ConnectChatRequest, G2C_ConnectChatResponse>
{
    protected override async FTask Run(Session session, C2G_ConnectChatRequest request, G2C_ConnectChatResponse response, Action reply)
    {
        var chatScene = SceneConfigData.Instance.GetSceneBySceneType(SceneType.Chat);
        var chatSceneConfig = chatScene[0];
        var sceneNetworkMessagingComponent = session.Scene.NetworkMessagingComponent;
        var ret = (Chat2G_ConnectResponse) await sceneNetworkMessagingComponent.CallInnerRoute(chatSceneConfig.RouteId, new G2Chat_ConnectRequest()
        {
            GateRouteId = session.RuntimeId
        });

        if (ret.ErrorCode != 0)
        {
            Log.Error($"chat response errorcode : " + ret.ErrorCode);
            return;
        }
        
        Log.Debug($"get chat route id : {ret.RouteId}");

        var routeComponent = session.AddComponent<RouteComponent>();
        routeComponent.AddAddress(RouteType.ChatRoute, ret.RouteId);
    }
}