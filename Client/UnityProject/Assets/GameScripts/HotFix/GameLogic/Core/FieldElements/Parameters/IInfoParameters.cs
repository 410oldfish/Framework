// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Types;

namespace MergeIt.Core.FieldElements
{
    public interface IInfoParameters
    {
        int Id { get; set; }
        GridPoint LogicPosition { get; set; }
        bool IsBlocked { get; set; }
        int SandCount { get; set; }
        public ElementType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}