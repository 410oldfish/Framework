using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;

namespace Entity.Handler;

public class ChatEntity : Fantasy.Entitas.Entity
{
    public string name;
    public long GateRouteId;
}

public class G2Chat_ConnectRequestHandler : RouteRPC<Scene, G2Chat_ConnectRequest, Chat2G_ConnectResponse>
{
    protected override async FTask Run(Scene scene, G2Chat_ConnectRequest request, Chat2G_ConnectResponse response, Action reply)
    {
        var chatEntity = Fantasy.Entitas.Entity.Create<ChatEntity>(scene, true, true);
        chatEntity.name = "Chat1";
        chatEntity.GateRouteId = request.GateRouteId;
        response.RouteId = chatEntity.RuntimeId;
        Log.Debug($"GateRouteId:{request.GateRouteId}");
        await FTask.CompletedTask;
    }
}