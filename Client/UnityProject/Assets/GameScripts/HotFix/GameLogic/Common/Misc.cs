using System.Collections.Generic;
using GameConfig;
using GameConfig.farm;
using GameConfig.site;
using UnityEngine;

namespace GameLogic
{
    struct GridXY
    {
        public int X;
        public int Y;

        public GridXY(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public struct ConfigIdRange
    {
        public EItemType ItemType;
        public int Min;
        public int Max;
        public ConfigIdRange(EItemType itemType, int min, int max)
        {
            ItemType = itemType;
            Min = min;
            Max = max;
        }
    }
    
    public static class Misc
    {
        public const int GOLD_ID = 1000001;
        public const int DIAMOND_ID = 1000002;
        
        //---------------------------
        
        static readonly ConfigIdRange CommonItemRange = new ConfigIdRange(EItemType.Common, 1000000, 1009999);
        //农场
        static readonly ConfigIdRange SeedItemRange = new ConfigIdRange(EItemType.Seed, 1100000, 1109999);
        static readonly ConfigIdRange CropItemRange = new ConfigIdRange(EItemType.Crop, 1110000, 1119999);
        static readonly ConfigIdRange FertilizerItemRange = new ConfigIdRange(EItemType.Fertilizer, 1120000, 1129999);
        static readonly ConfigIdRange DePesterItemRange = new ConfigIdRange(EItemType.DePester, 1130000, 1139999);
        //牧场
        static readonly ConfigIdRange CubItemRange = new ConfigIdRange(EItemType.Cub, 1200000, 1209999);
        static readonly ConfigIdRange LivestockItemRange = new ConfigIdRange(EItemType.Livestock, 1210000, 1219999);
        static readonly ConfigIdRange RanchProductItemRange = new ConfigIdRange(EItemType.RanchProduct, 1220000, 1229999);
        static readonly ConfigIdRange RanchByProductItemRange = new ConfigIdRange(EItemType.RanchByProduct, 1230000, 1239999);
        //渔场
        static readonly ConfigIdRange FryItemRange = new ConfigIdRange(EItemType.Fry, 1300000, 1309999);
        static readonly ConfigIdRange FishItemRange = new ConfigIdRange(EItemType.Fish, 1310000, 1319999);
        static readonly ConfigIdRange FishProductItemRange = new ConfigIdRange(EItemType.FishProduct, 1320000, 1329999);
        //雇员
        static readonly ConfigIdRange WorkerItemRange = new ConfigIdRange(EItemType.Worker, 2000000, 2009999);
        //家具
        static readonly ConfigIdRange FornitureItemRange = new ConfigIdRange(EItemType.Forniture, 3000000, 3009999);

        public static Dictionary<EItemType, ConfigIdRange> ItemTypeConfigIdRangeMap = new Dictionary<EItemType, ConfigIdRange>
        {
            { EItemType.Common, CommonItemRange },
            { EItemType.Seed, SeedItemRange },
            { EItemType.Crop, CropItemRange },
            { EItemType.Fertilizer, FertilizerItemRange },
            { EItemType.DePester, DePesterItemRange },
            { EItemType.Cub, CubItemRange },
            { EItemType.Livestock, LivestockItemRange },
            { EItemType.RanchProduct, RanchProductItemRange },
            { EItemType.RanchByProduct, RanchByProductItemRange },
            { EItemType.Fry, FryItemRange },
            { EItemType.Fish, FishItemRange },
            { EItemType.FishProduct, FishProductItemRange },
            { EItemType.Worker, WorkerItemRange },
            { EItemType.Forniture, FornitureItemRange }
        };
        
        /// <summary>
        /// 获取物品类型，根据物品ID
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static EItemType GetItemTypeById(int itemId)
        {
            if (itemId >= CommonItemRange.Min && itemId <= CommonItemRange.Max)
            {
                return CommonItemRange.ItemType;
            }
            else if (itemId >= SeedItemRange.Min && itemId <= SeedItemRange.Max)
            {
                return SeedItemRange.ItemType;
            }
            else if (itemId >= CropItemRange.Min && itemId <= CropItemRange.Max)
            {
                return CropItemRange.ItemType;
            }
            else if (itemId >= FertilizerItemRange.Min && itemId <= FertilizerItemRange.Max)
            {
                return FertilizerItemRange.ItemType;
            }
            else if (itemId >= DePesterItemRange.Min && itemId <= DePesterItemRange.Max)
            {
                return DePesterItemRange.ItemType;
            }
            else if (itemId >= CubItemRange.Min && itemId <= CubItemRange.Max)
            {
                return CubItemRange.ItemType;
            }
            else if (itemId >= LivestockItemRange.Min && itemId <= LivestockItemRange.Max)
            {
                return LivestockItemRange.ItemType;
            }
            else if (itemId >= RanchProductItemRange.Min && itemId <= RanchProductItemRange.Max)
            {
                return RanchProductItemRange.ItemType;
            }
            else if (itemId >= RanchByProductItemRange.Min && itemId <= RanchByProductItemRange.Max)
            {
                return RanchByProductItemRange.ItemType;
            }
            else if (itemId >= FryItemRange.Min && itemId <= FryItemRange.Max)
            {
                return FryItemRange.ItemType;
            }
            else if (itemId >= FishItemRange.Min && itemId <= FishItemRange.Max)
            {
                return FishItemRange.ItemType;
            }
            else if (itemId >= FishProductItemRange.Min && itemId <= FishProductItemRange.Max)
            {
                return FishProductItemRange.ItemType;
            }
            else if (itemId >= WorkerItemRange.Min && itemId <= WorkerItemRange.Max)
            {
                return WorkerItemRange.ItemType;
            }
            else if (itemId >= FornitureItemRange.Min && itemId <= FornitureItemRange.Max)
            {
                return FornitureItemRange.ItemType;
            }

            return EItemType.Common; // 未知类型
        }

        /// <summary>
        /// 获取物品图标路径
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static string GetBagItemIcon(int itemId)
        {
            return "img_BagIcon_" + itemId.ToString();
        }

        public static ItemBase GetItemConfig(int itemId)
        {
            var itemType = GetItemTypeById(itemId);
            if (itemType == EItemType.Common)
            {
                return ConfigSystem.Instance.Tables.TbCommonItem.Get(itemId);
            }
            else if (itemType == EItemType.Seed)
            {
                return ConfigSystem.Instance.Tables.TbSeed.Get(itemId);
            }
            else if (itemType == EItemType.Crop)
            {
                return ConfigSystem.Instance.Tables.TbCrops.Get(itemId);
            }
            else if (itemType == EItemType.Fertilizer)
            {
                return ConfigSystem.Instance.Tables.TbFertilizer.Get(itemId);
            }
            else if (itemType == EItemType.DePester)
            {
                return ConfigSystem.Instance.Tables.TbDepester.Get(itemId);
            }
            else if (itemType == EItemType.Cub)
            {
                return ConfigSystem.Instance.Tables.TbCub.Get(itemId);
            }
            else if (itemType == EItemType.Livestock)
            {
                return ConfigSystem.Instance.Tables.TbLivestock.Get(itemId);
            }
            else if (itemType == EItemType.RanchProduct)
            {
                return ConfigSystem.Instance.Tables.TbRanchProduct.Get(itemId);
            }
            else if (itemType == EItemType.RanchByProduct)
            {
                return ConfigSystem.Instance.Tables.TbRanchProduct.Get(itemId);
            }
            else if (itemType == EItemType.Fry)
            {
                return ConfigSystem.Instance.Tables.TbFry.Get(itemId);
            }
            else if (itemType == EItemType.Fish)
            {
                return ConfigSystem.Instance.Tables.TbFish.Get(itemId);
            }
            else if (itemType == EItemType.FishProduct)
            {
                return ConfigSystem.Instance.Tables.TbFishProduct.Get(itemId);
            }
            // else if (itemType == EItemType.Worker)
            // {
            //     return ConfigSystem.Instance.Tables.TbWorker.Get(itemId);
            // }
            else if (itemType == EItemType.Forniture)
            {
                return ConfigSystem.Instance.Tables.TbForniture.Get(itemId);
            }

            return null; // 未知类型
        }

        /// <summary>
        /// 通过当前的游戏模块获取需要显示的背包标签
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static List<EItemType> GetBagTabsByGameModule(EGameModule module)
        {
            if (module == EGameModule.Farmland)
            {
                return new List<EItemType>() { 
                    EItemType.Seed, 
                    EItemType.Crop, 
                    EItemType.Fertilizer, 
                    EItemType.DePester 
                };
            }
            
            if (module == EGameModule.Ranchland)
            {
                return new List<EItemType>() { 
                    EItemType.Cub, 
                    EItemType.Livestock, 
                    EItemType.RanchProduct, 
                    EItemType.RanchByProduct 
                };
            }

            if (module == EGameModule.Fishland)
            {
                return new List<EItemType>() { 
                    EItemType.Fry, 
                    EItemType.Fish, 
                    EItemType.FishProduct 
                };
            }

            return null;
        }

        public static string GetItemTypeName(EItemType itemType)
        {
            switch (itemType)
            {
                case EItemType.Common:
                    return "通用";
                case EItemType.Seed:
                    return "种子";
                case EItemType.Crop:
                    return "农作物";
                case EItemType.Fertilizer:
                    return "肥料";
                case EItemType.DePester:
                    return "杀虫剂";
                case EItemType.Cub:
                    return "幼崽";
                case EItemType.Livestock:
                    return "牲畜";
                case EItemType.RanchProduct:
                    return "牧场产品";
                case EItemType.RanchByProduct:
                    return "牧场副产品";
                case EItemType.Fry:
                    return "鱼苗";
                case EItemType.Fish:
                    return "鱼";
                case EItemType.FishProduct:
                    return "渔场产品";
                case EItemType.Worker:
                    return "员工";
                case EItemType.Forniture:
                    return "家具";
                default:
                    return "Unknown Item Type";
            }
        }

        public static string GetLandTypeImgPath(ELandType landType)
        {
            return "img_landtypes_" + (int)landType;
        }
        
        public static string GetSeedImgPath(int seedId, int stage)
        {
            return "img_seed_" + seedId.ToString()+ "_" + stage.ToString();
        }
        
        /// <summary>
        /// 秒数转换为时分秒格式的字符串
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string SecondToTimeString(long second)
        {
            // 将秒数转换为时分秒格式的字符串
            long hours = second / 3600;
            long minutes = (second % 3600) / 60;
            long seconds = second % 60;
            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
        
        /// <summary>
        ///  将世界坐标转换为屏幕坐标
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public static Vector2 GetScreenPositionByWorldPosition(Vector3 worldPosition)
        {
            // 将世界坐标转换为屏幕坐标
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            return new Vector2(screenPos.x, screenPos.y);
        }
    }
}