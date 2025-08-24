using GameConfig;
using GameConfig.item;
using GameConfig.site;

namespace GameLogic
{
    public class ConfigHelper
    {
        //Fast Config
        public static TbFarmlandUnlock FarmlandUnlockConfig = ConfigSystem.Instance.Tables.TbFarmlandUnlock;
        public static TbSeed SeedConfig = ConfigSystem.Instance.Tables.TbSeed;
    }
}