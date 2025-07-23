// Copyright (c) 2024, Awessets

using Cysharp.Threading.Tasks;
using MergeIt.Core.FieldElements;
using MergeIt.Game.Field;
using UnityEngine;

namespace MergeIt.Game.Factories.Field
{
    public interface IFieldFactory
    {
        UniTask<FieldPresenter> CreateField(Transform parent);
        UniTask<FieldCellComponent> CreateFieldCell(Transform parent, GridPoint gridPoint);
    }
}