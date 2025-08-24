using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;
using UnityEngine.Events;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class Pfb_TimerItem : UIWidget
    {
        #region 脚本工具生成的代码
        private Image _img_Progress;
        private TextMeshProUGUI _tmp_LeftTime;
        private Button _btn_Finish;
        private int _totalTime = 0; // 总时间
        private int _leftTime = 0; // 剩余时间
        
        private int _updateTimerId = 0; // 更新计时器ID
        private int _updateInterval = 1; // 更新间隔时间，单位为秒
        protected override void ScriptGenerator()
        {
            _img_Progress = FindChildComponent<Image>("m_img_Progress");
            _tmp_LeftTime = FindChildComponent<TextMeshProUGUI>("m_tmp_LeftTime");
            _btn_Finish = FindChildComponent<Button>("m_btn_Finish");
        }
        #endregion

        public void Init(long leftTime, Vector2 screenPos, UnityAction callback)
        {
            _totalTime = (int)leftTime;
            _leftTime = _totalTime;
            this.transform.position = screenPos;
            if (callback != null)
            {
                _btn_Finish.onClick.AddListener(callback);
            }
            if (_totalTime <= 0)
            {
                TimerFinish();
                return;
            }
            _updateTimerId = GameModule.Timer.AddTimer(OnUpdateTimer, _updateInterval, true);
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            _btn_Finish.gameObject.SetActive(false);
        }

        void OnUpdateTimer(object[] args)
        {
            _leftTime -= _updateInterval;
            if (_leftTime <= 0)
            {
                StopTimer();
                TimerFinish();
                return;
            }

            float progress = 1f - (float)_leftTime / _totalTime;
            _img_Progress.fillAmount = progress;
            _tmp_LeftTime.text = Misc.SecondToTimeString(_leftTime);
        }

        void TimerFinish()
        {
            _img_Progress.gameObject.SetActive(false);
            _tmp_LeftTime.gameObject.SetActive(false);
            _btn_Finish.gameObject.SetActive(true);
        }

        void StopTimer()
        {
            GameModule.Timer.RemoveTimer(_updateTimerId);
            _updateTimerId = 0;
        }

        public void OnDestroy()
        {
            StopTimer();
        }
    }
}