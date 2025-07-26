using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Fantasy;
using GameLogic;
using TEngine;
using UnityEngine;
using QFramework;
using WeChatWASM;
using Fantasy.Platform.Unity;
using Log = TEngine.Log;

#pragma warning disable CS0436


/// <summary>
/// 游戏App。
/// </summary>
public partial class GameApp
{
    private static List<Assembly> _hotfixAssembly;
    private static IArchitecture framework;

    /// <summary>
    /// 热更域App主入口。
    /// </summary>
    /// <param name="objects"></param>
    public static void Entrance(object[] objects)
    {
        GameEventHelper.Init();
        _hotfixAssembly = (List<Assembly>)objects[0];
        Log.Warning("======= 看到此条日志代表你成功运行了热更新代码 =======");
        Log.Warning("======= Entrance GameApp =======");
        Utility.Unity.AddDestroyListener(Release);
        StartGameLogic();
    }

    private static async void StartGameLogic()
    {
        framework = GameMgr.Interface;
        //Network
        await GameModule.Network.InitServer(_hotfixAssembly.ToArray());
        //开始游戏前自动连接网关
        GameModule.Network.SessionConnect();
        
        GameModule.UI.ShowUI<Pfb_LoginCenterServer>();
    }
    
    private static void Release()
    {
        SingletonSystem.Release();
        Log.Warning("======= Release GameApp =======");
    }
}