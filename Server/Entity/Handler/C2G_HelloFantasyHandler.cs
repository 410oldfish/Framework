using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Fantasy.Platform.Net;

namespace Entity.Handler;

public class C2G_HelloFantasyHandler : Message<C2G_HelloFantasy>
{
    protected override async FTask Run(Session session, C2G_HelloFantasy message)
    {
        Log.Debug($"get message:{message.Tag}");
        var chatScene = SceneConfigData.Instance.GetSceneBySceneType(SceneType.Chat);
        var chatSceneConfig = chatScene[0];
        
        session.Scene.NetworkMessagingComponent.SendInnerRoute(chatSceneConfig.RouteId, new G2Chat_HelloRouteMsg()
        {
            Tag = "Hello Chat, this is a route message from gate"
        });
        
        // Route Request
        var response = (G2Chat_HelloRouteResponse)await session.Scene.NetworkMessagingComponent.CallInnerRoute(
            chatSceneConfig.RouteId, new G2Chat_HelloRouteRequest()
            {
                Tag = "Hello Chat, this is a route request from gate"
            });
        Log.Debug(response.Tag);
        await FTask.CompletedTask;
    }
}