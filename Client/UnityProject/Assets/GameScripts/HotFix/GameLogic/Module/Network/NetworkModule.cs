using System;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public sealed class NetworkModule : Singleton<NetworkModule>
    {
        Scene _scene;
        public Scene Scene => _scene;
        private Session _session;
        public async void InitServer(params System.Reflection.Assembly[] assemblies)
        {
            await Fantasy.Platform.Unity.Entry.Initialize(assemblies);
            _scene = await Fantasy.Platform.Unity.Entry.CreateScene();
        }

        public void SessionConnect()
        {
            _session = _scene.Connect(
                "127.0.0.1:20000",
                NetworkProtocolType.WebSocket,
                () =>
                {
                    _session.AddComponent<SessionHeartbeatComponent>().Start(1000);
                    Debugger.print("连接成功");
                },
                () =>
                {
                    Debugger.print("连接失败");
                },
                ()=>{
                    Debugger.print("连接断开");
                },
                false);
        }

        public void Send()
        {
            _session.Send(new C2G_HelloFantasy()
            {
                Tag = "Hello Fantasy !!!"
            });
        }

        public async FTask<TResponse> RPC<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest
            where TResponse : IResponse
        {
            if (_session == null || _session.IsDisposed)
            {
                throw new InvalidOperationException("Session is not connected or has been disposed.");
            }

            var response = (TResponse) await _session.Call(request);
            if (response == null)
            {
                throw new InvalidCastException($"Response cannot be cast to type {typeof(TResponse).Name}");
            }

            return response;
        }
        
        public void Dispose()
        {
            if (_session is { IsDisposed: false })
            {
                _session.Dispose();
                _session = null;
                Debugger.print("Session disposed.");
            }
        }
    }
}