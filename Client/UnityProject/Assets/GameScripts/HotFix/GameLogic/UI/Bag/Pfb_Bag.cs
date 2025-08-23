using System.Collections.Generic;
using Fantasy;
using GameConfig;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;
using SuperScrollView;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class Pfb_Bag : UIWindow
    {
        #region 脚本工具生成的代码
        private Button _btn_Close;
        private TextMeshProUGUI _tmp_Title;
        private ScrollRect _scroll_GridScrollView;
        private LoopGridView _loopGridView;
        private RectTransform _rect_Tabs;
        
        private InventoryModel _inventoryModel;

        private Dictionary<EItemType, List<ItemProto>> _itemTypeListMap = new Dictionary<EItemType, List<ItemProto>>();
        private EGameModule _gameModule;
        private EItemType _currentTab;
        List<ItemProto> _currentItemList => _itemTypeListMap[_currentTab];
        protected override void ScriptGenerator()
        {
            _rect_Tabs = FindChildComponent<RectTransform>("m_rect_Tabs");
            _btn_Close = FindChildComponent<Button>("m_btn_Close");
            _tmp_Title = FindChildComponent<TextMeshProUGUI>("m_tmp_Title");
            _scroll_GridScrollView = FindChildComponent<ScrollRect>("m_scroll_GridScrollView");
            _loopGridView = FindChildComponent<LoopGridView>("m_scroll_GridScrollView");
            _btn_Close.onClick.AddListener(OnClick_CloseBtn);
        }
        #endregion

        #region 事件
        private void OnClick_CloseBtn()
        {
            GameModule.UI.CloseUI<Pfb_Bag>();
        }
        #endregion

        protected override void OnCreate()
        {
            base.OnCreate();
            _gameModule = (EGameModule)_userDatas[0];
            _inventoryModel = this.GetModel<InventoryModel>();
            var showTabs = Misc.GetBagTabsByGameModule(_gameModule);
            if (showTabs.Count == 0)
            {
                Log.Error("No tabs to show in bag for game module: " + _gameModule);
                return;
            }
            CreateTabs(showTabs);
            _currentTab = showTabs[0];
            for (int i = 0; i < showTabs.Count; i++)
            {
                var curTab = showTabs[i];
                _itemTypeListMap.Add(curTab, _inventoryModel.GetItemsByType(curTab));
            }

            int itemCount = _currentItemList.Count;
            _loopGridView.InitGridView(itemCount, OnCreateGridScrollViewItem);
        }

        private LoopGridViewItem OnCreateGridScrollViewItem(LoopGridView gridView, int index, int row, int col)
        {
            ItemProto itemData = _currentItemList[index];
            if(itemData == null)
            {
                return null;
            }

            LoopGridViewItem item = gridView.NewListViewItem("Pfb_BagItem");
            Pfb_BagItem bagItem = item.GetComponent<Pfb_BagItem>();
            bagItem.Init(itemData);
            return item;
        }

        async void CreateTabs(List<EItemType> tabs)
        {
            foreach (var tab in tabs)
            {
                var tabWidget = await CreateWidgetByPathAsync<Pfb_BagTabItem>(_rect_Tabs,"Pfb_BagTabItem");
                tabWidget.Init(tab);
                tabWidget.TabAddListener(OnChangeTab);
            }
        }

        void OnChangeTab(EItemType tab)
        {
            _currentTab = tab;
            _loopGridView.SetListItemCount(_currentItemList.Count);
            _loopGridView.RefreshAllShownItem();
        }
    }
}