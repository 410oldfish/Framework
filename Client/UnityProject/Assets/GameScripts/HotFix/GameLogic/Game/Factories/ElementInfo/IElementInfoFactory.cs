// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.Configs.Elements;
using MergeIt.Game.Windows.ElementInfo;

namespace MergeIt.Game.Factories.ElementInfo
{
    public interface IElementInfoFactory
    {
        UniTask<ElementInfoItemComponent> CreateElementWindowItem(ElementConfig elementConfig, ElementInfoType infoType = ElementInfoType.InfoWindow, bool isLocked = false);

        UniTask<ElementInfoItemComponent> CreateUnknownElementWindowItem(ElementInfoType infoType = ElementInfoType.InfoWindow);
    }
}