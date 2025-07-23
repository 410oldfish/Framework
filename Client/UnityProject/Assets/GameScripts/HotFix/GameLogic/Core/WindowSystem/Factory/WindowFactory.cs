// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.Helpers;
using MergeIt.Core.WindowSystem.Data;
using MergeIt.Core.WindowSystem.Windows;
using TEngine;
using UnityEngine;
using YooAsset;

namespace MergeIt.Core.WindowSystem.Factory
{
    public class WindowFactory : IWindowFactory
    {
        private const string RootPath = "WindowsRoot";
        private const string Blackout = "Blackout";

        public async UniTask<RectTransform> GetRoot()
        {
            var root = await GameModule.Resource.LoadAssetAsync<GameObject>(RootPath);

            if (root)
            {
                Canvas rootCanvas = root.GetComponent<Canvas>();
                rootCanvas.worldCamera = Camera.main;
                GameObject gameObject = Object.Instantiate(root.gameObject);

                return gameObject.GetComponent<RectTransform>();
            }

            return null;
        }

        public async UniTask<BlackoutComponent> GetBlackout(RectTransform parent)
        {
            var blackoutObject = await GameModule.Resource.LoadAssetAsync<GameObject>(Blackout);

            if (blackoutObject)
            {
                GameObject gameObject = Object.Instantiate(blackoutObject, parent);

                if (gameObject)
                {
                    if (gameObject.TryGetComponent(out RectTransform rectTransform))
                    {
                        rectTransform.Stretch();
                        rectTransform.SetAsFirstSibling();
                    }
                }

                gameObject.TryGetComponent(out BlackoutComponent blackoutComponent);

                return blackoutComponent;
            }

            return null;
        }

        public async UniTask<TPresenter> CreateWindow<TPresenter>(WindowCreateInfo createInfo, Transform parent, IWindowArgs windowArgs)
            where TPresenter : class, IWindowPresenter, new()
        {
            if (!string.IsNullOrEmpty(createInfo.PrefabPath))
            {
                var presenter = new TPresenter();
                GameObject windowPrototype =
                    await GameModule.Resource.LoadAssetAsync<GameObject>(createInfo.PrefabPath);
                GameObject windowObject = Object.Instantiate(windowPrototype, parent);

                if ((windowObject ? windowObject.GetComponent(createInfo.Type) : null) is IWindow window)
                {
                    window.Initialize();

                    presenter.Initialize(window, createInfo.LayerName, windowArgs);
                    presenter.SetWindowLayer();
                    presenter.SetWindowActive(false);
                }
                
                return presenter;
            }

            return null;
        } 
    }
}