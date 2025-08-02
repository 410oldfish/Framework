using Fantasy;
using Fantasy.Entitas;
using Fantasy.Entitas.Interface;
using GameConfig;
using GameConfig.item;
using GameConfig.site;
using global::GameConfig.global;
using Luban;

namespace Hotfix.Config;

public sealed class OnSceneAwakeSystem : AwakeSystem<ConfigHelper>
{
    protected override void Awake(ConfigHelper self)
    {
        self.Load();
    }
}

public class ConfigHelper : Entity
{
    private bool _init = false;

    private Tables _tables;

    public Tables Tables
    {
        get
        {
            if (!_init)
            {
                Load();
            }

            return _tables;
        }
    }

    /// <summary>
    /// 加载配置。
    /// </summary>
    public void Load()
    {
        _tables = new Tables(LoadByteBuf);
        _init = true;
    }

    /// <summary>
    /// 加载二进制配置。
    /// </summary>
    /// <param name="file">FileName</param>
    /// <returns>ByteBuf</returns>
    private ByteBuf LoadByteBuf(string file)
    {
        string prefix = "../../../../GameConfig/";
        byte[] bytes = File.ReadAllBytes(prefix + file + ".bytes");
        return new ByteBuf(bytes);
    }
    
    //快速访问
    public TbGlobal GlobalConfig => Tables.TbGlobal;
    public TbFarmlandUnlock FarmlandUnlockConfig => Tables.TbFarmlandUnlock;
    public TbFarmlandType FarmlandTypeConfig => Tables.TbFarmlandType;
    
    public TbSeed SeedConfig => Tables.TbSeed;
    public TbFertilizer FertilizerConfig => Tables.TbFertilizer;
    public TbDepester DepesterConfig => Tables.TbDepester;
}