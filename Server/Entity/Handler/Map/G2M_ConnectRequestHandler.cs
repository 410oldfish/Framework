using Fantasy;
using Fantasy.Async;
using Fantasy.Entitas.Interface;
using Fantasy.Network.Interface;
using Fantasy.Network.Route;

namespace Entity.Handler.Map;

public class MapUser : Fantasy.Entitas.Entity
{
    public string name;
}

public class HelloDBEntity : Fantasy.Entitas.Entity , ISupportedDataBase
{
    public string Tag;
}

public class G2M_ConnectRequestHandler : RouteRPC<Scene, G2M_ConnectRequest, M2G_ConnectResponse>
{
    protected override async FTask Run(Scene scene, G2M_ConnectRequest request, M2G_ConnectResponse response, Action reply)
    {
        var mapUser = Fantasy.Entitas.Entity.Create<MapUser>(scene, true, true);
        mapUser.name = "connect test user";
        mapUser.AddComponent<AddressableMessageComponent>().Register();
        response.AddressableId = mapUser.Id;
        await FTask.CompletedTask;
    }
}