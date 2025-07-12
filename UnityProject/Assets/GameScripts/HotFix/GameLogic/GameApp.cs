using System.Collections.Generic;
using System.Reflection;
using GameLogic;
using TEngine;
using UnityEngine;

#pragma warning disable CS0436


/// <summary>
/// 游戏App。
/// </summary>
public partial class GameApp
{
    private static List<Assembly> _hotfixAssembly;

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
        FindFontMaterialShaders();
        StartGameLogic();
    }

    //在加载预制体前，把所有TMP材质的Shader重新加载一遍，这些材质都打包在 FontMaterials Group 中
    static async void FindFontMaterialShaders()
    {
        //加载指定group 资源
        var fontMaterials = await GameModule.Resource.LoadAssetAsyncByTag<Material>("FontMaterial");
        foreach (var mat in fontMaterials)
        {
            mat.shader = Shader.Find(mat.shader.name);
        }
    }

    private static void StartGameLogic()
    {
        GameEvent.Get<ILoginUI>().ShowLoginUI();
        GameModule.UI.ShowUIAsync<TestPfb>();
    }
    
    private static void Release()
    {
        SingletonSystem.Release();
        Log.Warning("======= Release GameApp =======");
    }
}