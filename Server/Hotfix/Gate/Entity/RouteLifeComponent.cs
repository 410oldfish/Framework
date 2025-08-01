using Fantasy.Entitas;

namespace Hotfix;

public class RouteLifeComponent : Entity
{
    public long centerRouteId;

    public override void Dispose()
    {
        base.Dispose();
        centerRouteId = 0;
    }
}