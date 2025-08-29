using System.Collections.Generic;
using Fantasy.Helper;
using GameConfig;
using GameConfig.farm;
using QFramework;
using TEngine;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D;

namespace GameLogic
{
    //农场的控制器
    public class FarmlandSystem : AbstractSystem, ICanSendCommand
    {
        private FarmlandModel _farmlandModel;
        private InventoryModel _inventoryModel;
        
        SpriteAtlas _landtypesAltas;
        GameObject _farmlandSceneObj;
        protected override void OnInit()
        {
            _farmlandModel = this.GetModel<FarmlandModel>();
            _inventoryModel = this.GetModel<InventoryModel>();
        }
        
        
        public async void OpenModule()
        {
            _landtypesAltas = await GameModule.Resource.LoadAssetAsync<SpriteAtlas>("atlas_landtypes");
            GameModule.UI.ShowUI<Pfb_Farmland_UI>();
            _farmlandSceneObj = await GameModule.Resource.LoadGameObjectAsync("Pfb_Farmland_Scene");
        }

        public async void CloseModule()
        {
            GameModule.Resource.UnloadAsset(_landtypesAltas);
            _landtypesAltas = null;
            GameModule.UI.CloseUI<Pfb_Farmland_UI>();
            GameObject.Destroy(_farmlandSceneObj);
            _farmlandSceneObj = null;
        }

        //资源----------------------------------------------------
        public Sprite GetSpriteFromAtlas(string subSpriteName)
        {
            return _landtypesAltas.GetSprite(subSpriteName);
        }

        //行为----------------------------------------------------


        public void GetFarmlandAllData()
        {
            this.SendCommand(new Cmd_Farmland_GetFarmlandDataRequest());
        }

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
            var costArr = unlockCfg.Cost;
            if (!_inventoryModel.CheckItemsCount(costArr))
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, "道具不足");
                return false;
            }
            
            //执行解锁命令
            this.SendCommand(new Cmd_Farmland_UnlockBlock_Req() {LandId = farmlandId});

            return false;
        }

        /// <summary>
        /// 完成解锁
        /// </summary>
        /// <param name="landId"></param>
        public void FinishUnlockLand(int landId)
        {
            var landData = _farmlandModel.LandDataDic[landId];
            if (landData.LandType != ELandType.Unlocking)
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, "土地未处于解锁中");
                return;
            }
            
            this.SendCommand(new Cmd_Farmland_FinishUnlockLandRequest() {LandId = landId});
        }
        
        /// <summary>
        /// 改变农田类型
        /// </summary>
        /// <param name="landId"></param>
        /// <param name="targetLandType"></param>
        public void ChangeLandType(int landId, ELandType targetLandType)
        {
            if (!_farmlandModel.CanLandOperate(landId))
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, "土地无法操作");
                return;
            }

            var landTypeConfig = ConfigHelper.FarmlandTypeConfig.Get(targetLandType);
            var costItems = landTypeConfig.Cost;

            if (!_inventoryModel.CheckItemsCount(costItems))
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, StringDefine.NotEnoughItem(costItems));
                return;
            }

            this.SendCommand(new Cmd_Farmland_ChangeLandType());
        }

        
        /// <summary>
        /// 种植
        /// </summary>
        /// <param name="landId"></param>
        /// <param name="seedId"></param>
        public void Seed(int landId, int seedId)
        {
            if (!_farmlandModel.CanLandOperate(landId))
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, "土地无法操作");
                return;
            }

            if (!_inventoryModel.CheckItemCountOne(seedId))
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, StringDefine.NotEnoughItem(landId, 1));
                return;
            }
        }

        /// <summary>
        ///收获指定地块
        /// </summary>
        /// <param name="landId"></param>
        public void Gain(List<int> landIds)
        {
            foreach (var landId in landIds)
            {
                if (!_farmlandModel.CanGain(landId))
                {
                    GameEvent.Send(EventID.UI_COMMON_MSG, StringDefine.NotArriveHarvestTime(landId));
                    return;
                }
            }

            this.SendCommand(new Cmd_Farmland_Gain_Req() { LandId = landIds });
        }

        /// <summary>
        /// 浇水
        /// </summary>
        /// <param name="landId"></param>
        public void Water(int landId)
        {
            if (!_farmlandModel.CanWater(landId))
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, "土地无法浇水");
                return;
            }

            this.SendCommand(new Cmd_Farmland_Water_Request() { LandId = landId });
        }

        /// <summary>
        /// 除虫
        /// </summary>
        /// <param name="landId"></param>
        /// <param name="dePesterId"></param>
        public void Depest(int landId, int dePesterId)
        {
            if (!_farmlandModel.CanDepest(landId))
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, "土地无法除虫");
                return;
            }

            if (!_inventoryModel.CheckItemCountOne(dePesterId))
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, StringDefine.NotEnoughItem(dePesterId, 1));
                return;
            }

            this.SendCommand(new Cmd_Farmland_Depest_Request() { LandId = landId, DePesterId = dePesterId });
        }
        
        /// <summary>
        /// 施肥
        /// </summary>
        /// <param name="landId"></param>
        /// <param name="fertilizerId"></param>
        public void Fertilize(int landId, int fertilizerId)
        {
            if (!_farmlandModel.CanFertilize(landId))
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, "土地无法施肥");
                return;
            }

            if (!_inventoryModel.CheckItemCountOne(fertilizerId))
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, StringDefine.NotEnoughItem(fertilizerId, 1));
                return;
            }

            this.SendCommand(new Cmd_Farmland_Fertilize_Req() { landId = landId, fertilizerId = fertilizerId });
        }
        
        /// <summary>
        /// 移除作物
        /// </summary>
        /// <param name="landId"></param>
        public void RemoveCrop(int landId)
        {
            if (!_farmlandModel.CanRemoveCrop(landId))
            {
                GameEvent.Send(EventID.UI_COMMON_MSG, "农作物无法移除");
                return;
            }

            this.SendCommand(new Cmd_Farmland_RemoveCrop_Req() { LandId = landId });
        }

    }
}