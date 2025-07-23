using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Log = TEngine.Log;

namespace GameScripts.HotFix.GameProto.Handler
{
    public class Chat2C_PushChatRouteMsgHandler : Message<Chat2C_PushChatRouteMsg>
    {
        protected override async FTask Run(Session session, Chat2C_PushChatRouteMsg message)
        {
            Log.Debug("get message from chat: " + message.Tag);
            await FTask.CompletedTask;
        }
    }
}