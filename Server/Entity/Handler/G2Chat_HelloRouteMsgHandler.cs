using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;

namespace Entity.Handler;

public class G2Chat_HelloRouteMsgHandler : Route<Scene, G2Chat_HelloRouteMsg>
{
    protected override async FTask Run(Scene scene, G2Chat_HelloRouteMsg message)
    {
        Log.Debug($" scene : {scene.SceneType} get message:{message.Tag}");
        await FTask.CompletedTask;
    }
}