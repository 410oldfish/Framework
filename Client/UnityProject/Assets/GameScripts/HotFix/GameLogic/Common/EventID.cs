namespace GameLogic
{
    public static class EventID
    {
        // UI公共事件
        public const int UI_COMMON_MSG = 100001;
        
        //Login
        public const int LOGIN_SHOWSETNICKNAME = 110001;    //登录后要求设置昵称
        
        //--------------Model -> UI-----------------------------
        // Player Data
        // Inventory
        public const int INVENTORY_GOLD_CHANGE = 210001; // 金币变更
        public const int INVENTORY_DIAMOND_CHANGE = 210002; // 钻石变更
        public const int INVENTORY_ITEM_UPDATE_ALL = 210003; // 所有道具数据变更
        public const int INVENTORY_ITEM_UPDATE = 210004; // 单个道具数据变更
        //Farmland
        public const int FARMLAND_LAND_BLOCK_CLICK = 220001; // 农田地块点击事件
        public const int FARMLAND_LAND_UNLOCK_TIMER = 220002; // 农田地块解锁计时器事件
        public const int FARMLAND_LAND_UNLOCK_FINISH = 220003; // 农田地块解锁完成事件
        public const int FARMLAND_LAND_TYPE_CHANGE = 220004; // 农田地块类型改变事件
        public const int FARMLAND_LAND_WATER_TIMER = 220005; // 农田地块浇水计时器事件
        public const int FARMLAND_LAND_WATER_FINISH = 220006; // 农田地块浇水完成事件
        public const int FARMLAND_LAND_DEPEST_TIMER = 220007; // 农田地块除虫计时器事件
        public const int FARMLAND_LAND_DEPEST_FINISH = 220008; // 农田地块除虫完成事件
        public const int FARMLAND_LAND_UPDATE_DATA = 220009; // 农田地块数据更新事件
        public const int FARMLAND_LAND_UPDATE_DATA_ALL = 220010; // 农田地块更新所有数据事件
    }
}