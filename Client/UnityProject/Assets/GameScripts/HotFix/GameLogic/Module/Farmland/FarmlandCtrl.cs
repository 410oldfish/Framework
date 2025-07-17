using GameConfig;
using GameLogic.GameScripts.HotFix.GameLogic.Common;
using GameLogic.GameScripts.HotFix.GameLogic.Module.Player;
using QFramework;
using TEngine;

namespace GameLogic.GameScripts.HotFix.GameLogic.Module.Farmland
{
    //农场的控制器
    public class FarmlandCtrl : AbstractSystem, ICanSendCommand
    {
        protected override void OnInit()
        {
            
        }
        
        //行为----------------------------------------------------

        /// <summary>
        /// 解锁新的农田
        /// <summary>
        public bool UnlockNewFarmland(int farmlandId)
        {
            FarmlandUnlock unlockCfg = ConfigSystem.Instance.Tables.TbFarmlandUnlock.DataMap[farmlandId];
            if (unlockCfg == null)
            {
                Debugger.print("FarmlandCtrl.UnlockNewFarmland: farmlandId not found in config : " + farmlandId);
                GameEvent.Send(EventID.UI_COMMON_MSG, "配置错误 : " + farmlandId);
                return false;
            }
            
            //检查是否满足解锁条件
            PlayerModel playerModel = this.GetModel<PlayerModel>();
            if (playerModel.PlayerLevel < unlockCfg.NeedLv)
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, "解锁该农田需要玩家等级达到 " + unlockCfg.NeedLv);
                return false;
            }
            
            //检查消耗
            InventoryCtrl inventoryCtrl = this.GetSystem<InventoryCtrl>();
            var costArr = unlockCfg.Cost;
            foreach (var cost in costArr)
            {
                if(inventoryCtrl.GetItemCount(cost.Id) < cost.Num)
                {
                    GameEvent.Send(EventID.UI_COMMON_MSG, "道具不足,需要消耗 " + cost.Num + " 个 " + cost.Id);
                    return false;
                }
            }
            
            //执行解锁命令

            return false;
        }
    }
}