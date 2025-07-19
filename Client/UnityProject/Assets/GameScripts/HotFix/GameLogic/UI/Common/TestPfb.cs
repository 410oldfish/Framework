using Fantasy;
using Fantasy.Async;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class TestPfb : UIWindow
    {
        #region 脚本工具生成的代码
        private Image _imgTestImg;
        private TextMeshProUGUI _tmpTestTop;
        private TextMeshProUGUI _tmpTestVersion;
        private TextMeshProUGUI _tmpDebug;
        private Button _btnConnectBtn;
        private Button _btnSendBtn;
        private Button _btnRPCBtn;
        private Button _btnDisposeBtn;
        protected override void ScriptGenerator()
        {
            _imgTestImg = FindChildComponent<Image>("m_imgTestImg");
            _tmpTestTop = FindChildComponent<TextMeshProUGUI>("m_tmpTestTop");
            _tmpTestVersion = FindChildComponent<TextMeshProUGUI>("m_tmpTestVersion");
            _tmpDebug = FindChildComponent<TextMeshProUGUI>("m_tmpDebug");
            _btnConnectBtn = FindChildComponent<Button>("m_btnConnectBtn");
            _btnSendBtn = FindChildComponent<Button>("m_btnSendBtn");
            _btnRPCBtn = FindChildComponent<Button>("m_btnRPCBtn");
            _btnDisposeBtn = FindChildComponent<Button>("m_btnDisposeBtn");
            _btnConnectBtn.onClick.AddListener(OnClickConnectBtnBtn);
            _btnSendBtn.onClick.AddListener(OnClickSendBtnBtn);
            _btnRPCBtn.onClick.AddListener(OnClickRPCBtnBtn);
            _btnDisposeBtn.onClick.AddListener(OnClickDisposeBtnBtn);
        }
        
        #endregion

        #region 事件
        private void OnClickConnectBtnBtn()
        {
            GameModule.Network.SessionConnect();
        }
        private void OnClickSendBtnBtn()
        {
            GameModule.Network.Send();
        }
        
        private void OnClickRPCBtnBtn()
        {
            RPCTest();
        }
        
        private void OnClickDisposeBtnBtn()
        {
            GameModule.Network.Dispose();
        }

        async FTask RPCTest()
        {
            var rep = await GameModule.Network.RPC<C2G_HelloRequest, G2C_HelloResponse>(new C2G_HelloRequest(){Tag = "Fish"});
            Debugger.print("get message from server : " + rep.Tag);
        }

        #endregion

    }
}