using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using UnityEngine.Events;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class Pfb_Farmland_UI : UIWindow
    {
        private FarmlandSystem _farmlandSystem;
        
        #region 脚本工具生成的代码
        private Button _btn_RemoveLand;
        private Button _btn_Bag;
        private Button _btn_Water;
        private Button _btn_Depest;
        private Button _btn_Gain;
        private Button _btn_GainAll;

        private GameObject _go_TimerRoot;
        
        //解锁计时器widget
        Dictionary<int, Pfb_TimerItem> _timerItems = new Dictionary<int, Pfb_TimerItem>();
        //浇水计时器ID
        Dictionary<int, int> _waterTimerId = new Dictionary<int, int>();
        //除虫计时器ID
        Dictionary<int, int> _depestTimerId = new Dictionary<int, int>();
        
        
        //Test-------------------------------
        private Button _btn_GetAllData;
        protected override void ScriptGenerator()
        {
            _btn_RemoveLand = FindChildComponent<Button>("Bottom_btns/m_btn_RemoveLand");
            _btn_Bag = FindChildComponent<Button>("Bottom_btns/m_btn_Bag");
            _btn_Water = FindChildComponent<Button>("Bottom_btns/m_btn_Water");
            _btn_Depest = FindChildComponent<Button>("Bottom_btns/m_btn_Depest");
            _btn_Gain = FindChildComponent<Button>("Bottom_btns/m_btn_Gain");
            _btn_GainAll = FindChildComponent<Button>("Bottom_btns/m_btn_GainAll");
            _go_TimerRoot = FindChild("m_go_TimerRoot").gameObject;
            _btn_RemoveLand.onClick.AddListener(OnClick_RemoveLandBtn);
            _btn_Bag.onClick.AddListener(OnClick_BagBtn);
            _btn_Water.onClick.AddListener(OnClick_WaterBtn);
            _btn_Depest.onClick.AddListener(OnClick_DepestBtn);
            _btn_Gain.onClick.AddListener(OnClick_GainBtn);
            _btn_GainAll.onClick.AddListener(OnClick_GainAllBtn);
            
            //----Test--------------------------
            _btn_GetAllData = FindChildComponent<Button>("m_btn_GetAllData");
            _btn_GetAllData.onClick.AddListener(GetFarmlandAllData);
        }
        #endregion

        protected override void OnCreate()
        {
            base.OnCreate();
            _farmlandSystem = this.GetSystem<FarmlandSystem>();
        }

        #region 事件
        private void OnClick_RemoveLandBtn()
        {
        }
        private void OnClick_BagBtn()
        {
            GameModule.UI.ShowUI<Pfb_Bag>();
        }
        private void OnClick_WaterBtn()
        {
        }
        private void OnClick_DepestBtn()
        {
        }
        private void OnClick_GainBtn()
        {
        }
        private void OnClick_GainAllBtn()
        {
        }
        #endregion

        
        //Event
        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            //解锁
            AddUIEvent<int,long, Vector2, UnityAction>(EventID.FARMLAND_LAND_UNLOCK_TIMER, OnShowUnlockLandTimerItem);
            AddUIEvent<int>(EventID.FARMLAND_LAND_UNLOCK_FINISH, OnFinishUnlockLandTimerItem);
            //浇水
            AddUIEvent<int,long, Vector2, UnityAction>(EventID.FARMLAND_LAND_WATER_TIMER, OnCreateWaterTimer);
            AddUIEvent<int>(EventID.FARMLAND_LAND_WATER_FINISH, OnFinishWater);
            //除虫
            AddUIEvent<int,long, Vector2, UnityAction<int>>(EventID.FARMLAND_LAND_DEPEST_TIMER, OnCreateDepestTimer);
            AddUIEvent<int>(EventID.FARMLAND_LAND_WATER_FINISH, OnFinishDepest);
        }

        private void OnFinishUnlockLandTimerItem(int landId)
        {
            if(_timerItems.TryGetValue(landId, out var timerItem))
            {
                timerItem.Destroy();
                _timerItems.Remove(landId);
            }
        }

        void OnShowUnlockLandTimerItem(int landId,long leftTime, Vector2 screenPos, UnityAction callback)
        {
            InitializeTimerItem(landId,leftTime, screenPos, callback).Forget();
        }

        async UniTask<Pfb_TimerItem> InitializeTimerItem(int landId, long leftTime, Vector2 screenPos, UnityAction callback)
        {
            var timerItem = await CreateWidgetByPathAsync<Pfb_TimerItem>(_go_TimerRoot.transform,"Pfb_TimerItem");
            timerItem.Init(leftTime, screenPos, callback);
            _timerItems.Add(landId, timerItem); 
            return timerItem;
        }

        /// <summary>
        /// 接收到场景事件，在一定时间后出现浇水按钮
        /// </summary>
        /// <param name="landId"></param>
        /// <param name="leftTime"></param>
        /// <param name="screenPos"></param>
        /// <param name="callback"></param>
        void OnCreateWaterTimer(int landId,long leftTime, Vector2 screenPos, UnityAction callback)
        {
            
        }

        void OnFinishWater(int landId)
        {
        }

        void OnCreateDepestTimer(int landId, long leftTime, Vector2 screenPos, UnityAction<int> callback)
        {
        }
        
        void OnFinishDepest(int landId)
        {

        }
        
        //---------Operation-----------------------
        void GetFarmlandAllData()
        {
            _farmlandSystem.GetFarmlandAllData();
        }
    }
}