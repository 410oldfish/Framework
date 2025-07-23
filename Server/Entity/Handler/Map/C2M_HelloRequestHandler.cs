using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;

namespace Entity.Handler.Map;

public class C2M_HelloRequestHandler : AddressableRPC<MapUser, C2M_HelloRequest, M2C_HelloResponse>
{
    protected override async FTask Run(MapUser entity, C2M_HelloRequest request, M2C_HelloResponse response, Action reply)
    {
        Log.Debug($"MapUser {entity.name} received hello request with tag: {request.Tag}");
        response.Tag = $"Hello, {entity.name}! You sent: {request.Tag}";
        await FTask.CompletedTask;
    }
}