using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;

namespace Entity.Handler;

public class G2Chat_HelloRouteRequestHandler : RouteRPC<Scene, G2Chat_HelloRouteRequest, G2Chat_HelloRouteResponse>

{
    protected override async FTask Run(Scene entity, G2Chat_HelloRouteRequest request, G2Chat_HelloRouteResponse response, Action reply)
    {
        Log.Debug($" scene : {entity.SceneType} get request:{request.Tag}");
        response.Tag = "Hi " + request.Tag + " ! this is a rep from chat route request handler.";
        await FTask.CompletedTask;
    }
}