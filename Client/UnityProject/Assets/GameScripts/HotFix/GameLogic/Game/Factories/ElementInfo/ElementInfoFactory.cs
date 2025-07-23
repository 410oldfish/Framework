// Copyright (c) 2024, Awessets

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MergeIt.Core.Configs.Elements;
using MergeIt.Game.Windows.ElementInfo;
using TEngine;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

namespace MergeIt.Game.Factories.ElementInfo
{
    public class ElementInfoFactory : IElementInfoFactory
    {
        private static Dictionary<ElementInfoType, string> PrefabsLocations = new()
        {
            {ElementInfoType.InfoWindow, "ElementInfoWindowItem"},
            {ElementInfoType.UserProgressWindow, "Prefabs/Windows/UserProgress/UserProgressWindowItem"}
        };
        
        public async UniTask<ElementInfoItemComponent> CreateElementWindowItem(ElementConfig elementConfig,
            ElementInfoType infoType = ElementInfoType.InfoWindow, bool isLocked = false)
        {
            if (PrefabsLocations.TryGetValue(infoType, out string itemInfoPath))
            {
                var itemPanelObject = await GameModule.Resource.LoadAssetAsync<GameObject>(itemInfoPath);

                if (itemPanelObject)
                {
                    var panelItemObject = Object.Instantiate(itemPanelObject);
                    if (panelItemObject.TryGetComponent(out ElementInfoItemComponent resultComponent))
                    {
                        FieldElementIconComponent iconPrototype = elementConfig.GetIconComponent();

                        var icon = Object.Instantiate(iconPrototype, panelItemObject.transform);
                        var iconRectTransform = icon.GetComponent<RectTransform>();

                        iconPrototype.SetBlocked(isLocked);

                        resultComponent.SetIcon(iconRectTransform, isLocked);
                        resultComponent.elemId = Int32.Parse(elementConfig.Id);
                    }

                    return resultComponent;
                }
            }

            return null;
        }
        
        public async UniTask<ElementInfoItemComponent> CreateUnknownElementWindowItem(ElementInfoType infoType = ElementInfoType.InfoWindow)
        {
            if (PrefabsLocations.TryGetValue(infoType, out string itemInfoPath))
            {
                var itemPanelObject = await GameModule.Resource.LoadAssetAsync<GameObject>(itemInfoPath);

                if (itemPanelObject)
                {
                    var panelItemObject = Object.Instantiate(itemPanelObject);
                    if (panelItemObject.TryGetComponent(out ElementInfoItemComponent resultComponent))
                    {
                        resultComponent.SetUnknown();
                    }

                    return resultComponent;
                }
            }

            return null;
        }
    }
}