using Fantasy;
using Fantasy.Async;
using Fantasy.Entitas.Interface;
using Fantasy.Event;
using GameConfig.item;
using Hotfix.Common.Entity.Base;
using Hotfix.Common.Misc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace Hotfix;

public sealed class On_InventoryDataEntity_AwakeSystem : AwakeSystem<InventoryDataEntity>
{
    protected override void Awake(InventoryDataEntity self)
    {
        self.Scene.World.DataBase.Save<InventoryDataEntity>(self);
        //开启自动存库任务
        var timerId = self.Scene.TimerComponent.Net.RepeatedTimer(1000 * GameHelper.AUTO_SAVE_INTERVAL, new Event_AutoSave<InventoryDataEntity>(self));
        
        self.SetSaveTaskId(timerId);
    }
}

public sealed class On_InventoryDataEntity_Deserialize : DeserializeSystem<InventoryDataEntity>
{
    protected override void Deserialize(InventoryDataEntity self)
    {
        //开启自动存库任务
        var timerId = self.Scene.TimerComponent.Net.RepeatedTimer(1000 * GameHelper.AUTO_SAVE_INTERVAL, new Event_AutoSave<InventoryDataEntity>(self));
        
        self.SetSaveTaskId(timerId);
    }
}



public sealed class OnAutoSave_InventoryDataEntity : EventSystem<Event_AutoSave<InventoryDataEntity>>
{
    protected override void Handler(Event_AutoSave<InventoryDataEntity> self)
    {
        Log.Debug("1111");
        if(self.Data.IsDirty)
        {
            self.Data.Scene.World.DataBase.Save<InventoryDataEntity>(self.Data);
            self.Data.ClearDirty(); // 清除脏标记
            Log.Debug(" Dirty : {0}", self.Data.IsDirty);
            Log.Debug("Auto Save InventoryDataEntity: {0}", self.Data.RuntimeId);
        }
    }
}

public sealed class On_InventoryDataEntity_Dispose : DestroySystem<InventoryDataEntity>
{
    protected override void Destroy(InventoryDataEntity self)
    {
        self.Scene.TimerComponent.Net.Remove(self.SaveTaskId);
        Log.Debug( "Dispose InventoryDataEntity: {0}", self.RuntimeId);
    }
}

public class InventoryDataEntity : PlayerDataBase
{
    [BsonDictionaryOptions(DictionaryRepresentation.Document)]
    private Dictionary<string, int> ItemDictionary { get; set; } = new();

    public List<ItemProto> GetInventoryItems()
    {
        List<ItemProto> items = new();

        foreach (var kvp in ItemDictionary)
        {
            if (int.TryParse(kvp.Key, out int id))
            {
                items.Add(new ItemProto { Id = id, Count = kvp.Value });
            }
        }

        return items;
    }

    public Dictionary<int, int> GetCurItems(List<ItemExchange> items)
    {
        Dictionary<int, int> curItems = new Dictionary<int, int>();
        foreach (var item in items)
        {
            curItems[item.Id] = ItemDictionary.GetValueOrDefault(item.Id.ToString(), 0); // 如果不存在该物品，则数量为0
        }
        return curItems;
    }

    public static Dictionary<int, int> GetDeltaItems(List<ItemExchange> items)
    {
        Dictionary<int, int> deltaItems = new Dictionary<int, int>();
        foreach (var item in items)
        {
            if (item.Count > 0)
            {
                deltaItems[item.Id] = item.Count; // 只记录数量大于0的物品
            }
        }
        return deltaItems;
    }

    public bool CheckItemCount(int itemId, int count)
    {
        if (!ItemDictionary.ContainsKey(itemId.ToString()))
        {
            return false; // 不存在该物品
        }
        
        return ItemDictionary[itemId.ToString()] >= count; // 检查数量是否足够
    }
    
    public bool CheckItemCount(List<ItemExchange> items)
    {
        foreach (var item in items)
        {
            if (!CheckItemCount(item.Id, item.Count))
            {
                return false; // 只要有一个物品数量不足就返回false
            }
        }
        return true; // 所有物品数量都足够
    }
    
    /// <summary>
    /// 增加物品唯一方法
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public ItemProto AddItem(int itemId, int count, bool dirty = true)
    {
        string itemKey = itemId.ToString();
        if (count <= 0)
        {
            Log.Error("InventoryDataEntity: AddItem failed, count must be greater than 0: {0}", count);
            return null;
        }

        if (!ItemDictionary.TryAdd(itemKey, count))
        {
            ItemDictionary[itemKey] += count;
        }

        if (dirty)
        {
            SetDirty();
        }

        return new ItemProto(){Id = itemId, Count = ItemDictionary[itemKey]};
    }

    public List<ItemProto> AddItems(List<ItemExchange> items)
    {
        List<ItemProto> curItems = new List<ItemProto>(items.Count);
        if(items.Count == 0)
        {
            return curItems; // 如果没有物品，返回空列表
        }
        foreach (var item in items)
        {
            int itemId = item.Id;
            curItems.Add(AddItem(itemId, item.Count)); // 成功添加，记录数量
        }

        SetDirty();
        return curItems; // 返回每个物品的添加数量
    }
    
    public List<ItemProto> AddItems(List<ItemProto> items)
    {
        List<ItemProto> curItems = new List<ItemProto>(items.Count);
        if(items.Count == 0)
        {
            return curItems; // 如果没有物品，返回空列表
        }
        foreach (var item in items)
        {
            int itemId = item.Id;
            curItems.Add(AddItem(itemId, item.Count, false)); // 成功添加，记录数量
        }

        SetDirty();
        return curItems; // 返回每个物品的添加数量
    }
    
    /// <summary>
    /// 消耗物品唯一方法
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public ItemProto RemoveItem(int itemId, int count, bool dirty = true)
    {
        string itemKey = itemId.ToString();
        if (!ItemDictionary.ContainsKey(itemKey))
        {
            Log.Error("InventoryDataEntity: RemoveItem failed, item not found: {0}", itemId);
            return null;
        }
        
        if (ItemDictionary[itemKey] < count)
        {
            Log.Error("InventoryDataEntity: RemoveItem failed, not enough item count: {0}, required: {1}, available: {2}", itemId, count, ItemDictionary[itemKey]);
            return null; // 不足以移除
        }
        
        ItemDictionary[itemKey] -= count;
        int curCount = ItemDictionary[itemKey];
        if (ItemDictionary[itemKey] <= 0)
        {
            ItemDictionary.Remove(itemKey); // 如果数量为0，移除该项
        }
        
        if(dirty) 
        {
            SetDirty(); // 设置脏标记
        }
        return new ItemProto(){Id = itemId, Count = curCount}; // 成功移除
    }
    
    public List<ItemProto> RemoveItems(List<ItemExchange> items)
    {
        List<ItemProto> curItems = new List<ItemProto>(items.Count);
        
        if(items.Count == 0)
        {
            return curItems; // 如果没有物品，返回空列表
        }
        
        foreach (var item in items)
        {
            curItems.Add(RemoveItem(item.Id, item.Count, false)); // 成功移除，记录数量
        }
        SetDirty();
        return curItems; // 返回每个物品的移除数量
    }
}