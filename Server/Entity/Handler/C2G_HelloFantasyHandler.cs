using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace Entity.Handler;

public class C2G_HelloFantasyHandler : Message<C2G_HelloFantasy>
{
    protected override async FTask Run(Session session, C2G_HelloFantasy message)
    {
        Log.Debug($"get message:{message.Tag}");
        session.Send(new G2C_PushHelloMsg()
        {
            Tag = "server push to client : G2C_PushHelloMsg"
        });
        await FTask.CompletedTask;
    }
}