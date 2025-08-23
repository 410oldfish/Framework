using GameConfig;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class Pfb_BagTabItem : UIWidget
    {
        #region 脚本工具生成的代码
        private Button _btn_Tab;
        private EItemType _itemType;
        private TextMeshProUGUI _tmp_TabName;
        protected override void ScriptGenerator()
        {
            _btn_Tab = FindChildComponent<Button>("m_btn_Tab");
            _tmp_TabName = FindChildComponent<TextMeshProUGUI>("m_tmp_TabName");
        }
        #endregion
        
        public void Init(EItemType itemType)
        {
            _itemType = itemType;
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            _tmp_TabName.text = Misc.GetItemTypeName(_itemType);
        }

        public void TabAddListener(UnityEngine.Events.UnityAction<EItemType> action)
        {
            _btn_Tab.onClick.AddListener(() =>
            {
                action?.Invoke(_itemType);
            });
        }

    }
}