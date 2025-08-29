using System.Collections.Generic;
using GameConfig.item;

namespace GameLogic
{
    public class StringDefine
    {
        public static readonly string NOT_ENOUGH_ITEM = "道具不足{0}";
        public static readonly string NOT_ARRIVE_HARVEST_TIME = "未到收获时间{0}";
        public static string NotEnoughItem(int itemId, int itemCount)
        {
            return string.Format(NOT_ENOUGH_ITEM, itemId + "x" + itemCount);
        }

        public static string NotEnoughItem(List<ItemExchange> items)
        {
            string itemStr ="";
            foreach (var item in items)
            {
                itemStr += item.Id + "x" + item.Count + " ";
            }
            return string.Format(NOT_ENOUGH_ITEM, itemStr);
        }
        
        public static string NotArriveHarvestTime(int seedId)
        {
            return string.Format(NOT_ARRIVE_HARVEST_TIME, seedId);
        }
    }
}