using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class Pfb_CommonMsg : UIWindow
    {
        #region 脚本工具生成的代码
        private Button _btn_Mask;
        private Image _img_Bg;
        private TextMeshProUGUI _tmp_Title;
        private TextMeshProUGUI _tmp_Content;
        private Button _btn_Close;
        protected override void ScriptGenerator()
        {
            _btn_Mask = FindChildComponent<Button>("m_btn_Mask");
            _img_Bg = FindChildComponent<Image>("Content/m_img_Bg");
            _tmp_Title = FindChildComponent<TextMeshProUGUI>("Content/m_tmp_Title");
            _tmp_Content = FindChildComponent<TextMeshProUGUI>("Content/m_tmp_Content");
            _btn_Close = FindChildComponent<Button>("Content/m_btn_Close");
            _btn_Mask.onClick.AddListener(OnClick_MaskBtn);
            _btn_Close.onClick.AddListener(OnClick_CloseBtn);
        }
        #endregion

        protected override void OnCreate()
        {
            base.OnCreate();
            SetContent(_userDatas[0].ToString());
        }

        void SetContent(string str)
        {
            this._tmp_Content.text = str;
        }

        #region 事件
        private void OnClick_MaskBtn()
        {
            Close();
        }
        private void OnClick_CloseBtn()
        {
            Close();
        }
        #endregion

    }
}