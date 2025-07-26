using GameLogic.GameScripts.HotFix.GameLogic.Common;
using GameLogic.GameScripts.HotFix.GameLogic.Module.Login.Cmd;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class Pfb_LoginCenterServer : UIWindow, ICanSendCommand
    {
        #region 脚本工具生成的代码
        private TMP_InputField _tInputPlayerId;
        private Button _btnLogin;
        private GameObject _goSetNickName;
        private TMP_InputField _tInputNickName;
        private Button _btnSubmitNickName;
        protected override void ScriptGenerator()
        {
            _tInputPlayerId = FindChildComponent<TMP_InputField>("m_tInputPlayerId");
            _btnLogin = FindChildComponent<Button>("m_btnLogin");
            _goSetNickName = FindChild("m_goSetNickName").gameObject;
            _tInputNickName = FindChildComponent<TMP_InputField>("m_goSetNickName/Image/m_tInputNickName");
            _btnSubmitNickName = FindChildComponent<Button>("m_goSetNickName/Image/m_btnSubmitNickName");
            _btnLogin.onClick.AddListener(OnClickLoginBtn);
            _btnSubmitNickName.onClick.AddListener(OnClickSubmitNickNameBtn);
        }
        #endregion

        protected override void OnCreate()
        {
            base.OnCreate();
            _goSetNickName.SetActive(false);
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            AddUIEvent(EventID.LOGIN_SHOWSETNICKNAME, OnShowSetNickName);
        }

        #region 事件
        private void OnClickLoginBtn()
        {
            this.SendCommand(new Cmd_BuildCenterRouteRequest()
            {
                playerId = long.Parse(_tInputPlayerId.text)
            });
        }
        
        private void OnClickSubmitNickNameBtn()
        {
            this.SendCommand(new Cmd_SetNickNameRequest()
            {
                nickName = _tInputNickName.text
            });
            _goSetNickName.SetActive(false);
        }
        
        //--------------逻辑事件---------------
        private void OnShowSetNickName()
        {
            _goSetNickName.SetActive(true);
        }
        
        #endregion

        public IArchitecture GetArchitecture()
        {
            return GameMgr.Interface;
        }
    }
}