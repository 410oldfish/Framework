// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;
using MergeIt.Game.UI.InventoryPanel;
using MergeIt.Game.Windows.Inventory;
using TEngine;
using UnityEngine;
using YooAsset;

namespace MergeIt.Game.Factories.Inventory
{
    public class InventoryFactory : IInventoryFactory
    {
        private const string PanelItemPath = "InventoryPanelItem";
        private const string WindowItemPath = "InventoryWindowItem";
        private const string WindowEmptyItemPath = "InventoryWindowEmptyCell";
        private const string WindowPaidCellPath = "InventoryWindowPaidCell";

        public async UniTask<InventoryPanelItemComponent> CreateInventoryPanelItem(IFieldElement fieldElement)
        {
            var itemPanelObject = await GameModule.Resource.LoadAssetAsync<GameObject>(PanelItemPath);

            if (itemPanelObject)
            {
                GameObject panelItemObject = Object.Instantiate(itemPanelObject);
                FieldElementIconComponent iconPrototype = fieldElement.ConfigParameters.ElementConfig.GetIconComponent();
                
                var icon = Object.Instantiate(iconPrototype, panelItemObject.transform);
                var iconRectTransform = icon.GetComponent<RectTransform>();
                iconRectTransform.SetAsFirstSibling();
                
                var resultComponent = panelItemObject.GetComponent<InventoryPanelItemComponent>();
                return resultComponent;
            }

            return null;
        }
        
        public async UniTask<InventoryWindowItemComponent> CreateInventoryWindowItem(IFieldElement fieldElement)
        {
            
            var itemPanelObject = await GameModule.Resource.LoadAssetAsync<GameObject>(WindowItemPath);

            if (itemPanelObject)
            {
                var panelItemObject = Object.Instantiate(itemPanelObject);
                if (panelItemObject.TryGetComponent(out InventoryWindowItemComponent resultComponent))
                {
                    FieldElementIconComponent iconPrototype = fieldElement.ConfigParameters.ElementConfig.GetIconComponent();
                
                    var icon = Object.Instantiate(iconPrototype, panelItemObject.transform);
                    var iconRectTransform = icon.GetComponent<RectTransform>();
                    
                    resultComponent.SetIcon(iconRectTransform);
                }
                
                return resultComponent;
            }

            return null;
        }
        
        public async UniTask<InventoryWindowPaidCellComponent> CreateWindowPaidCell()
        {
            var itemPanelObject = await GameModule.Resource.LoadAssetAsync<GameObject>(WindowPaidCellPath);

            if (itemPanelObject)
            {
                var panelItemObject = Object.Instantiate(itemPanelObject);
                
                var resultComponent = panelItemObject.GetComponent<InventoryWindowPaidCellComponent>();
                return resultComponent;
            }

            return null;
        }
        
        public async UniTask<GameObject> CreateWindowEmptyCell()
        {
            var itemPanelObject = await GameModule.Resource.LoadAssetAsync<GameObject>(WindowEmptyItemPath);
            if (itemPanelObject)
            {
                var panelItemObject = Object.Instantiate(itemPanelObject);
                return panelItemObject.gameObject;
            }

            return null;
        }
    }

}