// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.WindowSystem.Data;
using MergeIt.Core.WindowSystem.Windows;
using UnityEngine;

namespace MergeIt.Core.WindowSystem.Factory
{
    public interface IWindowFactory
    {
        UniTask<RectTransform> GetRoot();
        UniTask<BlackoutComponent> GetBlackout(RectTransform parent);
        UniTask<TPresenter> CreateWindow<TPresenter>(WindowCreateInfo prefabPath, Transform parent, IWindowArgs windowArgs = null)
            where TPresenter : class, IWindowPresenter, new();
    }
}