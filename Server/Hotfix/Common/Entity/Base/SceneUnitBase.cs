namespace Hotfix.Common.Entity.Base;

public class SceneUnitBase : Fantasy.Entitas.Entity
{
    private long _gateRouteId;
    public long GateRouteId => _gateRouteId;
    public void SetGateRouteId(long gateRouteId)
    {
        _gateRouteId = gateRouteId;
    }

    private long _playerId;
    public long PlayerId => _playerId;
    public void SetPlayerId(long playerId)
    {
        _playerId = playerId;
    }
}