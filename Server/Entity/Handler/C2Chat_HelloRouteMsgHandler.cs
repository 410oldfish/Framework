using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;

namespace Entity.Handler;

public class C2Chat_HelloRouteMsgHandler : Route<ChatEntity, C2Chat_HelloRouteMsg>
{
    protected override async FTask Run(ChatEntity entity, C2Chat_HelloRouteMsg message)
    {
        Log.Debug($"chat entity : {entity.name}  {message.Tag}");
        entity.Scene.NetworkMessagingComponent.SendInnerRoute(entity.GateRouteId, new Chat2C_PushChatRouteMsg()
        {
            Tag = "C2Chat_HelloRouteMsgHandler"
        });
        await FTask.CompletedTask;
    }
}