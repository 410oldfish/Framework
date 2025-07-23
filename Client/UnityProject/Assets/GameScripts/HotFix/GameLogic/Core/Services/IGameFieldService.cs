// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Core.FieldElements;

namespace MergeIt.Core.Services
{
    public interface IGameFieldService
    {
        GridPoint? GetFreeCell(GridPoint centerPoint = new GridPoint());
        IFieldElement CreateNewElement(ElementConfig config, GridPoint point, bool blocked = false);
    }
}