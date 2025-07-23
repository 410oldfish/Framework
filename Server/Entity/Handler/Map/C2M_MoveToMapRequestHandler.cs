using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Fantasy.Network.Route;
using Fantasy.Platform.Net;

namespace Entity.Handler.Map;

public class C2M_MoveToMapRequestHandler : AddressableRPC<MapUser, C2M_MoveToMapRequest, M2C_MoveToMapResponse>
{
    protected override async FTask Run(MapUser mapUser, C2M_MoveToMapRequest request, M2C_MoveToMapResponse response, Action reply)
    {
        var mapUserScene = mapUser.Scene;
        var mapSceneConfig = SceneConfigData.Instance.GetSceneBySceneType(SceneType.Map)[1];

        await mapUser.GetComponent<AddressableMessageComponent>().LockAndRelease();
        var moveResponse = (M2M_SendUnitResponse) await mapUserScene.NetworkMessagingComponent.CallInnerRoute(mapSceneConfig.RouteId, new M2M_SendUnitRequest()
        {
            user = mapUser
        });

        if (moveResponse.ErrorCode != 0)
        {
            Log.Error("error when trans to map!");
            return;
        }
        
        mapUser.Dispose();
        Log.Debug("Trans to map success");
    }
}