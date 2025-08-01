namespace GameLogic
{
    public static class EventID
    {
        // UI 事件
        public const int UI_COMMON_MSG = 100001;
        
        //Login
        public const int LOGIN_SHOWSETNICKNAME = 110001;    //登录后要求设置昵称
        
        //--------------Model -> UI-----------------------------
        // Player Data
        // Inventory
        public const int INVENTORY_GOLD_CHANGE = 210001; // 金币变更
        public const int INVENTORY_DIAMOND_CHANGE = 210002; // 钻石变更
        public const int INVENTORY_ITEM_COUNT_CHANGE = 210003; // 道具数量变更
    }
}