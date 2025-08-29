using System.Collections.Generic;
using Fantasy;
using GameConfig;
using GameConfig.item;
using QFramework;
using TEngine;

namespace GameLogic
{
    //仓库数据
    public class InventoryModel : AbstractModel
    {
        int _gold = 0;
        public int Gold
        {
            get => _gold;
            private set
            {
                _gold = value;
                GameEvent.Send(EventID.INVENTORY_GOLD_CHANGE, _gold);
            }
        }

        int _diamond = 0;
        public int Diamond
        {
            get => _diamond;
            private set
            {
                _diamond = value;
                GameEvent.Send(EventID.INVENTORY_DIAMOND_CHANGE, _diamond);
            }
        }

        Dictionary<int, int> inventoryData = new Dictionary<int, int>();
        public IReadOnlyDictionary<int, int> InventoryData
        {
            get => inventoryData;
        }

        public List<ItemProto> GetItemsByType(EItemType itemType)
        {
            var idRange = Misc.ItemTypeConfigIdRangeMap[itemType];
            List<ItemProto> items = new List<ItemProto>();
            foreach (var kvp in inventoryData)
            {
                if (kvp.Key >= idRange.Min && kvp.Key <= idRange.Max)
                {
                    items.Add(new ItemProto { Id = kvp.Key, Count = kvp.Value });
                }
            }

            return items;
        }

        public int GetItemCount(int itemId)
        {
            if (itemId == Misc.GOLD_ID) return Gold;
            if (itemId == Misc.DIAMOND_ID) return Diamond;
            
            if (inventoryData == null || !inventoryData.ContainsKey(itemId))
            {
                return 0;
            }
            return inventoryData[itemId];
        }
        
        private void SetItemCount(int itemId, int count)
        {
            if(itemId == Misc.GOLD_ID)
            {
                Gold = count;
                return;
            }
            if(itemId == Misc.DIAMOND_ID)
            {
                Diamond = count;
                return;
            }
            
            if (inventoryData == null)
            {
                inventoryData = new Dictionary<int, int>();
            }
            
            if (count <= 0)
            {
                inventoryData.Remove(itemId);
            }
            else
            {
                inventoryData[itemId] = count;
            }
        }
        
        public void UpdateItems(List<ItemProto> items)
        {
            if(items.Count == 0)
            {
                return;
            }

            foreach (var item in items)
            {
                SetItemCount(item.Id, item.Count);
            }

            GameEvent.Send(EventID.INVENTORY_ITEM_UPDATE_ALL);
        }

        public void UpdateItem(ItemProto proto)
        {
            SetItemCount(proto.Id, proto.Count);
            GameEvent.Send(EventID.INVENTORY_ITEM_UPDATE, proto.Id);
        }

        public bool CheckItemsCount(List<ItemExchange> items)
        {
            foreach (var item in items)
            {
                if (!CheckItemCount(item))
                {
                    return false; // 只要有一个物品数量不足就返回false
                }
            }
            return true; // 所有物品数量都足够
        }

        /// <summary>
        /// 检查物品数量是否足够
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CheckItemCount(ItemExchange item)
        {
            return GetItemCount(item.Id) >= item.Count;
        }

        public bool CheckItemCountOne(int itemId)
        {
            return GetItemCount(itemId) >= 1;
        }

        protected override void OnInit()
        {
            
        }
    }
}