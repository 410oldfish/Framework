using System;
using Cysharp.Threading.Tasks;
using Fantasy.Helper;
using GameConfig.farm;
using QFramework;
using TEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

namespace GameLogic
{
    enum ESeedStage
    {
        Sprout = 1, //发芽
        Mature = 2, //成长
        Harvest = 3, //收获
    }
    
    public class FarmlandBlockView : MonoBehaviour, ICanSendCommand, ICanGetSystem
    {
        private int _landId;
        public int LandId
        {
            get => _landId;
            set => _landId = value;
        }
        
        private bool _active = false;
        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        SpriteRenderer _landSpriteRenderer;
        SpriteRenderer _cropSpriteRenderer;
        
        private int _unlockTimerId; //解锁
        private int _cropTimerId; //农作物外型
        private int _waterTimerId; //浇水
        private int _depestTimerId; //除虫

        private EventTrigger _eventTrigger;
        private EventTrigger.Entry _clickEntry = new EventTrigger.Entry(){eventID = EventTriggerType.PointerClick};
        private UnityAction<BaseEventData> _onClickLandAction;
        
        private FarmlandSystem _farmlandSystem;
        private void Start()
        {
            _farmlandSystem = this.GetSystem<FarmlandSystem>();
            _landSpriteRenderer = transform.GetComponentInTargetChild<SpriteRenderer>("Land");
            _cropSpriteRenderer = transform.GetComponentInTargetChild<SpriteRenderer>("Crop");
            _eventTrigger = gameObject.GetComponent<EventTrigger>();
            _eventTrigger.triggers.Clear();
            _clickEntry.callback.AddListener(OnClickProxy);
            _eventTrigger.triggers.Add(_clickEntry);
        }

        void OnClickProxy(BaseEventData data)
        {
            _onClickLandAction?.Invoke(data);
        }

        public void ChangeLandType(ELandType landType)
        {
            //土地类型变更
            SetLandImg(landType).Forget();
        }

        public async UniTask SetLockedBlock(int landId)
        {
            _landId = landId;
            _onClickLandAction = (e) =>
            {
                _farmlandSystem.UnlockNewFarmland(_landId);
            };
        }

        public async UniTask SetBlock(int landId, Farm_LandData data)
        {
            _landId = landId;
            SetLand(data);
            if(data.LandType == ELandType.Unlocking || data.LandType == ELandType.Lock) //未解锁
            {
                return; //不需要设置作物
            }
            SetCrop(data);
        }

        void SetLand(Farm_LandData data)
        {
            SetLandImg(data.LandType).Forget();
            if (data.LandType == ELandType.Unlocking) //处于解锁中状态
            {
                _cropSpriteRenderer.gameObject.SetActive(false);
                
                var startUnlockTime = data.LandUnlockTime;
                var needTime = ConfigHelper.FarmlandUnlockConfig.Get(_landId).Time;
                var endUnlockTime = startUnlockTime + needTime;
                var unlockNeedTime = endUnlockTime - TimeHelper.Now;
                var screenPos = Misc.GetScreenPositionByWorldPosition(transform.position);
                if (unlockNeedTime > 0)
                {
                    _unlockTimerId = GameModule.Timer.AddTimer(OnUnlockTimerFinished, unlockNeedTime, false);
                }
                GameEvent.Send(EventID.FARMLAND_LAND_UNLOCK_TIMER, _landId,unlockNeedTime, screenPos, (UnityAction)OnClickFinishUnlockLand);
            }
        }

        void SetCrop(Farm_LandData data)
        {
            if (data.SeedId <= 0)
            {
                return;
            }
            
            //有作物
            //-----------农作物状态-------------
            ESeedStage stageIndex = ESeedStage.Sprout;
            var seedConfig = ConfigHelper.SeedConfig.Get(data.SeedId);
            var harvestNeedTime = seedConfig.GrowTime;
            var firstStageTime = seedConfig.StageTime;
            var startTime = data.StartTime; //种植时间
            var harvestTime = data.HarvestTime; //收获时间
            if (TimeHelper.Now >= harvestTime) //已经可以收获
            {
                stageIndex = ESeedStage.Harvest;
            }
            else if(TimeHelper.Now >= startTime + firstStageTime) //已经过了第一阶段
            {
                stageIndex = ESeedStage.Mature;
            }
            else //还在发芽阶段
            {
                stageIndex = ESeedStage.Sprout;
            }

            SetCropImg(data.SeedId, stageIndex).Forget();

            long timerNeedTime = 0;
            if (stageIndex == ESeedStage.Sprout) //幼苗->成长
            {
                timerNeedTime = startTime + firstStageTime - TimeHelper.Now;
            }
            else if(stageIndex == ESeedStage.Mature) //成长->收获
            {
                timerNeedTime = harvestTime - TimeHelper.Now;
            }

            if (timerNeedTime > 0)
            {
                StartCropTimer(data.SeedId, stageIndex, timerNeedTime, harvestTime);
            }
            
            //-----------农作物交互-------------
            var nextWaterTime = data.NextWaterTime; //下次浇水时间
            var waterLeftTime = nextWaterTime - TimeHelper.Now;
            var position = transform.position;
            GameEvent.Send<int, long, Vector2, UnityAction>(EventID.FARMLAND_LAND_WATER_TIMER, _landId, waterLeftTime, 
                Misc.GetScreenPositionByWorldPosition(position), OnClickWater);
            
            var nextPestTime = data.NextPestTime; //下次除虫时间
            var depestLeftTime = nextPestTime - TimeHelper.Now;
            GameEvent.Send<int, long, Vector2, UnityAction<int>>(EventID.FARMLAND_LAND_DEPEST_TIMER, _landId, depestLeftTime, 
                Misc.GetScreenPositionByWorldPosition(position), OnClickDepest);
        }

        void StartCropTimer(int seedId, ESeedStage curStage, long needTime, long harvestTime = 0)
        {
            if(_cropTimerId != 0)
            {
                //如果之前有定时器，先移除
                Log.Error( "StartCropTimer: Remove old crop timer, seedId: {0}, curStage: {1}", seedId, curStage);
                GameModule.Timer.RemoveTimer(_cropTimerId);
            }
            _cropTimerId = GameModule.Timer.AddTimer(OnCropTimerFinished, needTime, false, false,seedId, curStage, harvestTime);
        }

        void OnUnlockTimerFinished(object[] args)
        {
            //解锁所需时间已到
            GameModule.Timer.RemoveTimer(_unlockTimerId);
            _unlockTimerId = 0;
        }
        
        void OnCropTimerFinished(object[] args)
        {
            //作物生长阶段已到
            GameModule.Timer.RemoveTimer(_cropTimerId);
            _cropTimerId = 0;
            
            var seedId = (int)args[0];
            var oldStage = (ESeedStage)args[1];
            ESeedStage newStage = oldStage + 1;
            
            //刷新作物外观
            SetCropImg(seedId, newStage).Forget();

            if (newStage == ESeedStage.Mature)
            {
                var harvestTime = (long)args[2];
                var timerNeedTime = harvestTime - TimeHelper.Now;
                if (timerNeedTime > 0)
                {
                    //成熟->收获
                    StartCropTimer(seedId, newStage, timerNeedTime, harvestTime);
                }
            }
        }

        //显示解锁倒计时
        void OnClickFinishUnlockLand()
        {
            this.SendCommand(new Cmd_Farmland_FinishUnlockLandRequest
            {
                LandId = _landId
            });
        }

        void OnClickWater()
        {
            //浇水
            this.SendCommand(new Cmd_Farmland_Water_Request()
            {
                LandId = _landId
            });
        }
        
        void OnClickDepest(int depestId)
        {
            //除虫
            this.SendCommand(new Cmd_Farmland_Depest_Request()
            {
                LandId = _landId,
                DePesterId = depestId
            });
        }

        async UniTask SetCropImg(int seedId, ESeedStage stage)
        {
            string cropImgPath = Misc.GetSeedImgPath(seedId, (int)stage);
            _cropSpriteRenderer.sprite = await GameModule.Resource.LoadAssetAsync<Sprite>(cropImgPath);
            _cropSpriteRenderer.gameObject.SetActive(true);
        }

        async UniTask SetLandImg(ELandType landType)
        {
            string landTypeImgPath = Misc.GetLandTypeImgPath(landType);
            _landSpriteRenderer.sprite = _farmlandSystem.GetSpriteFromAtlas(landTypeImgPath);
        }

        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }
    }
}