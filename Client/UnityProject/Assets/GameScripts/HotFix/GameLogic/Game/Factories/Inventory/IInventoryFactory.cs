// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.FieldElements;
using MergeIt.Game.UI.InventoryPanel;
using MergeIt.Game.Windows.Inventory;
using UnityEngine;

namespace MergeIt.Game.Factories.Inventory
{
    public interface IInventoryFactory
    {
        UniTask<InventoryPanelItemComponent> CreateInventoryPanelItem(IFieldElement fieldElement);
        UniTask<InventoryWindowItemComponent> CreateInventoryWindowItem(IFieldElement fieldElement);
        UniTask<InventoryWindowPaidCellComponent> CreateWindowPaidCell();
        UniTask<GameObject> CreateWindowEmptyCell();
    }
}