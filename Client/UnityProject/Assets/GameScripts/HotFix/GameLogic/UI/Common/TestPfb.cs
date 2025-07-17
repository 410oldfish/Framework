using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;

namespace GameLogic
{
    [Window(UILayer.UI)]
    public class TestPfb : UIWindow
    {
        #region 脚本工具生成的代码
        private Image _imgTestImg;
        private TextMeshProUGUI _tmpTestTop;
        private TextMeshProUGUI _tmpTestVersion;
        private TextMeshProUGUI _tmpDebug;
        private Button _btnTestBtn;
        protected override void ScriptGenerator()
        {
            _imgTestImg = FindChildComponent<Image>("m_imgTestImg");
            _tmpTestTop = FindChildComponent<TextMeshProUGUI>("m_tmpTestTop");
            _tmpTestVersion = FindChildComponent<TextMeshProUGUI>("m_tmpTestVersion");
            _tmpDebug = FindChildComponent<TextMeshProUGUI>("m_tmpDebug");
            _btnTestBtn = FindChildComponent<Button>("m_btnTestBtn");
            _btnTestBtn.onClick.AddListener(OnClickTestBtnBtn);
        }
        #endregion

        #region 事件
        private void OnClickTestBtnBtn()
        {
            _tmpDebug.text = "Hello HotFix!";
        }
        #endregion

        protected override void OnCreate()
        {
            base.OnCreate();
            // Material mat = _tmpTestTop.fontMaterial;
            // mat.shader = Shader.Find(mat.shader.name);
            _tmpTestVersion.text = "1.0.3";
            Debug.Log("444");
        }
    }
}