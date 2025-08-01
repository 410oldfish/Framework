using Fantasy;
using Fantasy.Async;
using Fantasy.Network.Interface;
using Hotfix.Center.Entity;

namespace Hotfix;

public class G2Center_DisposeCenterUnitHandler : Route<Scene, G2Center_DisposeCenterUnit_Msg>
{
    protected override async FTask Run(Scene scene, G2Center_DisposeCenterUnit_Msg message)
    {
        scene.GetEntity<CenterUnit>(message.CenterRouteId)?.Dispose();
        Log.Debug("Dispose CenterUnit: {0}", message.CenterRouteId);
        await FTask.CompletedTask;
    }
}