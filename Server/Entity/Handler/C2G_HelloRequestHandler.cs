using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace Entity.Handler;

public class C2G_HelloRequestHandler : MessageRPC<C2G_HelloRequest, G2C_HelloResponse>
{
    protected override async FTask Run(Session session, C2G_HelloRequest request, G2C_HelloResponse response, Action reply)
    {
        Log.Debug("Get req from client : " + request.Tag);
        response.Tag = "Hi " + request.Tag + " !!!";
        await FTask.CompletedTask;
    }
}