using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace Entity.Handler.Map;

public class C2M_HelloMsgHandler : Addressable<MapUser, C2M_HelloMsg>
{
    protected override async FTask Run(MapUser entity, C2M_HelloMsg message)
    {
        var worldDataBase = entity.Scene.World.DataBase;
        var mapUserList = await worldDataBase.Query<MapUser>(d=> true);
        Log.Debug("MapUserList count: " + mapUserList.Count);
        foreach (var user in mapUserList)
        {
            Log.Debug($"{user.Id}");
        }
        
        Log.Debug( $"Map {entity.Scene.SceneConfigId} 's  MapUser {entity.Id} received message: {message.Tag}");
        await FTask.CompletedTask;
    }
}