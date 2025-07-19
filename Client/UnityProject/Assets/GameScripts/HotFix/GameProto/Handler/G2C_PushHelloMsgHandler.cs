using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using TEngine;

namespace GameScripts.HotFix.GameProto.Handler
{
    public class G2C_PushHelloMsgHandler : Message<G2C_PushHelloMsg>
    {
        protected override async FTask Run(Session session, G2C_PushHelloMsg message)
        {
            Debugger.print("Received message from server: " + message.Tag);
            await FTask.CompletedTask;
        }
    }
}