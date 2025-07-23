using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Fantasy.Network.Route;

namespace Entity.Handler.Map;

public class M2M_SendUnitRequestHandler : RouteRPC<Scene, M2M_SendUnitRequest, M2M_SendUnitResponse>
{
    protected override async FTask Run(Scene scene, M2M_SendUnitRequest request, M2M_SendUnitResponse response, Action reply)
    {
        var mapUser = request.user;
        mapUser.Deserialize(scene);
        //这里是因为序列化的时候会把component丢弃
        await mapUser.AddComponent<AddressableMessageComponent>().UnLock("M2M_SendUnitRequest");
        Log.Debug("get mapUser");
    }
}