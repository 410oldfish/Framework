using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;

namespace Entity.Handler;

public class C2Chat_HelloRouteRequestHandler : RouteRPC<ChatEntity, C2Chat_HelloRouteRequest, Chat2C_HelloRouteResponse>
{
    protected override async FTask Run(ChatEntity entity, C2Chat_HelloRouteRequest request, Chat2C_HelloRouteResponse response, Action reply)
    {
        Log.Debug($" scene : {entity.name} get request:{request.Tag}");
        response.Tag = "Hi " + request.Tag + " ! this is a rep from chat route request handler.";
        await FTask.CompletedTask;
    }
}