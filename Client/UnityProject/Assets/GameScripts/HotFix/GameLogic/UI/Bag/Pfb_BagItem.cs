using System;
using Cysharp.Threading.Tasks;
using Fantasy;
using GameConfig;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class Pfb_BagItem : MonoBehaviour
    {
        #region 脚本工具生成的代码
        private Button _btn_Item;
        private TextMeshProUGUI _tmp_Name;

        private int _itemId;
        private int _itemCount;
        private EItemType _itemType;

        private bool _bind = false;
        
        void BindProperty()
        {
            if (_bind) return;
            _bind = true;

            _btn_Item = this.transform.GetComponentInTargetChild<Button>("m_btn_Item");
            _tmp_Name = this.transform.GetComponentInTargetChild<TextMeshProUGUI>("m_tmp_Name");
            _btn_Item.onClick.AddListener(OnClick_ItemBtn);
        }

        #endregion

        #region 事件
        private void OnClick_ItemBtn()
        {
            ShowDetails();
        }
        #endregion

        public void Init(ItemProto itemData)
        {
            _itemId = itemData.Id;
            _itemCount = itemData.Count;
            _itemType = Misc.GetItemTypeById(_itemId);
            
            BindProperty();
            SetItem().Forget();
        }

        async UniTask SetItem()
        {
            var itemCfg = Misc.GetItemConfig(_itemId);
            _tmp_Name.text = itemCfg.Name;
            string iconPath = Misc.GetBagItemIcon(_itemId);
            _btn_Item.GetComponent<Image>().sprite = await GameModule.Resource.LoadAssetAsync<Sprite>(iconPath);
        }

        void ShowDetails()
        {
            
        }

    }
}