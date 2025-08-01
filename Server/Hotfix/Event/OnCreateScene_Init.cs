using Fantasy;
using Fantasy.Async;
using Fantasy.Entitas;
using Fantasy.Event;
using Hotfix.Config;

public sealed class OnCreateScene_Init : AsyncEventSystem<OnCreateScene>
{
    protected override async FTask Handler(OnCreateScene self)
    {
        var scene = self.Scene;
        
        switch (scene.SceneType)
        {
            case SceneType.Map:
            {
                //挂载配置组件
                scene.AddComponent<ConfigHelper>();
                break;
            }
        }

        await FTask.CompletedTask;
    }
}