using GameConfig;
using GameLogic.GameScripts.HotFix.GameLogic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class Pfb_MainUI : UIWindow
    {
        #region 脚本工具生成的代码
        private GameObject _goResBar;
        private TextMeshProUGUI _tmpGold;
        private TextMeshProUGUI _tmpDiamond;
        private Button _btnGetItems;
        private Button _btnCostItems;
        private TMP_InputField _tInputItemIdInput;
        private TMP_InputField _tInputItemCountInput;

        private Button _btnBag;

        private InventoryModel _inventoryModel;
        protected override void ScriptGenerator()
        {
            _goResBar = FindChild("m_goResBar").gameObject;
            _tmpGold = FindChildComponent<TextMeshProUGUI>("m_goResBar/m_tmpGold");
            _tmpDiamond = FindChildComponent<TextMeshProUGUI>("m_goResBar/m_tmpDiamond");
            _btnGetItems = FindChildComponent<Button>("m_btnGetItems");
            _btnCostItems = FindChildComponent<Button>("m_btnCostItems");
            _tInputItemIdInput = FindChildComponent<TMP_InputField>("m_tInputItemIdInput");
            _tInputItemCountInput = FindChildComponent<TMP_InputField>("m_tInputItemCountInput");
            _btnBag = FindChildComponent<Button>("m_btn_Bag");
            
            _btnBag.onClick.AddListener(() =>
            {
                GameModule.UI.ShowUI<Pfb_Bag>(EGameModule.Farmland);
            });
            _btnGetItems.onClick.AddListener(OnClickGetItemsBtn);
            _btnCostItems.onClick.AddListener(OnClickCostItemsBtn);
        }
        #endregion

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            AddUIEvent<int>(EventID.INVENTORY_GOLD_CHANGE, OnGoldChange);
            AddUIEvent<int>(EventID.INVENTORY_DIAMOND_CHANGE, OnDiamondChange);
        }

        #region 事件
        private void OnClickGetItemsBtn()
        {
            this.SendCommand(new Cmd_Inventory_GetItems_Request()
            {
                itemId = int.Parse(_tInputItemIdInput.text),
                itemCount = int.Parse(_tInputItemCountInput.text)
            });
        }
        private void OnClickCostItemsBtn()
        {
        }


        private void OnGoldChange(int gold)
        {
            this._tmpGold.text = gold.ToString();
        }
        
        private void OnDiamondChange(int diamond)
        {
            this._tmpDiamond.text = diamond.ToString();
        }

        #endregion

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
        }
    }
}
