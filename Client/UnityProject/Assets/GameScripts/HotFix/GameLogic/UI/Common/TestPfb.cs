using Fantasy;
using Fantasy.Async;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;
using Log = TEngine.Log;

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
        private Button _btnCreateRouteBtn;
        private Button _btnSendRouteBtn;
        private Button _btnCallRouteBtn;
        private Button _btnRegisterAddressableBtn;
        private Button _btnSendAddressableBtn;
        private Button _btnCallAddressableBtn;
        private Button _btnTransMapBtn;
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
            _btnCreateRouteBtn = FindChildComponent<Button>("m_btnCreateRouteBtn");
            _btnSendRouteBtn = FindChildComponent<Button>("m_btnSendRouteBtn");
            _btnCallRouteBtn = FindChildComponent<Button>("m_btnCallRouteBtn");
            _btnRegisterAddressableBtn = FindChildComponent<Button>("m_btnRegisterAddressableBtn");
            _btnSendAddressableBtn = FindChildComponent<Button>("m_btnSendAddressableBtn");
            _btnCallAddressableBtn = FindChildComponent<Button>("m_btnCallAddressableBtn");
            _btnTransMapBtn = FindChildComponent<Button>("m_btnTransMapBtn");
            
            _btnConnectBtn.onClick.AddListener(OnClickConnectBtnBtn);
            _btnSendBtn.onClick.AddListener(OnClickSendBtnBtn);
            _btnRPCBtn.onClick.AddListener(OnClickRPCBtnBtn);
            _btnDisposeBtn.onClick.AddListener(OnClickDisposeBtnBtn);
            _btnCreateRouteBtn.onClick.AddListener(OnClickCreateRouteBtn);
            _btnSendRouteBtn.onClick.AddListener(OnClickSendRouteBtn);
            _btnCallRouteBtn.onClick.AddListener(OnClickCallRouteBtn);
            _btnRegisterAddressableBtn.onClick.AddListener(OnClickRegisterAddressableBtn);
            _btnSendAddressableBtn.onClick.AddListener(OnClickSendAddressableBtn);
            _btnCallAddressableBtn.onClick.AddListener(OnClickCallAddressableBtn);
            _btnTransMapBtn.onClick.AddListener(OnClickTransMapBtn);
        }
        
        #endregion

        #region 事件
        private void OnClickConnectBtnBtn()
        {
            GameModule.Network.SessionConnect();
        }
        private void OnClickSendBtnBtn()
        {
            GameModule.Network.Send(new C2G_HelloFantasy(){Tag = "Hello Fantasy"});
        }
        
        private void OnClickRPCBtnBtn()
        {
            RPCTest();
        }
        
        private void OnClickDisposeBtnBtn()
        {
            GameModule.Network.Dispose();
        }
        
        private void OnClickCreateRouteBtn()
        {
            GameModule.Network.CreateRoute<C2G_ConnectChatRequest, G2C_ConnectChatResponse>(new C2G_ConnectChatRequest());
        }
        
        private void OnClickSendRouteBtn()
        {
            GameModule.Network.SendRoute();
        }
        
        private void OnClickCallRouteBtn()
        {
            CustomRouteRPCTest();
        }
        
        private void OnClickRegisterAddressableBtn()
        {
            GameModule.Network.RPC<C2G_ConnectAddressableRequest, G2C_ConnectAddressableResponse>(new C2G_ConnectAddressableRequest());
            Log.Debug("注册Addressable成功");
        }
        
        private void OnClickSendAddressableBtn()
        {
            GameModule.Network.Send(new C2M_HelloMsg(){Tag = "Hello Addressable Message"});
        }
        
        private void OnClickCallAddressableBtn()
        {
            AddressableRPCTest();
        }

        private void OnClickTransMapBtn()
        {
            GameModule.Network.RPC<C2M_MoveToMapRequest, M2C_MoveToMapResponse>(new C2M_MoveToMapRequest());
        }

        async FTask AddressableRPCTest()
        {
            var ret = await GameModule.Network.RPC<C2M_HelloRequest, M2C_HelloResponse>(new C2M_HelloRequest()
            {
                Tag = "Hello Addressable Request"
            });
            Log.Debug( "get message from addressable : " + ret.Tag);
        }

        async FTask RPCTest()
        {
            var rep = await GameModule.Network.RPC<C2G_HelloRequest, G2C_HelloResponse>(new C2G_HelloRequest(){Tag = "Fish"});
            Debugger.print("get message from server : " + rep.Tag);
        }

        async FTask CustomRouteRPCTest()
        {
            var ret = await GameModule.Network.CallRoute<C2Chat_HelloRouteRequest, Chat2C_HelloRouteResponse>(new C2Chat_HelloRouteRequest()
            {
                Tag = "Hello Custom Route Request"
            });
            
            Debugger.print("get message from chat route : " + ret.Tag);
        }

        #endregion

    }
}