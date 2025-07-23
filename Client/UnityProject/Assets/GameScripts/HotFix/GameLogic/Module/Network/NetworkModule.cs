using System;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using TEngine;
using UnityEngine;
using Log = TEngine.Log;

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

        public void Send<TMessage>(TMessage msg)
        where TMessage : IMessage
        {
            _session.Send(msg);
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
        
        public async FTask<TResponse> CreateRoute<TRequest,TResponse>(TRequest request)
        where TRequest : IRequest
        where TResponse : IResponse
        {
            if (_session == null || _session.IsDisposed)
            {
                throw new InvalidOperationException("Session is not connected or has been disposed.");
            }

            var rep = (TResponse)await _session.Call(request);
            if(rep.ErrorCode != 0)
            {
                Log.Error("route rep error ! " + rep.ErrorCode);
            }
            return rep;
        }
        
        public void SendRoute()
        {
            _session.Send(new C2Chat_HelloRouteMsg()
            {
                Tag = "Hello Custom Route Message"
            });
        }

        public async FTask<TRouteResponse> CallRoute<TRouteRequest, TRouteResponse>(TRouteRequest request)
        where TRouteRequest : IRouteRequest
        where TRouteResponse : IRouteResponse
        {
            if (_session == null || _session.IsDisposed)
            {
                throw new InvalidOperationException("Session is not connected or has been disposed.");
            }

            var response = (TRouteResponse) await _session.Call(request);
            if (response == null)
            {
                throw new InvalidCastException($"Response cannot be cast to type {typeof(TRouteResponse).Name}");
            }
            
            return response;
        }

        public void RegisterAddressable<TRequest, TResponse>(string address)
            where TRequest : IRequest
            where TResponse : IResponse
        {
            if (_session == null || _session.IsDisposed)
            {
                throw new InvalidOperationException("Session is not connected or has been disposed.");
            }
            
            
        }
    }
}